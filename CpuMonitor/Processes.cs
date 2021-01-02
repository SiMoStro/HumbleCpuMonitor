using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using HumbleCpuMonitor.Win32;
using static HumbleCpuMonitor.Win32.Psapi;
using FT = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace HumbleCpuMonitor
{
    public class ProcessSnapshot
    {
        public DateTime _prevTime { get; set; }
        public DateTime _lastTime { get; set; }

        private PROCESS_MEMORY_COUNTERS_EX _memoryPrev;
        private PROCESS_MEMORY_COUNTERS_EX _memoryLast;
        private DateTime _creationTime;
        private DateTime _exitTime;

        private TimeSpan _prevKernelTime;
        private TimeSpan _prevUserTime;
        private TimeSpan _lastKernelTime;
        private TimeSpan _lastUserTime;

        public double CpuPerc
        {
            get
            {
                if (_prevKernelTime == TimeSpan.Zero) return 0.0;

                double deltaTime = _lastTime.Subtract(_prevTime).TotalMilliseconds;
                double totCpuTime = (_lastKernelTime + _lastUserTime).Subtract(_prevKernelTime + _prevUserTime).TotalMilliseconds;
                return (totCpuTime / deltaTime) * 100.0;
            }
        }

        public void Snapshot(IntPtr handle)
        {
            _prevKernelTime = _lastKernelTime;
            _prevUserTime = _lastUserTime;
            _prevTime = _lastTime;
            _memoryPrev = _memoryLast;

            _lastTime = DateTime.UtcNow;
            _memoryLast = new PROCESS_MEMORY_COUNTERS_EX();
            _memoryLast.cb = (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS_EX));
            GetProcessMemoryInfo(handle, out _memoryLast, _memoryLast.cb);

            FT kernelTime, userTime, creationTime, exitTime;
            GetProcessTimes(handle, out creationTime, out exitTime, out kernelTime, out userTime);
            _lastKernelTime = FiletimeToTimeSpan(kernelTime);
            _lastUserTime = FiletimeToTimeSpan(userTime);
            _creationTime = FiletimeToDateTime(creationTime);
            _exitTime = FiletimeToDateTime(exitTime);
        }
    }

    public class ProcessDescriptor
    {
        public uint Pid { get; set; }

        public string Name { get; set; }

        public ProcessSnapshot Snapshot { get; set; }

        public ProcessDescriptor()
        {
            Snapshot = new ProcessSnapshot();
        }
    }

    public class Processes
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
    }
}
