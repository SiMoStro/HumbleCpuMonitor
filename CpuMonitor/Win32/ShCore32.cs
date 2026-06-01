using System.Runtime.InteropServices;

namespace HumbleCpuMonitor.Win32
{
    public class ShCore32
    {
        [DllImport("shcore.dll")]
        public static extern int SetProcessDpiAwareness(int awareness);

    }
}
