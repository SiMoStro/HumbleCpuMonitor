using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using HumbleCpuMonitor.Config;

namespace HumbleCpuMonitor
{
    public partial class TopCpuProcesses : Form
    {
        #region [private] Objects and vars

        private Label[] _cpu;
        private Label[] _procs;
        private Timer _timer;
        private bool _isLight;
        private MouseMessageFilter _mouseHandler;

        #endregion

        public TopCpuProcesses()
        {
            InitializeComponent();

            _mouseHandler = new MouseMessageFilter(Handle);
            _mouseHandler.LeftButtonDoubleClick = new Action(() =>
            {
                _isLight = !_isLight;
                SetColors();
            });
            Application.AddMessageFilter(_mouseHandler);

            _cpu = new Label[]
            {
                w_lblCpu1,
                w_lblCpu2,
                w_lblCpu3,
                w_lblCpu4,
                w_lblCpu5,
            };
            _procs = new Label[]
            {
                w_lblProcName1,
                w_lblProcName2,
                w_lblProcName3,
                w_lblProcName4,
                w_lblProcName5,
            };

            Snapshot();
            _timer = new Timer
            {
                Interval = 1000
            };
            _timer.Tick += (s, a) => Snapshot();
            _timer.Start();

            AlignPropertiesToConfig();
            ConfigurationForm.ConfigurationFormClosed += HandleConfigurationFormClosed;
        }

        private void HandleConfigurationFormClosed(object sender, EventArgs e)
        {
            AlignPropertiesToConfig();
        }

        private void AlignPropertiesToConfig()
        {
            if (ScenarioManager.Instance.Configuration.TopProcsInfoLocation.HasValue)
            {
                StartPosition = FormStartPosition.Manual;
                Location = ScenarioManager.Instance.Configuration.TopProcsInfoLocation.Value;
            }

            TopMost = ScenarioManager.Instance.Configuration.TopProcessesTopmost;
        }

        private void SetColors()
        {
            if (_isLight)
            {
                BackColor = Color.FromKnownColor(KnownColor.White);
                ForeColor = Color.FromKnownColor(KnownColor.Black);
            }
            else
            {
                BackColor = Color.FromKnownColor(KnownColor.Black);
                ForeColor = Color.FromKnownColor(KnownColor.White);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ConfigurationForm.ConfigurationFormClosed -= HandleConfigurationFormClosed;
            _timer.Stop();
            SaveLocation();
            base.OnClosing(e);
        }

        internal void SaveLocation()
        {
            if (!Visible) return;
            ScenarioManager.Instance.Configuration.TopProcsInfoX = Location.X;
            ScenarioManager.Instance.Configuration.TopProcsInfoY = Location.Y;
        }

        private void Snapshot()
        {
            const int procsCount = 5;
            Process.ProcessDescriptor[] procs = FormMain.Main.GetOverallTopProc(5);
            int num = 0;

            if (procs != null)
            {
                num = Math.Min(procsCount, procs.Length);
            }

            for (int i = 0; i < num; i++)
            {
                _procs[i].Text = $"[{procs[i].Pid}] {procs[i].Name}";
                _cpu[i].Text = procs[i].Snapshot.OverallCpuPerc.ToString("P");
            }

            for (int j = num; j < procsCount; j++)
            {
                _procs[j].Text = _cpu[j].Text = "";
            }
        }
    }
}
