﻿using System;
using System.Windows.Forms;
using HumbleCpuMonitor.Charts;
using HumbleCpuMonitor.Config;
using HumbleCpuMonitor.Win32;

namespace HumbleCpuMonitor
{
    internal class ProcessObserver : IDisposable
    {
        #region [private]

        private IntPtr _hndl;
        private double _lastKt, _lastUt;
        private DateTime _lastSnapshotTime;
        private Timer _timer;
        private MiniBarChart _chart;
        private Panel _panel;
        private string _name;
        private Form _form;
        private long _cycles;
        private System.Diagnostics.Process _process;

        private float _max;
        private DateTime _processStart;

        #endregion

        public double LastKernelPct
        {
            get; private set;
        }

        public double LastUserPct
        {
            get; private set;
        }

        public double LastDelta
        {
            get; private set;
        }

        public int Pid
        {
            get; private set;
        }

        public ProcessObserver(int pid, string name)
        {
            Pid = pid;
            _name = name;
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += HandleTimerTick;
            _hndl = Kernel32.OpenSimple(Pid);

            _chart = new MiniBarChart();

            _process = System.Diagnostics.Process.GetProcessById(pid);
            _process.EnableRaisingEvents = true;
            _process.Exited += HandleProcessExited;
            _processStart = _process.StartTime;
            _chart.Dock = DockStyle.Fill;

            InitForm();
            _form.Show();
            UpdateTitle();
            Start();
            ConfigurationForm.ConfigurationFormClosed += HandleConfigurationFormClosed;
            AlignPropertiesToConfiguration();
        }

        private void HandleConfigurationFormClosed(object sender, EventArgs e)
        {
            AlignPropertiesToConfiguration();
        }

        private void AlignPropertiesToConfiguration()
        {
            _form.TopMost = ScenarioManager.Instance.Configuration.ProcessChartTopmost;
        }

        private void HandleProcessExited(object sender, EventArgs e)
        {
            try
            {
                if (_form == null) return;
                if (_form.InvokeRequired)
                {
                    _form.BeginInvoke(new Action(PostExit));
                    return;
                }
                PostExit();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void PostExit()
        {
            _timer.Stop();

            if(_process != null)
            {
                _process.Exited -= HandleProcessExited;
                _process = null;
            }            
            _form.Text = _name + " (Exited)";
        }

        private void InitForm()
        {
            _form = new Form();
            _panel = new Panel();
            _panel.Dock = DockStyle.Fill;
            _panel.Padding = new Padding(2);
            _form.MaximumSize = new System.Drawing.Size(1800, 200);
            _form.Size = new System.Drawing.Size(1000, 200);
            _form.MinimumSize = new System.Drawing.Size(400, 200);
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;
            _form.Icon = FormMain.GetIcon(10);
            _panel.Controls.Add(_chart);
            _form.Controls.Add(_panel);
            _form.FormClosing += HandleFormClosing;
            _form.FormClosed += HandleFormClosed;
        }

        private void HandleFormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigurationForm.ConfigurationFormClosed -= HandleConfigurationFormClosed;
        }

        private void HandleFormClosed(object sender, FormClosedEventArgs e)
        {
            _form = null;
            Dispose();
        }

        private void Start()
        {
            Tuple<double, double> ku = GetSnapshot();
            _lastSnapshotTime = DateTime.UtcNow;
            _lastKt = ku.Item1;
            _lastUt = ku.Item2;
            _timer.Start();
        }

        public void Stop()
        {
            _form.Close();
        }

        private void HandleTimerTick(object sender, EventArgs e)
        {
            if (_process == null) return;

            var ku = GetSnapshot();
            DateTime time = DateTime.UtcNow;
            LastDelta = time.Subtract(_lastSnapshotTime).TotalMilliseconds;
            LastKernelPct = (ku.Item1 - _lastKt) / LastDelta;
            LastUserPct = (ku.Item2 - _lastUt) / LastDelta;

            _lastKt = ku.Item1;
            _lastUt = ku.Item2;
            _lastSnapshotTime = time;

            float pct = (float)(LastKernelPct + LastUserPct) * 100;
            if (pct > _max) _max = pct;
            _chart.AddValue(pct);

            if (++_cycles % 10 == 0) UpdateTitle();
        }

        DateTime? _prevTitleUpdate;
        double _prevCpuTime;
        private void UpdateTitle()
        {
            _process.Refresh();
            var currentCpuTime = _process.TotalProcessorTime.TotalMilliseconds;
            double avg = 0d;
            if(_prevTitleUpdate.HasValue)
            {
                double deltaTime = DateTime.UtcNow.Subtract(_prevTitleUpdate.Value).TotalMilliseconds;
                avg = ((currentCpuTime - _prevCpuTime) / deltaTime) * 100;
            }

            _form.Text = $" {_name} (WrkSet: {Utilities.FormatBytes(_process.WorkingSet64)}; last interval CPU: Max {_max.ToString("#.00")}% Avg {avg.ToString("#.00")}%)";

            _max = 0;
            _prevTitleUpdate = DateTime.UtcNow;
            _prevCpuTime = currentCpuTime;
        }

        private Tuple<double, double> GetSnapshot()
        {
            System.Runtime.InteropServices.ComTypes.FILETIME p1, p2, p3, p4;
            Kernel32.GetProcessTimes(_hndl, out p1, out p2, out p3, out p4);
            TimeSpan kt = Kernel32.FileTimeToTimeSpan(p3);
            TimeSpan ut = Kernel32.FileTimeToTimeSpan(p4);
            return new Tuple<double, double>(kt.TotalMilliseconds, ut.TotalMilliseconds);
        }

        public void Dispose()
        {
            if (_hndl != IntPtr.Zero)
            {
                Kernel32.CloseHandle(_hndl);
                _hndl = IntPtr.Zero;
            }

            if(_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
