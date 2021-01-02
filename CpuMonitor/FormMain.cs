using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;

namespace HumbleCpuMonitor
{
    public partial class FormMain : Form
    {
        #region [private]

        private NotifyIcon _trayIcon;
        private PerformanceCounter _theCPUCounter;
        private PerformanceCounter[] _cpuIdCounter;
        private bool _allowsHowDisplay = false;
        private Icon[] _icons;
        private float _cpuUsage;
        private int _cpuIconIndex;
        private const int StepSize = 7;

        private ContextMenu _menu;
        private MenuItem _miExitMenu;
        private MenuItem _miToggleShowHideMenu;
        private MenuItem _miToggleSingleCpuMenu;
        private MenuItem _miUpdInsane;
        private MenuItem _miUpdHalfSecond;
        private MenuItem _miUpdOneSecond;
        private MenuItem _miUpdTwoSeconds;
        private MenuItem _miUpdThreeSeconds;
        private MenuItem _miMachineInfo;

        private ProcessSelector _processSelector;

        private MenuItem _selectProcess;

        private int _processors;

        private MiniChart _miniChart;
        private Panel _miniChartPanel;
        private MiniChart[] _miniChartCpuId;
        private bool _internalExit;
        private Timer _timer;
        private bool _totalCpuMode;
        private string _title = "Humble CPU Monitor";
        private long _cycles;

        private TableLayoutPanel _multiCpuPanel;

        private Processes _processes;
        private Process _self;

        MachineInfo _machineInfo;

        #endregion

        public float CpuUsage
        {
            get
            {
                return _cpuUsage;
            }
        }

        public static FormMain Main
        {
            get; private set;
        }

        public FormMain()
        {
            InitializeComponent();

            Main = this;

            SuperPower.Enable();
            _self = Process.GetCurrentProcess();

            _processes = new Processes();
            UpdateTitle();

            _icons = new Icon[15];
            _miniChart = new MiniChart();

            Application.ApplicationExit += HandleApplicationExit;

            _processors = Environment.ProcessorCount;
            _cpuIdCounter = new PerformanceCounter[_processors];
            _miniChartCpuId = new MiniChart[_processors];
            for (int p = 0; p < _processors; p++)
            {
                _cpuIdCounter[p] = new PerformanceCounter("Processor", "% Processor Time", p.ToString());
                _miniChartCpuId[p] = new MiniChart();
            }

            _theCPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _trayIcon = new NotifyIcon();
            _trayIcon.DoubleClick += (o, e) => ToggleWindowVisibility();

            LoadIcons();
            BuildMenu();
            UpdateTrayIcon();

            _totalCpuMode = true;
            UpdateVisualizationMode();

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += HandleTick;
            _timer.Start();
        }

        private void UpdateTitle()
        {
            _self.Refresh();
            Text = _title + " (WrkSet: " + Utilities.FormatBytes(_self.WorkingSet64) + ")";
        }

        private void LoadIcons()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            for (int i = 0; i < 15; i++)
            {
                string icoName = "HumbleCpuMonitor.ICOs." + i.ToString("00") + "-ico.ico";
                Stream ico = assembly.GetManifestResourceStream(icoName);
                Icon icon = new Icon(ico);
                _icons[i] = icon;
            }
        }

        public static Icon GetIcon(int idx)
        {
            if (idx < 0) idx = 0;
            if (idx > 14) idx = 14;
            Assembly assembly = Assembly.GetEntryAssembly();
            string icoName = "HumbleCpuMonitor.ICOs." + idx.ToString("00") + "-ico.ico";
            Stream ico = assembly.GetManifestResourceStream(icoName);
            Icon icon = new Icon(ico);
            return icon;
        }

        private void BuildMenu()
        {
            _menu = new ContextMenu();

            _miExitMenu = new MenuItem("Exit");
            _miExitMenu.Click += (o, e) =>
            {
                _internalExit = true;
                Application.Exit();
            };

            _miToggleShowHideMenu = new MenuItem();
            _miToggleShowHideMenu.Click += (o, e) => ToggleWindowVisibility();

            MenuItem upd = new MenuItem("Update Interval");
            _miUpdInsane = new MenuItem("1/4 second");
            _miUpdInsane.Click += (o, e) => _timer.Interval = 250;
            _miUpdHalfSecond = new MenuItem("1/2 second");
            _miUpdHalfSecond.Click += (o, e) => _timer.Interval = 500;
            _miUpdOneSecond = new MenuItem("1 second");
            _miUpdOneSecond.Click += (o, e) => _timer.Interval = 1000;
            _miUpdTwoSeconds = new MenuItem("2 second");
            _miUpdTwoSeconds.Click += (o, e) => _timer.Interval = 2000;
            _miUpdThreeSeconds = new MenuItem("3 second");
            _miUpdThreeSeconds.Click += (o, e) => _timer.Interval = 3000;

            _miToggleSingleCpuMenu = new MenuItem();
            _miToggleSingleCpuMenu.Click += (o, e) =>
            {
                _totalCpuMode = !_totalCpuMode;
                UpdateVisualizationMode();
            };

            _miMachineInfo = new MenuItem("Machine info");
            _miMachineInfo.Click += (o, e) =>
            {
                if (_machineInfo != null)
                {
                    _machineInfo.Close();
                    return;
                }

                _machineInfo = new MachineInfo();
                _machineInfo.FormClosing += (o2, e2) =>
                {
                    _machineInfo = null;
                };
                _machineInfo.Show();
            };

            _selectProcess = new MenuItem("Select process");
            _selectProcess.Click += (o, e) =>
            {
                if (_processSelector != null) return;
                _processes.Update();
                _processSelector = new ProcessSelector();
                _processSelector.Initialize(_processes.Descriptors);
                _processSelector.Show();
                _processSelector.FormClosed += HandleProcessSelectorClosed;
            };

            upd.MenuItems.Add(_miUpdInsane);
            upd.MenuItems.Add(_miUpdHalfSecond);
            upd.MenuItems.Add(_miUpdOneSecond);
            upd.MenuItems.Add(_miUpdTwoSeconds);
            upd.MenuItems.Add(_miUpdThreeSeconds);

            _menu.MenuItems.Add(_miToggleShowHideMenu);
            _menu.MenuItems.Add(upd);
            _menu.MenuItems.Add(_miToggleSingleCpuMenu);
            _menu.MenuItems.Add(_selectProcess);
            _menu.MenuItems.Add(_miMachineInfo);
            _menu.MenuItems.Add(new MenuItem("-"));
            _menu.MenuItems.Add(_miExitMenu);

            _menu.Popup += (o, e) =>
            {
                _miToggleShowHideMenu.Text = Visible ? "Hide CPU chart" : "Show CPU chart";
                _miToggleSingleCpuMenu.Text = _totalCpuMode ? "Show separate CPUs" : "Show Total CPU usage";
                _selectProcess.Enabled = (_processSelector == null);
            };
            _trayIcon.ContextMenu = _menu;
        }

        private void HandleProcessSelectorClosed(object sender, FormClosedEventArgs e)
        {
            ProcessSelector ps = sender as ProcessSelector;
            ps.FormClosed -= HandleProcessSelectorClosed;
            if (ps.SelectedPid != 0)
            {
                ProcessObserver po = new ProcessObserver(ps.SelectedPid, ps.SelectedProcessExecutable);
            }
            _processSelector = null;
        }

        private void ToggleWindowVisibility()
        {
            _allowsHowDisplay = true;
            if (Visible) Hide();
            else Show();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_allowsHowDisplay ? value : _allowsHowDisplay);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_internalExit) e.Cancel = true;
            base.OnClosing(e);
            Hide();
        }

        private void UpdateTrayIcon()
        {
            if (_trayIcon.Icon != _icons[_cpuIconIndex]) _trayIcon.Icon = _icons[_cpuIconIndex];
            if (!_trayIcon.Visible) _trayIcon.Visible = true;
        }

        private void HandleApplicationExit(object sender, EventArgs e)
        {
            _trayIcon.Visible = false;
        }

        private void HandleTick(object sender, EventArgs e)
        {
            TotalCpuSnapshot();
            if (!_totalCpuMode) AllCpuSnapshot();

            if (++_cycles % 10 == 0) UpdateTitle();
            _processes.Update();
        }

        public ProcessDescriptor[] GetTopProc()
        {
            if (_processes.ProcessCount < 3) return null;
            return _processes.Descriptors.OrderByDescending(p => p.Snapshot.CpuPerc).Take(3).ToArray();
        }

        private void UpdateVisualizationMode()
        {
            Controls.Clear();
            if (_totalCpuMode)
            {
                if (_miniChartPanel == null)
                {
                    _miniChartPanel = new Panel();
                    _miniChartPanel.Padding = new Padding(1);
                    _miniChart.Margin = new Padding(2);
                    _miniChart.Dock = DockStyle.Fill;
                    _miniChart.DoubleClickAction = MiniChartDoubleClick;
                    _miniChartPanel.Controls.Add(_miniChart);
                    _miniChartPanel.Dock = DockStyle.Fill;
                }
                _miniChart.Restart();
                Controls.Add(_miniChartPanel);
            }
            else
            {
                if (_multiCpuPanel == null)
                {
                    _multiCpuPanel = new TableLayoutPanel();
                    _multiCpuPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                    _multiCpuPanel.Dock = DockStyle.Fill;
                    _multiCpuPanel.RowCount = 2;
                    _multiCpuPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    _multiCpuPanel.ColumnCount = _processors / 2;

                    for (int row = 0; row < _multiCpuPanel.RowCount; row++)
                    {
                        _multiCpuPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / _multiCpuPanel.RowCount));
                    }

                    for (int col = 0; col < _multiCpuPanel.ColumnCount; col++)
                    {
                        _multiCpuPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / _multiCpuPanel.ColumnCount));
                    }

                    for (int cpu = 0; cpu < _processors; cpu++)
                    {
                        _miniChartCpuId[cpu].Dock = DockStyle.Fill;
                        _miniChartCpuId[cpu].Margin = new Padding(1);
                        _miniChartCpuId[cpu].DoubleClickAction = SplittedChartsDoubleClick;
                        _multiCpuPanel.Controls.Add(_miniChartCpuId[cpu], cpu % _multiCpuPanel.ColumnCount, cpu / _multiCpuPanel.ColumnCount);
                    }
                }
                foreach (MiniChart chart in _miniChartCpuId) chart.Restart();
                Controls.Add(_multiCpuPanel);
            }
        }

        private void SplittedChartsDoubleClick(MiniChart mc)
        {
            foreach (MiniChart chart in _miniChartCpuId) chart.Restart();
        }

        private void MiniChartDoubleClick(MiniChart mc)
        {
            _miniChart.Restart();
        }

        private void TotalCpuSnapshot()
        {
            _cpuUsage = _theCPUCounter.NextValue();
            _cpuIconIndex = (int)Math.Floor(_cpuUsage / 7);
            if (_cpuIconIndex > 14) _cpuIconIndex = 14;
            _trayIcon.Text = (_cpuUsage / 100).ToString("P");
            UpdateTrayIcon();

            if (_totalCpuMode) _miniChart.AddValue(_cpuUsage);
        }

        private void AllCpuSnapshot()
        {
            for (int cpu = 0; cpu < _processors; cpu++)
            {
                float cpuUsage = _cpuIdCounter[cpu].NextValue();
                _miniChartCpuId[cpu].AddValue(cpuUsage);
            }
        }
    }
}
