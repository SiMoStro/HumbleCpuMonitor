using HumbleCpuMonitor.Win32;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace HumbleCpuMonitor
{
    public partial class MachineInfo : Form
    {
        private Timer _timer = new Timer();
        private MEMORYSTATUSEX _mem = new MEMORYSTATUSEX();
        private MouseMessageFilter _mouseHandler;
        private long _counter;

        public MachineInfo()
        {
            InitializeComponent();

            Snapshot();

            _mouseHandler = new MouseMessageFilter(Handle);
            Application.AddMessageFilter(_mouseHandler);

            _timer.Interval = 1000;
            _timer.Tick += HandleTimerTick;
            w_prgPhy.Minimum = w_prgPageFile.Minimum = w_prgCpu.Minimum = 0;
            w_prgPhy.Maximum = w_prgPageFile.Maximum = w_prgCpu.Maximum = 100;

            w_prgProc1.Minimum = w_prgProc2.Minimum = w_prgProc3.Minimum = 0;
            w_prgProc1.Maximum = w_prgProc2.Maximum = w_prgProc3.Maximum = 100;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Application.RemoveMessageFilter(_mouseHandler);
            _mouseHandler = null;
        }

        private void HandleTimerTick(object sender, EventArgs e)
        {
            _counter++;
            Snapshot();
        }

        private void Snapshot()
        {
            if(_counter % 3 == 0) SnapProcesses();

            Kernel32.GlobalMemoryStatusEx(_mem);
            double phy = ((double)_mem.ullAvailPhys / _mem.ullTotalPhys) * 100;
            double availPhy = Math.Ceiling(phy);
            w_prgPhy.Value = 100 - (int)availPhy;
            w_prgPhy.Text = (100.0d - phy).ToString("0.00") + "%";

            double cc = ((double)_mem.ullAvailPageFile / _mem.ullTotalPageFile) * 100;
            double availPageFile = Math.Ceiling(cc);
            w_prgPageFile.Value = 100 - (int)availPageFile;
            w_prgPageFile.Text = (100.0d - cc).ToString("0.00") + "%";

            w_prgCpu.Value = (int)FormMain.Main.CpuUsage;
            w_prgCpu.Text = (FormMain.Main.CpuUsage).ToString("0.00") + "%";
        }

        private void SnapProcesses()
        {
            ProcessDescriptor[] proc = FormMain.Main.GetTopProc();
            if (proc != null)
            {
                w_lblProc1.Text = proc[0].Snapshot.CpuPerc.ToString("0.0");
                w_lblProc1.Update();
                w_lblProc2.Text = proc[1].Snapshot.CpuPerc.ToString("0.0");
                w_lblProc2.Update();
                w_lblProc3.Text = proc[2].Snapshot.CpuPerc.ToString("0.0");
                w_lblProc3.Update();

                int p = (int)proc[0].Snapshot.CpuPerc;
                w_prgProc1.Value = p;
                w_prgProc1.Text = proc[0].Name;

                p = (int)proc[1].Snapshot.CpuPerc;
                w_prgProc2.Value = p;
                w_prgProc2.Text = proc[1].Name;

                p = (int)proc[2].Snapshot.CpuPerc;
                w_prgProc3.Value = p;
                w_prgProc3.Text = proc[2].Name;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible) _timer.Start();
            else _timer.Stop();
        }
    }
}
