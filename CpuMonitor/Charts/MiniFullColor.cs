using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Charts
{
    internal class MiniFullColor : MiniChartBase
    {
        internal MiniFullColor() : base()
        {
            HorizontalLines = 3;
            ItemHorPaintSize = 1;
            Type = ChartType.FullColor;
        }

        protected override void ChartPaint(PaintEventArgs e)
        {
            base.ChartPaint(e);

            for (int x = 0; x < MaxItems; x++)
            {
                if (x >= _points.Count) break;
                RectangleF rect = new RectangleF(new PointF(x * ItemHorPaintSize, 0), new SizeF(ItemHorPaintSize, Height));
                e.Graphics.FillRectangle(GetBrush(_points[x]), rect);
            }
        }
    }
}
