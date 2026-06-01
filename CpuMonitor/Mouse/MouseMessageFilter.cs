using HumbleCpuMonitor.Interfaces;
using HumbleCpuMonitor.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static HumbleCpuMonitor.Win32.User32;

namespace HumbleCpuMonitor
{
    public class MouseMessageFilter : IMessageFilter
    {
        // private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        // private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x0203;

        private RECT? _rectMouseDown;
        private Point _mouseDown;
        private IWinInfo _iwi;

        private GlobalMouseHook _globalHook;

        public Action LeftButtonDoubleClick { get; set; }

        public MouseMessageFilter(IWinInfo winInfo)
        {
            _iwi = winInfo;
            _globalHook = new GlobalMouseHook
            {
                ExtHook = GlobalMouseMove
            };
        }

        private void GlobalMouseMove(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0) return;
            if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                _rectMouseDown = null;
                _globalHook.Stop();
            }

            if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
            {
                GetCursorPos(out POINT mPos);
                var dx = _mouseDown.X - mPos.x;
                var dy = _mouseDown.Y - mPos.y;

                // Debug.WriteLine($"Mouse MOVE @ {mPos.x},{mPos.y} - DELTA {dx}x{dy}");
                SetWindowPos(_iwi.Handle, IntPtr.Zero, _rectMouseDown.Value.Left - dx, _rectMouseDown.Value.Top - dy, 0, 0, SetWindowPosFlags.IgnoreResize | SetWindowPosFlags.DoNotChangeOwnerZOrder);
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            try
            {
                if (_iwi.Handle == IntPtr.Zero) return false;

                IntPtr parent = GetAncestor(m.HWnd, GetAncestorFlags.GetRoot);
                if (parent == IntPtr.Zero) return false;
                if (parent != _iwi.Handle) return false;

                if (m.Msg == WM_LBUTTONDOWN)
                {
                    RECT r;
                    GetWindowRect(_iwi.Handle, out r);
                    _rectMouseDown = r;
                    GetCursorPos(out POINT mPos);
                    _mouseDown = new Point(mPos.x, mPos.y);

                    // Debug.WriteLine($"Mouse DOWN @ {mPos.x},{mPos.y} - rect [{r.Left},{r.Top}][{r.Right-r.Left}x{r.Bottom-r.Top}]");
                    _globalHook.Start();
                }
                else if (m.Msg == WM_LBUTTONDBLCLK)
                {
                    LeftButtonDoubleClick?.Invoke();
                }
            }
            catch (Exception e)
            {

            }

            return false;
        }
    }
}
