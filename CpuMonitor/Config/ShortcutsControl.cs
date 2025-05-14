using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Config
{
    public partial class ShortcutsControl : UserControl
    {
        private List<LauncherItem> _items;
        private LauncherItem _currentItem;
        private List<TextBox> _tbList = new List<TextBox>();

        public LauncherItem[] Items
        {
            get
            {
                return _items.ToArray();
            }
        }

        public event EventHandler Apply;

        public ShortcutsControl()
        {
            InitializeComponent();

            if (DesignMode) return;

            _items = new List<LauncherItem>();
            _tbList = new List<TextBox>(new TextBox[] { w_tbName, w_tbDescription, w_tbExecutable, w_tbWorkDir, w_tbFiling, w_tbParameters });
            w_lvItems.MultiSelect = false;
            foreach (var tb in _tbList)
            {
                tb.LostFocus += HandleTextBoxLostFocus;
            }
            
            w_btnRemoveItem.Enabled = false;
            EnableTextWidgets(false);
        }

        private void EnableTextWidgets(bool mode)
        {
            foreach (var tb in _tbList) tb.Enabled = mode;
            w_cbAdmin.Enabled = mode;
            w_btnExe.Enabled = mode;
        }

        private void HandleTextBoxLostFocus(object sender, EventArgs e)
        {
            if (_currentItem == null) return;
            UpdateCurrentItem();
            if (sender == w_tbName)
            {
                ListViewItem lvi = FindListViewItem(_currentItem.Id);
                if (lvi == null) return;
                if (lvi.Text != w_tbName.Text) lvi.Text = w_tbName.Text;
            }
            else if(sender == w_tbWorkDir)
            {
                string dir = w_tbWorkDir.Text;
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    w_errorProvider.SetError(w_tbWorkDir, "Directory does not exist");
                }
                else
                {
                    w_errorProvider.SetError(w_tbWorkDir, string.Empty);
                    w_errorProvider.Clear();
                }
            }
            else if (sender == w_tbExecutable)
            {
                string exe = w_tbExecutable.Text;
                if (!string.IsNullOrEmpty(exe) && !File.Exists(exe))
                {
                    w_errorProvider.SetError(w_tbExecutable, "File does not exist");
                }
                else
                {
                    w_errorProvider.SetError(w_tbExecutable, string.Empty);
                }
            }
        }

        private ListViewItem FindListViewItem(string id)
        {
            foreach(ListViewItem lvi in w_lvItems.Items)
            {
                LauncherItem conf = (LauncherItem)lvi.Tag;
                if (conf.Id == id) return lvi;
            }
            return null;
        }

        internal void Initialize(List<LauncherItem> items)
        {
            _items.Clear();
            if (items == null) return;

            foreach (var item in items.OrderBy(k => k.Name))
            {
                AddItem(item.Clone());
            };

            if(w_lvItems.Items.Count > 0)
            {
                w_lvItems.Items[0].Selected = true;
            }
        }

        private void AddItem(LauncherItem item, bool focus = false)
        {
            ListViewItem lvi = new ListViewItem(item.Name)
            {
                Tag = item
            };
            w_lvItems.Items.Add(lvi);
            _items.Add(item);

            w_btnRemoveItem.Enabled = true;
            if (focus)
            {
                BeginInvoke(new Action(() =>
                {
                    lvi.Selected = true;
                    lvi.EnsureVisible();
                }
                ));                
            }
        }

        private void ShowLauncherItem(LauncherItem item)
        {
            w_tbName.Text = item.Name;
            w_tbDescription.Text = item.Description;
            w_tbFiling.Text = item.Filing;
            w_tbExecutable.Text = item.Executable;
            w_tbWorkDir.Text = item.WorkDir;
            w_tbParameters.Text = item.Params;
            w_cbAdmin.Checked = item.RunAsAdmin;
            EnableTextWidgets(true);
        }

        private void UpdateCurrentItem()
        {
            if (_currentItem == null) return;

            _currentItem.Name = w_tbName.Text;
            _currentItem.Description = w_tbDescription.Text;
            _currentItem.Filing = w_tbFiling.Text;
            _currentItem.Executable = w_tbExecutable.Text;
            _currentItem.WorkDir = w_tbWorkDir.Text;
            _currentItem.Params = w_tbParameters.Text;
            _currentItem.RunAsAdmin = w_cbAdmin.Checked;
        }

        private void HandleButtonAddItemClick(object sender, EventArgs e)
        {
            _currentItem = new LauncherItem
            {
                Id = Guid.NewGuid().ToString(),
                Name = GenerateName(),
            };
            ShowLauncherItem(_currentItem);
            AddItem(_currentItem, true);
        }

        private void HandleRemoveItemClick(object sender, EventArgs e)
        {
            if (_currentItem == null) return;

            _items.Remove(_currentItem);

            var lvi = FindListViewItem(_currentItem.Id);
            if (lvi == null) return;

            w_lvItems.Items.Remove(lvi);
            if (w_lvItems.Items.Count == 0)
            {
                w_btnRemoveItem.Enabled = false;
                ClearUserInterface();
                EnableTextWidgets(false);
            }
            else
            {
                w_lvItems.Items[0].Selected = true;
            }
        }

        private void HandleCloneButtonClick(object sender, EventArgs e)
        {
            if (_currentItem == null) return;

            LauncherItem cloned = _currentItem.Clone();
            cloned.Id = Guid.NewGuid().ToString();
            cloned.Name = $"{_currentItem.Name}_cloned";
            AddItem(cloned, true);
        }

        private void ClearUserInterface()
        {
            w_tbName.Text = "";
            w_tbDescription.Text = "";
            w_tbFiling.Text = "";
            w_tbExecutable.Text = "";
            w_tbParameters.Text = "";
        }

        private void HandleButtonApplyClick(object sender, EventArgs e)
        {
            UpdateCurrentItem();
            Apply?.Invoke(this, EventArgs.Empty);
        }

        private string GenerateName()
        {
            int i = 0;
            LauncherItem si = null;
            string name = $"Item_{i}";
            do
            {
                name = $"Item_{i}";
                si = _items.FirstOrDefault(k => name == k.Name);
                i++;
            } while (si != null);

            return name;
        }

        private void ListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (w_lvItems.SelectedItems.Count != 1) return;

            ListViewItem lvi = w_lvItems.SelectedItems[0];
            LauncherItem li = (LauncherItem) lvi.Tag;

            _currentItem = li;
            ShowLauncherItem(_currentItem);
        }

        private void ChoseExeButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Executables|*.exe|All files|*.*";
            dlg.DefaultExt = "exe";
            DialogResult res = dlg.ShowDialog();
            if(res == DialogResult.OK)
            {
                w_tbExecutable.Text = dlg.FileName;
            }

        }

        private void CheckBoxAdminCheckedChanged(object sender, EventArgs e)
        {
            if(_currentItem == null)
            {
                return;
            }

            _currentItem.RunAsAdmin = w_cbAdmin.Checked;
        }

        private void ChooseDirButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                w_tbWorkDir.Text = dlg.SelectedPath;
            }
        }
    }
}
