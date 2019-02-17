using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

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
        private MenuItem _exitMenu;
        private MenuItem _toggleShowHideMenu;
        private MenuItem _toggleSingleCpuMenu;
        private MenuItem _updInsane;
        private MenuItem _updHalfSecond;
        private MenuItem _updOneSecond;
        private MenuItem _updTwoSeconds;
        private MenuItem _updThreeSeconds;

        private int _processors;

        private MiniChart _miniChart;
        private Panel _miniChartPanel;
        private MiniChart[] _miniChartCpuId;
        private bool _internalExit;
        private Timer _timer;
        private bool _totalCpuMode;

        TableLayoutPanel _multiCpuPanel;

        #endregion

        public FormMain()
        {

            InitializeComponent();
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

        private void BuildMenu()
        {
            _menu = new ContextMenu();

            _exitMenu = new MenuItem("Exit");
            _exitMenu.Click += (o, e) =>
            {
                _internalExit = true;
                Application.Exit();
            };

            _toggleShowHideMenu = new MenuItem();
            _toggleShowHideMenu.Click += (o, e) => ToggleWindowVisibility();

            MenuItem upd = new MenuItem("Update Interval");
            _updInsane = new MenuItem("1/4 second");
            _updInsane.Click += (o, e) => _timer.Interval = 250;
            _updHalfSecond = new MenuItem("1/2 second");
            _updHalfSecond.Click += (o, e) => _timer.Interval = 500;
            _updOneSecond = new MenuItem("1 second");
            _updOneSecond.Click += (o, e) => _timer.Interval = 1000;
            _updTwoSeconds = new MenuItem("2 second");
            _updTwoSeconds.Click += (o, e) => _timer.Interval = 2000;
            _updThreeSeconds = new MenuItem("3 second");
            _updThreeSeconds.Click += (o, e) => _timer.Interval = 3000;

            _toggleSingleCpuMenu = new MenuItem();
            _toggleSingleCpuMenu.Click += (o, e) =>
            {
                _totalCpuMode = !_totalCpuMode;
                UpdateVisualizationMode();
            };

            upd.MenuItems.Add(_updInsane);
            upd.MenuItems.Add(_updHalfSecond);
            upd.MenuItems.Add(_updOneSecond);
            upd.MenuItems.Add(_updTwoSeconds);
            upd.MenuItems.Add(_updThreeSeconds);

            _menu.MenuItems.Add(_toggleShowHideMenu);
            _menu.MenuItems.Add(upd);
            _menu.MenuItems.Add(_toggleSingleCpuMenu);
            _menu.MenuItems.Add(_exitMenu);
            _menu.Popup += (o, e) =>
            {
                _toggleShowHideMenu.Text = Visible ? "Hide CPU chart" : "Show CPU chart";
                _toggleSingleCpuMenu.Text = _totalCpuMode ? "Show separate CPUs" : "Show Total CPU usage";
            };
            _trayIcon.ContextMenu = _menu;
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
        }

        private void UpdateVisualizationMode()
        {
            Controls.Clear();
            if (_totalCpuMode)
            {
                if(_miniChartPanel == null)
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
