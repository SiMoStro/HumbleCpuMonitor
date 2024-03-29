﻿using HumbleCpuMonitor.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor.Charts
{
    abstract internal class MiniChartBase : Control
    {
        #region [private]

        private int _itemsCount;
        private Pen _horLinePen;

        #endregion

        #region [protected]

        protected List<float> _points = new List<float>();
        protected int ItemHorPaintSize = 4;
        protected Brush ForegroundBrush;

        #endregion

        /// <summary>
        /// The current number of items
        /// </summary>
        public int Items => _points.Count;

        /// <summary>
        /// Chart values
        /// </summary>
        public IEnumerable<float> Values => _points;

        /// <summary>
        /// Maximum number of items the chart can draw
        /// </summary>
        public int MaxItems => _itemsCount;

        /// <summary>
        /// Number of horizontal lines
        /// </summary>
        public int HorizontalLines { get; set; }

        /// <summary>
        /// Chart type
        /// </summary>
        public ChartType Type { get; protected set; }

        internal MiniChartBase()
        {
            DoubleBuffered = true;
            UpdateColors();
            Type = ChartType.Unknown;
        }

        internal virtual void UpdateColors()
        {
            ConfigData config = ScenarioManager.Instance.Configuration;
            _horLinePen = new Pen(new SolidBrush(config.ChartLines), 0.25f);
            ForegroundBrush = new SolidBrush(config.Foreground);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            FullPaint(e);
        }

        private void FullPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(ScenarioManager.Instance.Configuration.Background);

            // horizontal lines
            float h1 = (float)Height / (HorizontalLines + 1);
            for (int l = 1; l < HorizontalLines + 1; l++) e.Graphics.DrawLine(_horLinePen, 0, h1 * l, Width, h1 * l);

            try
            {
                ChartPaint(e);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            // print last percentage
            if (_points.Count != 0)
            {
                e.Graphics.DrawString((_points[_points.Count - 1] / 100).ToString("P"), SystemFonts.DialogFont, ForegroundBrush, 10, 10);
            }
        }

        protected virtual void ChartPaint(PaintEventArgs e)
        {

        }

        public void AddValue(float value)
        {
            _points.Add(value);
            while (_points.Count > _itemsCount)
            {
                _points.RemoveAt(0);
            }
            Invalidate();
        }

        public void Restart()
        {
            _points.Clear();
            Invalidate();
        }

        public void InitValues(IEnumerable<float> values)
        {
            _points.Clear();
            _points.AddRange(values);
        }

        protected void UpdatePaintItemsCount()
        {
            _itemsCount = (int)Math.Floor((float)Width / ItemHorPaintSize);
            while (_points.Count > _itemsCount) _points.RemoveAt(0);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdatePaintItemsCount();
            Invalidate();
        }
    }
}
