using System;
using System.Drawing;

namespace HumbleCpuMonitor
{
    internal class Utilities
    {
        public const int KiloBytes = 1024;
        public const int MegaBytes = 1048576;
        public const int GigaBytes = 1073741824;

        public static string FormatBytes(long bytes)
        {
            if (bytes < KiloBytes) return bytes + "B";
            if (bytes < MegaBytes) return bytes / KiloBytes + "KB";
            if (bytes < GigaBytes) return bytes / MegaBytes + "MB";
            return bytes.ToString();
        }
    }

    internal static class ColorExt
    {
        /// <summary>
        /// Converts a color into a string
        /// </summary>
        /// <param name="c">Color</param>
        /// <returns>HTML color</returns>
        public static string ToHtmlColor(this Color c)
        {
            return $"#{c.A:000}{c.R:000}{c.G:000}{c.B:000}";
        }

        /// <summary>
        /// Converts an HTML color into a <see cref="System.Drawing.Color"/>
        /// </summary>
        /// <param name="s">HTML color string</param>
        /// <returns>Color</returns>
        public static Color FromHtmlColor(this string s)
        {
            try
            {
                if (s.Length != 13) return Color.Red;
                int.TryParse(s.Substring(1, 3), out var a);
                int.TryParse(s.Substring(4, 3), out var r);
                int.TryParse(s.Substring(7, 3), out var g);
                int.TryParse(s.Substring(10, 3), out var b);
                return Color.FromArgb(a, r, g, b);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Color.Red;
            }            
        }
    }
}
