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
    class AbsoluteElement : FormulaElement
    {
        public const double SymbolWidth = 1;
        public const double Gap = 2;
        private readonly FormulaElement element;

        public AbsoluteElement(FormulaElement element)
        {
            this.element = element;
            Width = element.Width + (SymbolWidth + Gap) * 2;
            Height = element.Height;
        }

        public override void Draw(DrawingContext ctx)
        {
            ctx.DrawLine(LinePen, new Point(0, 0), new Point(0, Height));
            ctx.DrawLine(LinePen, new Point(Width, 0), new Point(Width, Height));
            element.Draw(ctx, SymbolWidth + Gap, 0);
        }
    }
}
