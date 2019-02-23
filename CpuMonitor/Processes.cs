using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumbleCpuMonitor.Win32;

namespace HumbleCpuMonitor
{
    public class ProcessDescriptor
    {
        public uint Pid { get; set; }

        public string Name { get; set; }
    }

    public class Processes
    {
        List<ProcessDescriptor> _snapshot;

        public IReadOnlyList<ProcessDescriptor> Descriptors
        {
            get
            {
                return _snapshot;
            }
        }

        public int ProcessCount { get; private set; }

        public Processes()
        {
            _snapshot = new List<ProcessDescriptor>();
        }

        public void Update()
        {
            _snapshot.Clear();
            uint[] buffer = new uint[4096];
            uint bytesOut;
            Psapi.EnumProcesses(buffer, 4096, out bytesOut);
            ProcessCount = (int)(bytesOut / 4);

            for (int p = 0; p < ProcessCount; p++)
            {
                IntPtr hndl = Kernel32.OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, (int)buffer[p]);
                if (hndl != IntPtr.Zero)
                {
                    ProcessDescriptor pd = new ProcessDescriptor();
                    pd.Pid = buffer[p];
                    StringBuilder sb = new StringBuilder(1024);
                    Psapi.GetModuleFileNameEx(hndl, IntPtr.Zero, sb, 1024);
                    string temp = sb.ToString();
                    int lio = temp.LastIndexOf('\\') + 1;
                    if (lio > 0) temp = temp.Substring(lio);
                    pd.Name = temp;
                    _snapshot.Add(pd);
                    Kernel32.CloseHandle(hndl);
                }
            }
        }
    }
}
