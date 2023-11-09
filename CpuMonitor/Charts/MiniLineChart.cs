using HumbleCpuMonitor.Config;
using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Charts
{
    internal class MiniLineChart : MiniChartBase
    {
        internal MiniLineChart() : base()
        {
            HorizontalLines = 3;
            Type = ChartType.Line;
        }

        protected override void ChartPaint(PaintEventArgs e)
        {
            base.ChartPaint(e);

            PointF lastPoint = new PointF(0, 0);
            for (int x = 0; x < MaxItems; x++)
            {
                if (x >= _points.Count) break;
                float y = (Height * _points[x]) / 100.0f;
                PointF pt = new PointF(x * ItemHorPaintSize, Height - y);
                e.Graphics.DrawLine(ScenarioManager.Instance.Configuration.GetPenForValue(_points[x]), lastPoint, pt);
                lastPoint = pt;
            }
        }
    }
}
