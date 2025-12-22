
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

class DpiHelper
{

    public enum MONITOR_DPI_TYPE : int
    {
        MDT_EFFECTIVE_DPI = 0,
        MDT_ANGULAR_DPI = 1,
        MDT_RAW_DPI = 2,
        MDT_DEFAULT
    };

    [DllImport("User32.dll")]
    private static extern IntPtr GetDesktopWindow();

    [DllImport("User32.dll")]
    private static extern IntPtr GetDpiForWindow(IntPtr hwnd);


    [DllImport("Shcore.dll")]
    private static extern int GetDpiForMonitor(IntPtr hmonitor, MONITOR_DPI_TYPE dpiType, out uint dpiX, out uint dpiY);


    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);


    public static int GetCurrentDpi()
    {
        IntPtr hwnd = GetDesktopWindow();
        return (int)GetDpiForWindow(hwnd);
    }

    public static double GetScalingFactor()
    {
        int dpi = GetCurrentDpi();
        // 96 DPI = 100% scaling
        return dpi / 96.0;
    }

    public static Point GetRdpScaling(IntPtr windowHandle)
    {
        var hm = MonitorFromWindow(windowHandle, 2);
        uint dpiX, dpiY;
        var retVal = GetDpiForMonitor(hm, MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out dpiX, out dpiY);
        Debug.WriteLine($"{{{dpiX},{dpiY}}}");
        retVal = GetDpiForMonitor(hm, MONITOR_DPI_TYPE.MDT_ANGULAR_DPI, out dpiX, out dpiY);
        Debug.WriteLine($"{{{dpiX},{dpiY}}}");
        retVal = GetDpiForMonitor(hm, MONITOR_DPI_TYPE.MDT_DEFAULT, out dpiX, out dpiY);
        Debug.WriteLine($"{{{dpiX},{dpiY}}}");
        retVal = GetDpiForMonitor(hm, MONITOR_DPI_TYPE.MDT_RAW_DPI, out dpiX, out dpiY);
        Debug.WriteLine($"{{{dpiX},{dpiY}}}");
        return new Point { X = (int)dpiX, Y = (int)dpiY };
    }
}
