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
    class CommaBracketElement : FormulaElement
    {
        public const double CommaWidth = 5;
        private static readonly FormattedText Comma = new FormattedText(",", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, ExprTreeVisualizer.FormulaFont, 16, Foreground, 1);
        private readonly FormulaElement[] elements;

        public CommaBracketElement(FormulaElement[] elements)
        {
            this.elements = elements;
            Width = BracketElement.OpenBracketWidth + BracketElement.CloseBracketWidth + BracketElement.BracketGap * 2 + CommaWidth * (elements.Length - 1);
            Height = 0;
            foreach (FormulaElement fe in elements)
            {
                Width += fe.Width;
                Height = Math.Max(Height, fe.Height);
            }
        }

        public override void Draw(DrawingContext ctx)
        {
            BracketElement.DrawBracket(ctx, 0, 0, Height, true);
            BracketElement.DrawBracket(ctx, Width - BracketElement.CloseBracketWidth, 0, Height, false);
            ctx.DrawRectangle(Brushes.Transparent, LinePen, new Rect(0, 0, Width, Height));

            double x = 0;
            for(int i = 0; i < elements.Length; i++)
            {
                if(i != 0)
                {
                    ctx.DrawText(Comma, new Point(BracketElement.OpenBracketWidth + BracketElement.BracketGap + x, Height - Comma.Height));
                    x += 5;
                }
                elements[i].Draw(ctx, BracketElement.OpenBracketWidth + BracketElement.BracketGap + x, (Height - elements[i].Height) / 2);
                x += elements[i].Width;
            }
        }
    }
}
