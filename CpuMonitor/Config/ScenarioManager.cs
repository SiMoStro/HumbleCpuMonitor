using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace HumbleCpuMonitor.Config
{
    internal class ScenarioManager
    {
        private const string ScenarioFilename = "HcmScenario.xml";

        internal ConfigData Configuration { get; private set; }

        #region Singleton

        private static volatile ScenarioManager _instance;
        private static readonly object Locker = new object();

        /// <summary>
        /// True if the ScenarioManager instance is valid, false otherwise
        /// </summary>
        public static bool IsStarted
        {
            get => (_instance != null);
        }

        /// <summary>
        /// The ScenarioManager instance
        /// </summary>
        public static ScenarioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new ScenarioManager();
                        }
                    }
                }

                return _instance;
            }
        }

        private ScenarioManager()
        {
        }

        #endregion

        public void Initialize()
        {
            string filename = GetFullPath();
            if (!File.Exists(filename))
            {
                Configuration = new ConfigData();
                return;
            }
            Configuration = new ConfigData();
            Configuration.LoadData(filename);
        }

        internal void Save()
        {
            Configuration.SaveData(GetFullPath());
        }

        internal void SetNewColors(List<Color> colors)
        {
            if (colors.Count != 10) return;
            Configuration.SetValueColors(colors);
        }

        private string GetFullPath()
        {
            string appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fullPath = Path.Combine(appDir, "HumbleCpuMonitor");
            string filename = Path.Combine(fullPath, ScenarioFilename);
            return filename;
        }
    }
}
