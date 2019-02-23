using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace HumbleCpuMonitor
{
    public partial class ProcessSelector : Form
    {
        public int SelectedPid { get; private set; }

        public string SelectedProcessExecutable { get; private set; }
        public ProcessSelector()
        {
            InitializeComponent();
        }

        public void Initialize(IReadOnlyList<ProcessDescriptor> processes)
        {
            lvProcesses.Items.Clear();
            foreach (ProcessDescriptor pd in processes.OrderBy(p => p.Name))
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
