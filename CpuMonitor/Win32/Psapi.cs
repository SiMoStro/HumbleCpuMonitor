using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HumbleCpuMonitor.Win32
{
    public static class Psapi
    {
        [DllImport("Psapi.dll", SetLastError = true)]
        public static extern bool EnumProcesses([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In][Out] uint[] processIds,
            UInt32 arraySizeBytes, [MarshalAs(UnmanagedType.U4)] out uint bytesCopied);

        [DllImport("psapi.dll")]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, uint nSize);
    }
}
