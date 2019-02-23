using System;
using System.Runtime.InteropServices;

namespace HumbleCpuMonitor.Win32
{
    public enum DesiredAccess : uint
    {
        STANDARD_RIGHTS_REQUIRED = 0x000F0000,
        STANDARD_RIGHTS_READ = 0x00020000,
        TOKEN_ASSIGN_PRIMARY = 0x0001,
        TOKEN_DUPLICATE = 0x0002,
        TOKEN_IMPERSONATE = 0x0004,
        TOKEN_QUERY = 0x0008,
        TOKEN_QUERY_SOURCE = 0x0010,
        TOKEN_ADJUST_PRIVILEGES = 0x0020,
        TOKEN_ADJUST_GROUPS = 0x0040,
        TOKEN_ADJUST_DEFAULT = 0x0080,
        TOKEN_ADJUST_SESSIONID = 0x0100,
        TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY),
        TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
                            TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
                            TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
                            TOKEN_ADJUST_SESSIONID)
    }

    public enum SePrivilege : uint
    {
        PrivilegeEnabledByDefault = 0x00000001,
        PrivilegeEnabled = 0x00000002,
        PrivilegeRemoved = 0x00000004,
        PrivilegeUsedForAccess = 0x8000000,
    }

    public struct Luid
    {
        public uint LowPart;
        public int HighPart;

        public uint GetSize()
        {
            return sizeof(uint) + sizeof(int);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LuidAndAttributes
    {
        public Luid Luid;
        public SePrivilege Attributes;

        public uint GetSize()
        {
            return Luid.GetSize() + sizeof(SePrivilege);
        }
    }

    public struct TokenPrivileges
    {
        public int PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LuidAndAttributes[] Privileges;

        public uint GetSize()
        {
            uint laaSize = 0;
            foreach (LuidAndAttributes laa in Privileges) laaSize += laa.GetSize();
            return sizeof(uint) + laaSize;
        }
    }

    public static class Advapi32
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, DesiredAccess DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll")]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref Luid lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
            [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
            ref TokenPrivileges NewState,
            uint BufferLengthInBytes,
            ref TokenPrivileges PreviousState,
            out uint ReturnLengthInBytes);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
            [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
            ref TokenPrivileges NewState,
            uint BufferLengthInBytes,
            IntPtr PreviousState,
            IntPtr ReturnLengthInBytes);
    }
}
