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
}
