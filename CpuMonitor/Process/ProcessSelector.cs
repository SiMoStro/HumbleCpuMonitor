using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using HumbleCpuMonitor.Process;

namespace HumbleCpuMonitor
{
    internal partial class ProcessSelector : Form
    {
        private IReadOnlyList<ProcessDescriptor> _procs;

        public int SelectedPid { get; private set; }

        public string SelectedProcessExecutable { get; private set; }

        public ProcessSelector()
        {
            InitializeComponent();
            _tbFilter.TextChanged += HandleFilterChanged;
            _tbFilter.Select();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _tbFilter.Select();
        }

        private void HandleFilterChanged(object sender, EventArgs e)
        {
            UpdateItems(_tbFilter.Text);
        }

        public void Initialize(IReadOnlyList<ProcessDescriptor> processes)
        {
            _procs = processes;
            _tbFilter.Text = string.Empty;
            UpdateItems(string.Empty);
        }

        private void UpdateItems(string filter)
        {
            lvProcesses.Items.Clear();
            var filtered = _procs.Where(p => p.Name.Contains(filter)).OrderBy(n => n.Name);
            foreach (ProcessDescriptor pd in filtered)
            {
                ListViewItem lvi = new ListViewItem(new string[] { pd.Pid.ToString(), pd.Name });
                lvProcesses.Items.Add(lvi);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            OkSelected();
        }

        private void OkSelected()
        {
            SelectedPid = 0;
            if (lvProcesses.SelectedItems.Count == 1)
            {
                ListViewItem item = lvProcesses.SelectedItems[0];
                SelectedProcessExecutable = item.SubItems[1].Text;
                int tryInt;
                if (int.TryParse(item.Text, out tryInt)) SelectedPid = tryInt;
            }
            Close();
        }

        private void ListViewMouseDoubleClick(object sender, MouseEventArgs e)
        {
            OkSelected();
        }
    }
}
