using System;
using System.Collections.Generic;
using System.IO;

namespace HumbleCpuMonitor.Config
{
    public class LauncherConfig
    {
        private List<LauncherItem> _items;

        public LauncherItem[] Items
        {
            get => _items.ToArray();
            set
            {
                if (value == null)
                {
                    return;
                }

                _items.Clear();
                _items.AddRange(value);
            }
        }

        public LauncherConfig()
        {
            _items = new List<LauncherItem>();
        }


        internal void LoadData(string filename)
        {
            try
            {
                if(!File.Exists(filename))
                {
                    return;
                }

                LauncherConfig launcherConfig = SerializationExt.DeserializeFromFile<LauncherConfig>(filename);
                if (launcherConfig == null) return;
                InitFromInstance(launcherConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal void SaveData(string filename)
        {
            this.SerializeToFile(filename);
        }

        private void InitFromInstance(LauncherConfig lc)
        {
            if (lc == null) return;

            _items.Clear();
            if (lc.Items != null)
            {
                _items.AddRange(lc.Items);
            }
        }
    }
}
