using System;
using System.Runtime.InteropServices;
using static HumbleCpuMonitor.Win32.Psapi;
using FT = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace HumbleCpuMonitor.Process
{
    internal class ProcessSnapshot
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

        /// <summary>
        /// Last interval CPU%
        /// </summary>
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

        /// <summary>
        /// Total process CPU time (Kernel + user)
        /// </summary>
        public double CpuTime
        {
            get
            {
                return _lastKernelTime.TotalMilliseconds + _lastUserTime.TotalMilliseconds;
            }
        }

        public double CpuTimeLastSlot
        {
            get
            {
                return (_lastKernelTime.TotalMilliseconds + _lastUserTime.TotalMilliseconds) - (_prevKernelTime.TotalMilliseconds + _prevUserTime.TotalMilliseconds);
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
}
