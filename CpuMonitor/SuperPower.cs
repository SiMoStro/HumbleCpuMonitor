using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumbleCpuMonitor.Win32;

namespace HumbleCpuMonitor
{
    internal static class SuperPower
    {
        public static bool Enable()
        {
            IntPtr handle;
            bool res = Advapi32.OpenProcessToken(Kernel32.GetCurrentProcess(), DesiredAccess.TOKEN_ADJUST_PRIVILEGES | DesiredAccess.TOKEN_QUERY, out handle);
            if (!res)
            {
                Kernel32.CloseHandle(handle);
                return false;
            };

            // First pass: get current privilege settings
            Luid luid = new Luid();
            res = Advapi32.LookupPrivilegeValue(null, "SeDebugPrivilege", ref luid);
            TokenPrivileges tp1 = new TokenPrivileges
            {
                PrivilegeCount = 1
            };
            tp1.Privileges = new LuidAndAttributes[1];
            tp1.Privileges[0].Luid = luid;
            tp1.Privileges[0].Attributes = 0;

            TokenPrivileges tPrev = new TokenPrivileges();
            uint retSize = 0;
            res = Advapi32.AdjustTokenPrivileges(handle, false, ref tp1, tp1.GetSize(), ref tPrev, out retSize);
            if (!res)
            {
                Kernel32.CloseHandle(handle);
                return false;
            };

            // 
            // Second pass: set privilege based on previous setting
            // 
            tPrev.PrivilegeCount = 1;
            tPrev.Privileges[0].Luid = luid;
            tPrev.Privileges[0].Attributes |= SePrivilege.PrivilegeEnabled;

            res = Advapi32.AdjustTokenPrivileges(handle, false, ref tPrev, retSize, IntPtr.Zero, IntPtr.Zero);

            return res;

        }
    }
}
