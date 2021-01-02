using System;
using System.Runtime.InteropServices;
using FT = System.Runtime.InteropServices.ComTypes.FILETIME;
using System.Text;

namespace HumbleCpuMonitor.Win32
{
    public static class Psapi
    {
        [StructLayout(LayoutKind.Sequential, Size = 44)]
        public struct PROCESS_MEMORY_COUNTERS_EX
        {
            public uint cb;
            public uint PageFaultCount;
            public int PeakWorkingSetSize;
            public int WorkingSetSize;
            public int QuotaPeakPagedPoolUsage;
            public int QuotaPagedPoolUsage;
            public int QuotaPeakNonPagedPoolUsage;
            public int QuotaNonPagedPoolUsage;
            public int PagefileUsage;
            public int PeakPagefileUsage;
            public int PrivateUsage;
        }

        public static DateTime FiletimeToDateTime(FT fileTime)
        {   // NB! uint conversion must be done on both fields before ulong conversion
            ulong hFT2 = unchecked((((ulong)(uint)fileTime.dwHighDateTime) << 32) | (uint)fileTime.dwLowDateTime);
            return DateTime.FromFileTimeUtc((long)hFT2);
        }

        public static TimeSpan FiletimeToTimeSpan(FT fileTime)
        {   // NB! uint conversion must be done on both fields before ulong conversion
            ulong hFT2 = unchecked((((ulong)(uint)fileTime.dwHighDateTime) << 32) | (uint)fileTime.dwLowDateTime);
            return TimeSpan.FromTicks((long)hFT2);
        }

        [DllImport("Psapi.dll", SetLastError = true)]
        public static extern bool EnumProcesses([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In][Out] uint[] processIds,
            UInt32 arraySizeBytes, [MarshalAs(UnmanagedType.U4)] out uint bytesCopied);

        [DllImport("psapi.dll")]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, uint nSize);

        [DllImport("psapi.dll", SetLastError = true)]
        public static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS_EX counters, uint size);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetProcessTimes(IntPtr hProcess, out FT lpCreationTime, out FT lpExitTime, out FT lpKernelTime, out FT lpUserTime);
    }
}
