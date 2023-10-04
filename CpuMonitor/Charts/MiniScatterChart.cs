using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Charts
{
    internal class MiniScatterChart : MiniChartBase
    {
        private float _halfDotSize = 1.5f;

        internal MiniScatterChart() : base()
        {
            HorizontalLines = 3;
            Type = ChartType.Scatter;
        }

        protected override void ChartPaint(PaintEventArgs e)
        {
            base.ChartPaint(e);

            for (int x = 0; x < MaxItems; x++)
            {
                if (x >= _points.Count) break;
                float y = (Height * _points[x]) / 100.0f;
                PointF pt = new PointF(x * ItemHorPaintSize, Height - y);
                var r = new RectangleF(new PointF(pt.X - _halfDotSize, pt.Y - _halfDotSize), new SizeF(_halfDotSize * 2, _halfDotSize * 2));
                e.Graphics.DrawEllipse(GetPen(_points[x]), r);
                e.Graphics.FillEllipse(GetBrush(_points[x]), r);
            }
        }
    }
}
