using System;

namespace HumbleCpuMonitor.Interfaces
{
    /// <summary>
    /// Interface to generic window properties
    /// </summary>
    public interface IWinInfo
    {
        IntPtr Handle { get; }

        float DpiX { get; }

        float DpiY { get; }
    }
}
