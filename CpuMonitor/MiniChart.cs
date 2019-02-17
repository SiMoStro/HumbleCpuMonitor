using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor
{
    internal class MiniChart : Control
    {
        #region [private]

        private List<float> _values = new List<float>();
        private const int BarSize = 4;
        private int _rectCount;
        private Pen _horLinePen;

        #endregion

        internal Action<MiniChart> DoubleClickAction { get; set; }

        internal MiniChart()
        {
            DoubleBuffered = true;
            _horLinePen = new Pen(new SolidBrush(Color.FromArgb(128, 64, 64, 64)), 0.25f);
        }

        internal void AddValue(float value)
        {
            _values.Add(value);
            while (_values.Count > _rectCount)
            {
                _values.RemoveAt(0);
            }
            Invalidate();
        }

        internal void Restart()
        {
            _values.Clear();
            Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            DoubleClickAction?.Invoke(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            FullPaint(e);
        }

        private void FullPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);

            // horizontal lines
            float h1 = (float)Height / 4;
            for(int l = 1; l < 4; l++) e.Graphics.DrawLine(_horLinePen, 0, h1 * l, Width, h1 * l);

            // chart bars
            if (_values.Count == 0) return;
            for (int x = 0; x < _rectCount; x++)
            {
                if (x >= _values.Count) break;
                float y = (Height * _values[x]) / 100.0f;
                e.Graphics.FillRectangle(GetBrush(_values[x]), x * BarSize, Height - y, BarSize - 1, y);
            }

            // print last percentage
            if (_values.Count != 0)
            {
                e.Graphics.DrawString((_values[_values.Count - 1] / 100).ToString("P"), SystemFonts.DialogFont, Brushes.White, 10, 10);
            }
        }

        private Brush GetBrush(float value)
        {
            if (value > 90) return Brushes.Red;
            if (value > 70) return Brushes.Orange;
            if (value > 50) return Brushes.Yellow;
            if (value > 30) return Brushes.YellowGreen;
            return Brushes.LawnGreen;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // recalculate the number of bars we can show and reduce values buffer (if needed)
            _rectCount = (int)Math.Floor((float)Width / BarSize);
            while (_values.Count > _rectCount) _values.RemoveAt(0);

            Invalidate();
        }
    }
}
