using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;
using HumbleCpuMonitor.Charts;
using HumbleCpuMonitor.Process;

namespace HumbleCpuMonitor
{
    internal partial class FormMain : Form
    {
        #region [private]

        private NotifyIcon _trayIcon;
        private PerformanceCounter _theCPUCounter;
        private PerformanceCounter[] _cpuIdCounter;
        private bool _allowsHowDisplay = false;
        private Icon[] _icons;
        private int _iconSelectionStep;
        private float _cpuUsage;
        private int _cpuIconIndex;
        private ChartType _chartMode;

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

        private MenuItem _miUseBarChart;
        private MenuItem _miUseLineChart;
        private MenuItem _miUseScatterChart;
        private MenuItem _miUseFullColorChart;

        private ProcessSelector _processSelector;

        private MenuItem _selectProcess;

        private int _processors;

        private MiniChartBase _miniChart;
        private Panel _miniChartPanel;
        private MiniChartBase[] _miniChartCpuId;
        private bool _internalExit;
        private Timer _timer;
        private bool _totalCpuMode;
        private string _title = "Humble CPU Monitor";
        private long _cycles;

        private TableLayoutPanel _multiCpuPanel;

        private Processes _processes;
        private System.Diagnostics.Process _self;

        MachineInfo _machineInfo;

        private MouseMessageFilter _mouseHandler;
        int? _borderSize = null;
        int? _captionSize = null;

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
            _self = System.Diagnostics.Process.GetCurrentProcess();

            _processes = new Processes();
            UpdateTitle();

            _icons = new Icon[15];
            _iconSelectionStep = 100 / 15;

            Application.ApplicationExit += HandleApplicationExit;

            _processors = Environment.ProcessorCount;
            _cpuIdCounter = new PerformanceCounter[_processors];
            for (int p = 0; p < _processors; p++)
            {
                _cpuIdCounter[p] = new PerformanceCounter("Processor", "% Processor Time", p.ToString());
            }

            _theCPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _trayIcon = new NotifyIcon();
            _trayIcon.DoubleClick += (o, e) => ToggleWindowVisibility();

            LoadIcons();
            BuildMenu();
            UpdateTrayIcon();

            _totalCpuMode = true;
            SwitchChartMode(ChartType.Bar);

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += HandleTick;
            _timer.Start();
        }

        private void RebuildCharts()
        {
            MiniChartBase oldChart = _miniChart;
            MiniChartBase[] oldGridChart = _miniChartCpuId;

            switch (_chartMode)
            {
                case ChartType.Scatter:
                    _miniChart = new MiniScatterChart { HorizontalLines = 9 };
                    _miniChartCpuId = new MiniScatterChart[_processors];
                    for (int p = 0; p < _processors; p++)
                    {
                        _miniChartCpuId[p] = new MiniScatterChart();
                    }
                    break;
                case ChartType.Bar:
                    _miniChart = new MiniBarChart { HorizontalLines = 9 };
                    _miniChartCpuId = new MiniBarChart[_processors];
                    for (int p = 0; p < _processors; p++)
                    {
                        _miniChartCpuId[p] = new MiniBarChart();
                    }
                    break;
                case ChartType.Line:
                    _miniChart = new MiniLineChart { HorizontalLines = 9 };
                    _miniChartCpuId = new MiniLineChart[_processors];
                    for (int p = 0; p < _processors; p++)
                    {
                        _miniChartCpuId[p] = new MiniLineChart();
                    }
                    break;
                case ChartType.FullColor:
                    _miniChart = new MiniFullColor { HorizontalLines = 0 };
                    _miniChartCpuId = new MiniFullColor[_processors];
                    for (int p = 0; p < _processors; p++)
                    {
                        _miniChartCpuId[p] = new MiniFullColor();
                    }
                    break;
            }
            if(oldChart != null)
            {
                _miniChart.InitValues(oldChart.Values);
            }
            
            if(oldGridChart != null)
            {
                for (int p = 0; p < _processors; p++)
                {
                    _miniChartCpuId[p].InitValues(oldGridChart[p].Values);
                }
            }

            UpdateVisualizationMode(false);
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

            MenuItem cType = new MenuItem("Chart type");
            _miUseBarChart = new MenuItem("Bar chart");
            _miUseBarChart.Click += (o, e) => SwitchChartMode(ChartType.Bar);
            _miUseLineChart = new MenuItem("Line chart");
            _miUseLineChart.Click += (o, e) => SwitchChartMode(ChartType.Line);
            _miUseScatterChart = new MenuItem("Scatter chart");
            _miUseScatterChart.Click += (o, e) => SwitchChartMode(ChartType.Scatter);
            _miUseFullColorChart = new MenuItem("FullColor chart");
            _miUseFullColorChart.Click += (o, e) => SwitchChartMode(ChartType.FullColor);
            cType.MenuItems.Add(_miUseBarChart);
            cType.MenuItems.Add(_miUseLineChart);
            cType.MenuItems.Add(_miUseScatterChart);
            cType.MenuItems.Add(_miUseFullColorChart);

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
            _menu.MenuItems.Add(cType);
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

        private void SwitchChartMode(ChartType chartMode)
        {
            _miUseBarChart.Checked = _miUseScatterChart.Checked = _miUseLineChart.Checked = _miUseFullColorChart.Checked= false;
            _chartMode = chartMode;
            switch (chartMode)
            {
                case ChartType.Unknown:
                    return;
                case ChartType.Bar:
                    _miUseBarChart.Checked = true;
                    break;
                case ChartType.Line:
                    _miUseLineChart.Checked = true;
                    break;
                case ChartType.Scatter:
                    _miUseScatterChart.Checked = true;
                    break;
                case ChartType.FullColor:
                    _miUseFullColorChart.Checked = true;
                    break;
            }
            RebuildCharts();
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

        private void UpdateVisualizationMode(bool restart = true)
        {
            Controls.Clear();
            if (_totalCpuMode)
            {
                if (_miniChartPanel == null)
                {
                    _miniChartPanel = new Panel
                    {
                        Padding = new Padding(1)
                    };
                }
                _miniChartPanel.Controls.Clear();
                _miniChart.Margin = new Padding(2);
                _miniChart.Dock = DockStyle.Fill;
                _miniChart.DoubleClick += HandleMiniChartDoubleClick;
                _miniChartPanel.Controls.Add(_miniChart);
                _miniChartPanel.Dock = DockStyle.Fill;
                if(restart) _miniChart.Restart();
                Controls.Add(_miniChartPanel);
            }
            else
            {
                if (_multiCpuPanel == null)
                {
                    _multiCpuPanel = new TableLayoutPanel
                    {
                        CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                        Dock = DockStyle.Fill,
                        RowCount = 2,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        ColumnCount = _processors / 2
                    };
                }

                _multiCpuPanel.Controls.Clear();
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
                    _miniChartCpuId[cpu].DoubleClick += HandleSplittedChartsDoubleClick;
                    _multiCpuPanel.Controls.Add(_miniChartCpuId[cpu], cpu % _multiCpuPanel.ColumnCount, cpu / _multiCpuPanel.ColumnCount);
                }

                if (restart)
                {
                    foreach (MiniChartBase chart in _miniChartCpuId) chart.Restart();
                }
                Controls.Add(_multiCpuPanel);
            }
        }

        private void HandleSplittedChartsDoubleClick(object sender, EventArgs e)
        {
            foreach (MiniBarChart chart in _miniChartCpuId) chart.Restart();
        }

        private void HandleMiniChartDoubleClick(object sender, EventArgs e)
        {
            if(ModifierKeys.HasFlag(Keys.Control))
            {
                FormBorderStyle currentStyle = FormBorderStyle;
                if(currentStyle == FormBorderStyle.Sizable)
                {   // switch to no border
                    _borderSize = Math.Max(0, ((Size.Width - ClientSize.Width) / 2));
                    _captionSize = Size.Height - ClientSize.Height - _borderSize.Value;
                    int desiredHeigth = Math.Max(10, Height - _borderSize.Value - _captionSize.Value);
                    int x = Location.X + _borderSize.Value;
                    int y = Location.Y + _captionSize.Value;
                    FormBorderStyle = FormBorderStyle.None;
                    MinimumSize = new Size(MinimumSize.Width, 10);
                    Location = new Point(x, y);
                    Size = new Size(Width, desiredHeigth);
                }
                else
                {   // switch to border
                    if (_borderSize.HasValue && _captionSize.HasValue)
                    {
                        int x = Location.X - _borderSize.Value;
                        int y = Location.Y - _captionSize.Value;
                        MinimumSize = new Size(400, _captionSize.Value + _borderSize.Value);
                        _borderSize = _captionSize = null;
                        Location = new Point(x, y);
                    }
                    FormBorderStyle = FormBorderStyle.Sizable;
                    
                }
                _mouseHandler = new MouseMessageFilter(Handle);
                Application.AddMessageFilter(_mouseHandler);
            }
            else
            {
                _miniChart.Restart();
                Application.RemoveMessageFilter(_mouseHandler);
                _mouseHandler = null;
            }            
        }

        private void TotalCpuSnapshot()
        {
            _cpuUsage = _theCPUCounter.NextValue();
            _cpuIconIndex = (int)Math.Floor(_cpuUsage / _iconSelectionStep);
            if (_cpuIconIndex > 14) _cpuIconIndex = 14;
            string trayText = (_cpuUsage / 100).ToString("P");

            var topProcs = _processes.GetTopProcessesByCpu(3);
            foreach(var proc in topProcs)
            {
                trayText += $"{Environment.NewLine}{proc.Name}";
            }
            _trayIcon.Text = trayText.Substring(0, Math.Min(trayText.Length, 63));
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
