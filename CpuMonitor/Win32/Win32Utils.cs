using System;
using System.Drawing;

namespace HumbleCpuMonitor.Win32
{
    internal static class Win32Utils
    {
        /// <summary>
        /// Gets the X coordinate from an lParam
        /// </summary>
        /// <param name="lParam">lPAram</param>
        /// <returns>X-Coordinate</returns>
        public static int GetXLParam(IntPtr lParam)
        {
            // For signed 16-bit integers (which the x-coord is), we cast to int 
            // to handle potential negative coordinates correctly on multi-monitor systems.
            return (int)(short)LowWord(lParam);
        }

        /// <summary>
        /// Gets the Y coordinate from an lParam
        /// </summary>
        /// <param name="lParam">lParam</param>
        /// <returns>Y-Coordinate</returns>
        public static int GetYLParam(IntPtr lParam)
        {
            // The y-coord is in the high word.
            return (int)(short)HighWord(lParam);
        }

        /// <summary>
        /// Gets the coordinates from an lParam 
        /// </summary>
        /// <param name="lParam">lParam</param>
        /// <returns>Point</returns>
        public static Point GetWin32Point(IntPtr lParam)
        {
            Point pt = new Point();
            pt.X = (short)LowWord(lParam);
            pt.Y = (short)HighWord(lParam);
            return pt;
        }

        private static uint LowWord(IntPtr lParam)
        {
            // Get the lower 32 bits of the IntPtr (it's 64 bits on modern systems)
            long val = lParam.ToInt64();
            return (uint)(val & 0xFFFF);
        }

        private static uint HighWord(IntPtr lParam)
        {
            // Shift right by 16 bits to access the high word
            long val = lParam.ToInt64();
            return (uint)((val >> 16) & 0xFFFF);
        }
    }
}
