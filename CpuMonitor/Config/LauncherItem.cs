using System;
using System.IO;

namespace HumbleCpuMonitor.Config
{
    public class LauncherItem
    {
        /// <summary>
        /// ID of this item (must be unique)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the applicaiton
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the application
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional ordering token to structure the shortcuts.
        /// <para>For example App;Dev ==> will put the item below an "App" and then a "Dev" submenu</para>
        /// </summary>
        public string Filing { get; set; }

        /// <summary>
        /// An optional ordering index
        /// </summary>
        public int LocalIdx { get; set; }

        /// <summary>
        /// Executable (full path)
        /// </summary>
        public string Executable { get; set; }

        /// <summary>
        /// Working Directory
        /// </summary>
        public string WorkDir { get; set; }

        /// <summary>
        /// Command line parameters
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// Run the process in admin mode
        /// </summary>
        public bool RunAsAdmin { get; set; }

        internal LauncherItem Clone()
        {
            LauncherItem li = new LauncherItem
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Filing = Filing,
                LocalIdx = LocalIdx,
                Executable = Executable,
                WorkDir = WorkDir,
                Params = Params,
                RunAsAdmin = RunAsAdmin
            };

            return li;
        }

        internal void Start()
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                {
                    Arguments = Params,
                    FileName = Executable,
                    UseShellExecute = true,
                    CreateNoWindow = true
                };

                if (WorkDir != null && Directory.Exists(WorkDir))
                {
                    psi.WorkingDirectory = WorkDir;
                }

                if (RunAsAdmin)
                {
                    psi.Verb = "runas";
                }

                proc.StartInfo = psi;
                proc.Start();
            }
            catch (Exception e)
            {

            }

        }
    }
}
