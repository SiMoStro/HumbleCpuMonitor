using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Config
{
    public partial class ConfigurationForm : Form
    {
        #region [private]

        private List<Panel> _valueColorPanels = new List<Panel>();
        private List<Panel> _allPanels = new List<Panel>();
        private static ConfigurationForm _instance;
        private ShortcutsControl _shortcuts;
        private static Point? _lastLocation;

        #endregion

        /// <summary>
        /// The configuration form has closed
        /// </summary>
        internal static event EventHandler ConfigurationFormClosed;
        
        /// <summary>
        /// The shortcuts definitions have changed
        /// </summary>
        internal static event EventHandler ShortcutsUpdated;

        public ConfigurationForm()
        {
            InitializeComponent();
            _shortcuts = new ShortcutsControl
            {
                Dock = DockStyle.Fill
            };
            _shortcuts.Apply += HandleShortcutApply;
            w_tabPageShortcuts.Controls.Add(_shortcuts);
            _shortcuts.Initialize(ShortcutManager.Instance.Items);

            Initialize();
        }

        private void HandleShortcutApply(object sender, EventArgs e)
        {
            ShortcutManager.Instance.UpdateShortcuts(_shortcuts.Items);
            ShortcutsUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void Initialize()
        {
            _valueColorPanels.AddRange(new List<Panel> { w_pnl0, w_pnl1, w_pnl2, w_pnl3, w_pnl4, w_pnl5, w_pnl6, w_pnl7, w_pnl8, w_pnl9 });
            for (int i = 0; i < ScenarioManager.Instance.Configuration.Colors.Length; i++)
            {
                _valueColorPanels[i].BackColor = ScenarioManager.Instance.Configuration.Colors[i];
            }
            w_pnlForeground.BackColor = ScenarioManager.Instance.Configuration.Foreground;
            w_pnlBackground.BackColor = ScenarioManager.Instance.Configuration.Background;
            w_pnlChartLines.BackColor = ScenarioManager.Instance.Configuration.ChartLines;

            w_cbMainChartTopmost.Checked = ScenarioManager.Instance.Configuration.MainChartTopmost;
            w_cbProcessChartTopmost.Checked = ScenarioManager.Instance.Configuration.ProcessChartTopmost;
            w_cbTopProcessesTopmost.Checked = ScenarioManager.Instance.Configuration.TopProcessesTopmost;
            w_cbMachineInfoTopmost.Checked = ScenarioManager.Instance.Configuration.MachineInfoTopmost;

            _allPanels.AddRange(_valueColorPanels);
            _allPanels.Add(w_pnlForeground);
            _allPanels.Add(w_pnlBackground);
            _allPanels.Add(w_pnlChartLines);
            foreach (Panel p in _allPanels) p.Click += HandlePanelClick;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if(_lastLocation.HasValue)
            {
                Location = _lastLocation.Value;
            }
        }

        private void HandlePanelClick(object sender, EventArgs e)
        {
            Panel pnl = (Panel)sender;
            ColorDialog dlg = new ColorDialog();
            int[] cc = new int[ScenarioManager.Instance.Configuration.Defaults.Length];
            for (int i = 0; i < ScenarioManager.Instance.Configuration.Defaults.Length; i++)
            {
                Color color = ScenarioManager.Instance.Configuration.Defaults[i];
                int color_value = (color.B << 16) + (color.G << 8) + color.R;
                cc[i] = color_value;
            }
            dlg.CustomColors = cc;
            dlg.Color = pnl.BackColor;
            dlg.ShowDialog();
            pnl.BackColor = dlg.Color;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _lastLocation = new Point(Location.X, Location.Y);
            _shortcuts.Apply -= HandleShortcutApply;
            List<Color> newColors = new List<Color>();
            foreach (Panel p in _valueColorPanels) newColors.Add(p.BackColor);
            ScenarioManager.Instance.SetNewColors(newColors);
            ScenarioManager.Instance.Configuration.Background = w_pnlBackground.BackColor;
            ScenarioManager.Instance.Configuration.Foreground = w_pnlForeground.BackColor;
            ScenarioManager.Instance.Configuration.ChartLines = w_pnlChartLines.BackColor;
            ScenarioManager.Instance.Configuration.MainChartTopmost = w_cbMainChartTopmost.Checked;
            ScenarioManager.Instance.Configuration.ProcessChartTopmost = w_cbProcessChartTopmost.Checked;
            ScenarioManager.Instance.Configuration.TopProcessesTopmost = w_cbTopProcessesTopmost.Checked;
            ScenarioManager.Instance.Configuration.MachineInfoTopmost = w_cbMachineInfoTopmost.Checked;
            _instance = null;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            ConfigurationFormClosed?.Invoke(this, EventArgs.Empty);
        }

        internal static void ShowConfig()
        {
            if (_instance != null) return;
            _instance = new ConfigurationForm
            {
                TopMost = true
            };
            _instance.ShowDialog();
        }

        private void HandleButtonOkClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
