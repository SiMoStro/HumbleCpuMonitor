using System.Windows.Forms;

namespace HumbleCpuMonitor.Charts
{
    internal class MiniBarChart : MiniChartBase
    {
        internal MiniBarChart() : base()
        {
            HorizontalLines = 3;
            Type = ChartType.Bar;
        }

        protected override void ChartPaint(PaintEventArgs e)
        {
            base.ChartPaint(e);

            if (_points.Count == 0) return;
            for (int x = 0; x < MaxItems; x++)
            {
                if (x >= _points.Count) break;
                float y = (Height * _points[x]) / 100.0f;
                e.Graphics.FillRectangle(GetBrush(_points[x]), x * ItemHorPaintSize, Height - y, ItemHorPaintSize - 1, y);
            }
        }
    }
}
