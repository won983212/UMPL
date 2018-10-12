using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MathCalc.TypeRenderer
{
    class SquareRootElement : FormulaElement
    {
        private const double SqrtWidth = 10;
        private const double Margin = 1;
        private readonly FormulaElement element;

        public SquareRootElement(FormulaElement element)
        {
            this.element = element;
            Width = element.Width + SqrtWidth + Margin;
            Height = element.Height + Margin * 2;
        }

        public override void Draw(DrawingContext ctx)
        {
            ctx.DrawLine(LinePen, new Point(0, Height - SqrtWidth * 2 / 3), new Point(SqrtWidth * 2 / 3, Height));
            ctx.DrawLine(LinePen, new Point(SqrtWidth * 2 / 3, Height), new Point(SqrtWidth, 0));
            ctx.DrawLine(LinePen, new Point(SqrtWidth, 0), new Point(Width, 0));
            element.Draw(ctx, SqrtWidth + Margin, Margin);
        }
    }
}
