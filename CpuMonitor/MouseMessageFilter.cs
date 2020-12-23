using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static HumbleCpuMonitor.Win32.User32;

namespace HumbleCpuMonitor
{
    public class MouseMessageFilter : IMessageFilter
    {
        private const int WM_LBUTTONDOWN = 0x201;
        // private const int WM_LBUTTONUP = 0x202;
        // private const int WM_MOUSEMOVE = 0x200;

        private IntPtr _winHandle;
        private RECT? _rectMouseDown;
        private Point _mouseDown;

        private GlobalMouseHook _globalHook;

        public MouseMessageFilter(IntPtr handle)
        {
            _winHandle = handle;
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
            if(MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                var dx = _mouseDown.X - hookStruct.pt.x;
                var dy = _mouseDown.Y - hookStruct.pt.y;
                SetWindowPos(_winHandle, IntPtr.Zero, _rectMouseDown.Value.Left - dx, _rectMouseDown.Value.Top - dy, 0, 0, SetWindowPosFlags.IgnoreResize | SetWindowPosFlags.DoNotChangeOwnerZOrder);
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (_winHandle == IntPtr.Zero) return false;

            IntPtr parent = GetAncestor(m.HWnd, GetAncestorFlags.GetRoot);
            if (parent == IntPtr.Zero) return false;

            if (m.Msg == WM_LBUTTONDOWN)
            {
                if (parent != _winHandle) return false;

                RECT r;
                GetWindowRect(_winHandle, out r);
                _rectMouseDown = r;
                _mouseDown = Cursor.Position;
                _globalHook.Start();
            }

            return false;
        }
    }    
}
