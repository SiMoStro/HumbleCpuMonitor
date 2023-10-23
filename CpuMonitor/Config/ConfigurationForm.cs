using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Config
{
    public partial class ConfigurationForm : Form
    {
        List<Panel> _panels = new List<Panel>();

        public ConfigurationForm()
        {
            InitializeComponent();
            _panels.Add(w_pnl0);
            _panels.Add(w_pnl1);
            _panels.Add(w_pnl2);
            _panels.Add(w_pnl3);
            _panels.Add(w_pnl4);
            _panels.Add(w_pnl5);
            _panels.Add(w_pnl6);
            _panels.Add(w_pnl7);
            _panels.Add(w_pnl8);
            _panels.Add(w_pnl9);
            foreach (Panel p in _panels) p.Click += HandlePanelClick;
            Initialize();
        }
        
        public void Initialize()
        {
            for(int i = 0; i < ScenarioManager.Instance.Configuration.Colors.Length; i++)
            {
                _panels[i].BackColor = ScenarioManager.Instance.Configuration.Colors[i];
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
            List<Color> newColors = new List<Color>();
            foreach (Panel p in _panels) newColors.Add(p.BackColor);
            ScenarioManager.Instance.SetNewColors(newColors);
            _instance = null;
        }

        static ConfigurationForm _instance;

        internal static void ShowConfig()
        {
            if (_instance != null) return;
            _instance = new ConfigurationForm();
            _instance.ShowDialog();
        }

        private void HandleButtonOkClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
