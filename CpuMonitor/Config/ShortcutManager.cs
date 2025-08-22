using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Config
{
    internal class ShortcutManager
    {
        private const string LauncherFilename = "HcmLauncher.xml";
        private LauncherConfig _launcherConfig;
        private List<MenuItem> _allMenuItems;

        internal List<LauncherItem> Items => _launcherConfig?.Items.ToList() ?? null;

        #region Singleton

        private static volatile ShortcutManager _instance;
        private static readonly object Locker = new object();

        /// <summary>
        /// True if the ShortcutManager instance is valid, false otherwise
        /// </summary>
        public static bool IsStarted
        {
            get => (_instance != null);
        }

        /// <summary>
        /// The ShortcutManager instance
        /// </summary>
        public static ShortcutManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new ShortcutManager();
                        }
                    }
                }

                return _instance;
            }
        }

        public MenuItem[] RootMenuItems 
        { 
            get; 
            private set;
        }

        private ShortcutManager()
        {
            _launcherConfig = new LauncherConfig();
        }

        #endregion

        public void Initialize()
        {
            _launcherConfig = new LauncherConfig();
            _launcherConfig.LoadData(Utilities.GetConfigFilePath(LauncherFilename));
            UpdateShortcuts(_launcherConfig.Items);
        }

        public void Save()
        {
            _launcherConfig.SaveData(Utilities.GetConfigFilePath(LauncherFilename));
        }

        public void UpdateShortcuts(LauncherItem[] items)
        {
            if(items == null)
            {
                return;
            }

            _launcherConfig.Items = items;
            RebuildMenuItems();
        }

        private void RebuildMenuItems()
        {
            if(_allMenuItems != null)
            {
                foreach (var tmp in _allMenuItems) tmp.Tag = null;
            }

            List<MenuItem> levelZeroItems = new List<MenuItem>();
            _allMenuItems = new List<MenuItem>();

            if (_launcherConfig.Items != null)
            {
                IEnumerable<IGrouping<string, LauncherItem>> groups = _launcherConfig.Items.GroupBy(i => i.Filing);
                foreach(IGrouping<string, LauncherItem> g in groups)
                {
                    string groupName = g.Key;
                    IOrderedEnumerable<LauncherItem> orderedItems = g.OrderBy(k => k.LocalIdx);
                    foreach(LauncherItem li in orderedItems)
                    {
                        GenerateMenuItem(li, levelZeroItems);
                    }
                }
            }

            RootMenuItems = levelZeroItems.ToArray();
        }

        private void GenerateMenuItem(LauncherItem item, List<MenuItem> levelZeroItems)
        {
            MenuItem root = null;
            if (!string.IsNullOrEmpty(item.Filing))
            {
                string[] tokens = item.Filing.Split(";".ToCharArray());
                foreach (string token in tokens)
                {
                    root = CheckHostMenuItem(token, root, levelZeroItems);
                }
            }

            MenuItem mi = CreateMenuItem(item.Name);
            mi.Tag = item;
            mi.Click += (o, a) => {
                MenuItem menuItem = (MenuItem)o;
                LauncherItem li = menuItem.Tag as LauncherItem;
                if (li != null)
                {
                    li.Start();
                }
            };

            if (root != null)
            {
                root.MenuItems.Add(mi);
            }
            else
            {
                levelZeroItems.Add(mi);
            }
        }

        private MenuItem CheckHostMenuItem(string token, MenuItem daddy, List<MenuItem> levelZeroItems)
        {
            if (daddy != null)
            {
                MenuItem[] items = daddy.MenuItems.Find(token, false);
                if(items.Length == 1)
                {   // Container already exists
                    return items[0];
                }
                else
                {   // Create a new container
                    MenuItem mi = CreateMenuItem(token);
                    daddy.MenuItems.Add(mi);
                    return mi;
                }
            }
            else
            {   // Level 0 container
                MenuItem item = levelZeroItems.FirstOrDefault(k => k.Name == token);
                if(item == null)
                {   // create a new one
                    MenuItem mi = CreateMenuItem(token);
                    levelZeroItems.Add(mi);
                    return mi;
                }

                return item;
            }
        }

        private MenuItem CreateMenuItem(string name)
        {
            MenuItem mi = new MenuItem(name)
            {
                Name = name,
            };
            _allMenuItems.Add(mi);
            return mi;
        }
    }
}
