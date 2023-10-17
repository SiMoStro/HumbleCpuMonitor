using System.Drawing;
using System.Windows.Forms;

namespace HumbleCpuMonitor
{
    public class CustomProgressBar : Control
    {
        private int _min, _max, _value;
        private Brush _background, _foreground;
        private ContentAlignment textAlign = ContentAlignment.TopLeft;

        public int Minimum
        {
            get
            {
                return _min;
            }
            set
            {
                if (value > _max) return;
                if (value == _min) return;
                _min = value;
                Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return _max;
            }
            set
            {
                if (value < _min) return;
                if (value == _max) return;
                _max = value;
                Invalidate();
            }
        }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value < Minimum || value > Maximum) return;
                if (value == _value) return;
                _value = value;
                Invalidate();
            }
        }

        public Brush Background
        {
            get
            {
                return _background;
            }

            set
            {
                _background = value;
                Invalidate();
            }
        }

        public Brush Foreground
        {
            get
            {
                return _foreground;
            }

            set
            {
                _foreground = value;
                Invalidate();
            }
        }

        public CustomProgressBar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            TabStop = false;

            Background = new SolidBrush(Color.FromKnownColor(KnownColor.Control));
            Foreground = new SolidBrush(Color.FromKnownColor(KnownColor.ActiveCaption));
        }

        /// <summary>
        /// Gets the creation parameters.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        /// <summary>
        /// Paints the background.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Background, new RectangleF(0, 0, Width, Height));
        }

        /// <summary>
        /// Paints the control.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    Debug.WriteLine(m.Msg);
        //    if (m.Msg == 0x000F) // WM_PAINT
        //    {
        //        Draw(null);
        //    }

        //    if (m.Msg == 0x0014) // WM_ERASEBKGND
        //    {
        //        Draw(null);
        //    }
        //}

        private void Draw(Graphics g)
        {
            bool isNew = false; ;
            Graphics graphics;
            if (g == null)
            {
                isNew = true;
                graphics = CreateGraphics();
            }
            else graphics = g;

            float delta1 = (Maximum - Minimum);
            float delta2 = (Value - Minimum);
            float rect = (Width * delta2) / delta1;
            graphics.FillRectangle(Background, new RectangleF(0, 0, Width, Height));
            graphics.FillRectangle(Foreground, new RectangleF(0, 0, rect, Height));
            using (SolidBrush brush = new SolidBrush(ForeColor))
            {
                SizeF size = graphics.MeasureString(Text, Font);

                // first figure out the top
                float top = 0;
                switch (textAlign)
                {
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.MiddleRight:
                        top = (Height - size.Height) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.BottomRight:
                        top = Height - size.Height;
                        break;
                }

                float left = -1;
                switch (textAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.BottomLeft:
                        if (RightToLeft == RightToLeft.Yes)
                            left = Width - size.Width;
                        else
                            left = -1;
                        break;
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.BottomCenter:
                        left = (Width - size.Width) / 2;
                        break;
                    case ContentAlignment.TopRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.BottomRight:
                        if (RightToLeft == RightToLeft.Yes)
                            left = -1;
                        else
                            left = Width - size.Width;
                        break;
                }
                graphics.DrawString(Text, Font, brush, left, top);
            }

            if(isNew) graphics.Dispose();
        }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <returns>
        /// The text associated with this control.
        /// </returns>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text == value) return;
                base.Text = value;
                Invalidate();
                //RecreateHandle();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Windows.Forms.RightToLeft"/> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit"/>.
        /// </returns>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
        /// The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft"/> values.
        /// </exception>
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
                Invalidate();
                //RecreateHandle();
            }
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Drawing.Font"/> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"/> property.
        /// </returns>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                Invalidate();
                //RecreateHandle();
            }
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public ContentAlignment TextAlign
        {
            get { return textAlign; }
            set
            {
                textAlign = value;
                Invalidate();
                //RecreateHandle();
            }
        }
    }
}
