using HumbleCpuMonitor.Win32;
using System;
using System.Windows.Forms;

namespace HumbleCpuMonitor
{
    public partial class MachineInfo : Form
    {
        private Timer _timer = new Timer();
        private MEMORYSTATUSEX _mem = new MEMORYSTATUSEX();
        private MouseMessageFilter _mouseHandler;

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
        }

        private void HandleTimerTick(object sender, EventArgs e)
        {
            Snapshot();
        }

        private void Snapshot()
        {
            Kernel32.GlobalMemoryStatusEx(_mem);

            double phy = ((double)_mem.ullAvailPhys / _mem.ullTotalPhys) * 100;
            double availPhy = Math.Ceiling(phy);
            w_prgPhy.Value = 100 - (int)availPhy;
            w_prgPhy.Text = (100.0d - phy).ToString("#.00") + "%";

            double cc = ((double)_mem.ullAvailPageFile / _mem.ullTotalPageFile) * 100;
            double availPageFile = Math.Ceiling(cc);
            w_prgPageFile.Value = 100 - (int)availPageFile;
            w_prgPageFile.Text = (100.0d - cc).ToString("#.00") + "%";

            w_prgCpu.Value = (int)FormMain.Main.CpuUsage;
            w_prgCpu.Text= (FormMain.Main.CpuUsage).ToString("#.00") + "%";
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible) _timer.Start();
            else _timer.Stop();
        }
    }
}
