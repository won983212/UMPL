using ExprCore.Renderer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MathCalc
{
    class WpfTreeRenderContext : IRenderContext
    {
        private static readonly Brush Background = Brushes.White;
        private static readonly Brush Foreground = Brushes.Black;
        private static readonly Pen LinePen = new Pen(Foreground, 1);
        private static readonly string FontFamily = "맑은 고딕";
        private DrawingContext ctx;

        public WpfTreeRenderContext(DrawingContext ctx)
        {
            this.ctx = ctx;
        }

        static WpfTreeRenderContext()
        {
            LinePen.Freeze();
        }

        public void DrawBackground(ExprCore.Renderer.Size size)
        {
            ctx.DrawRectangle(Background, null, new System.Windows.Rect(0, 0, size.Width, size.Height));
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            ctx.DrawLine(LinePen, new System.Windows.Point(x1, y1), new System.Windows.Point(x2, y2));
        }

        public void DrawRectangle(int x, int y, int w, int h)
        {
            ctx.DrawRectangle(null, LinePen, new System.Windows.Rect(x, y, w, h));
        }

        public void DrawString(string text, double x, double y)
        {
            ctx.DrawText(CreateText(text), new Point(x, y));
        }

        public ExprCore.Renderer.Size MeasureString(string text)
        {
            FormattedText txt = CreateText(text);
            return new ExprCore.Renderer.Size(txt.Width, txt.Height);
        }

        private FormattedText CreateText(string text)
        {
            Typeface typeface = new Typeface(FontFamily);
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 12, Foreground);
        }
    }
}
