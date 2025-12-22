using HumbleCpuMonitor.Config;
using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Charts
{
    internal class MiniPixelChart : MiniChartBase
    {
        internal MiniPixelChart() : base()
        {
            ItemHorPaintSize = 1;
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
                var r = new RectangleF(new PointF(pt.X, pt.Y), new SizeF(1, 1));
                e.Graphics.FillRectangle(ScenarioManager.Instance.Configuration.GetBrushForValue(_points[x]), r);
            }
        }
    }
}
