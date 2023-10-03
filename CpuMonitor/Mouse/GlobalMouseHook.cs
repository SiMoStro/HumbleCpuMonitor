using System;
using System.Diagnostics;
using static HumbleCpuMonitor.Win32.User32;


namespace HumbleCpuMonitor
{
    public class GlobalMouseHook
    {
        private LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private const int WH_MOUSE_LL = 14;

        public void Start() => _hookID = SetHook(_proc);

        public void Stop() => UnhookWindowsHookEx(_hookID);

        public Action<int, IntPtr, IntPtr> ExtHook { get; set; }

        public GlobalMouseHook()
        {
            _proc = HookCallback;
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (System.Diagnostics.Process curProcess = System.Diagnostics.Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && ExtHook != null)
            {
                ExtHook(nCode, wParam, lParam);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
