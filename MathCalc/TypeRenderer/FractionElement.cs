using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MathCalc.TypeRenderer
{
    class FractionElement : FormulaElement
    {
        private readonly FormulaElement numerator;
        private readonly FormulaElement denominator;

        public FractionElement(FormulaElement n, FormulaElement d)
        {
            numerator = n;
            denominator = d;
            Width = Math.Max(numerator.Width, denominator.Width);
            Height = numerator.Height + denominator.Height + 3;
        }

        public override void Draw(DrawingContext ctx)
        {
            double y = numerator.Height + 1;

            numerator.Draw(ctx, (Width - numerator.Width) / 2, 0);
            ctx.DrawLine(LinePen, new Point(0, y), new Point(Width, y));
            denominator.Draw(ctx, (Width - denominator.Width) / 2, y + 3);
        }
    }
}
