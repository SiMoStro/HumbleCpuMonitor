using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using HumbleCpuMonitor.Win32;
using static HumbleCpuMonitor.Win32.Psapi;

namespace HumbleCpuMonitor.Process
{
    internal class Processes
    {
        private Dictionary<uint, ProcessDescriptor> _processes = new Dictionary<uint, ProcessDescriptor>();

        public IReadOnlyList<ProcessDescriptor> Descriptors
        {
            get
            {
                return _processes.Values.ToArray();
            }
        }

        public int ProcessCount { get; private set; }

        public void Update()
        {
            var oldSnapshot = _processes;
            _processes = new Dictionary<uint, ProcessDescriptor>();

            uint[] buffer = new uint[4096];
            uint bytesOut;
            EnumProcesses(buffer, 4096, out bytesOut);
            ProcessCount = (int)(bytesOut / 4);

            for (int p = 0; p < ProcessCount; p++)
            {
                IntPtr hndl = Kernel32.OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, (int)buffer[p]);

                if (hndl != IntPtr.Zero)
                {
                    uint pid = buffer[p];
                    ProcessDescriptor pd;
                    if (oldSnapshot.ContainsKey(pid)) pd = oldSnapshot[pid];
                    else
                    {
                        pd = new ProcessDescriptor();
                        pd.Pid = pid;
                        StringBuilder sb = new StringBuilder(1024);
                        GetModuleFileNameEx(hndl, IntPtr.Zero, sb, 1024);
                        string temp = sb.ToString();
                        int lio = temp.LastIndexOf('\\') + 1;
                        if (lio > 0) temp = temp.Substring(lio);
                        pd.Name = temp;
                    }

                    _processes[pid] = pd;

                    pd.Snapshot.Snapshot(hndl);
                    Kernel32.CloseHandle(hndl);
                }
            }

            oldSnapshot.Clear();
        }

        public ProcessDescriptor[] GetTopProcessesByCpu(int count)
        {
            return _processes.Values.OrderByDescending(p => p.Snapshot.CpuTimeLastSlot).Take(count).ToArray();
        }
    }
}
