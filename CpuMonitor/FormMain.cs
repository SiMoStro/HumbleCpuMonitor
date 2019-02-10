using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

namespace CpuMonitor
{
    public partial class FormMain : Form
    {
        #region [private]

        private NotifyIcon _trayIcon;
        private PerformanceCounter _theCPUCounter;
        private bool _allowsHowDisplay = false;
        private Icon[] _icons;
        private float _cpuUsage;
        private int _cpuIconIndex;
        private const int StepSize = 7;

        private ContextMenu _menu;
        private MenuItem _exitMenu;
        private MenuItem _toggleShowhideMenu;
        private MenuItem _updInsane;
        private MenuItem _updHalfSecond;
        private MenuItem _updOneSecond;
        private MenuItem _updTwoSeconds;
        private MenuItem _updThreeSeconds;

        private MiniChart _miniChart;
        private bool _internalExit;
        private Timer _timer;

        #endregion

        public FormMain()
        {
            InitializeComponent();
            _icons = new Icon[15];
            _miniChart = new MiniChart();
            _miniChart.Dock = DockStyle.Fill;
            Controls.Add(_miniChart);

            Application.ApplicationExit += HandleApplicationExit;

            _theCPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _trayIcon = new NotifyIcon();
            _trayIcon.DoubleClick += (o, e) => ToggleWindowVisibility();

            LoadIcons();
            BuildMenu();
            UpdateTrayIcon();

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += HandleTick;
            _timer.Start();
        }

        private void LoadIcons()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            for (int i = 0; i < 15; i++)
            {
                string icoName = "CpuMonitor.ICOs." + i.ToString("00") + "-ico.ico";
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

            _toggleShowhideMenu = new MenuItem();
            _toggleShowhideMenu.Click += (o, e) => ToggleWindowVisibility();



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
            upd.MenuItems.Add(_updInsane);
            upd.MenuItems.Add(_updHalfSecond);
            upd.MenuItems.Add(_updOneSecond);
            upd.MenuItems.Add(_updTwoSeconds);
            upd.MenuItems.Add(_updThreeSeconds);

            _menu.MenuItems.Add(_toggleShowhideMenu);
            _menu.MenuItems.Add(upd);
            _menu.MenuItems.Add(_exitMenu);
            _menu.Popup += (o, e) =>
            {
                _toggleShowhideMenu.Text = Visible ? "Hide CPU chart" : "Show CPU chart";
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
            if(_trayIcon.Icon != _icons[_cpuIconIndex]) _trayIcon.Icon = _icons[_cpuIconIndex];
            if(!_trayIcon.Visible) _trayIcon.Visible = true;
        }

        private void HandleApplicationExit(object sender, EventArgs e)
        {
            _trayIcon.Visible = false;
        }

        private void HandleTick(object sender, EventArgs e)
        {
            _cpuUsage = _theCPUCounter.NextValue();
            _miniChart.AddValue(_cpuUsage);
            _cpuIconIndex = (int)Math.Floor(_cpuUsage / 7);
            if (_cpuIconIndex > 14) _cpuIconIndex = 14;
            _trayIcon.Text = (_cpuUsage / 100).ToString("P");
            UpdateTrayIcon();
        }
    }
}
