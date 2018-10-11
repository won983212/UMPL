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
    class BracketElement : FormulaElement
    {
        public const double BracketWidth = 5;
        public const double BracketGap = 2;
        public const double BracketSize = 16;
        private static readonly FormattedText OpenBracket;
        private static readonly FormattedText CloseBracket;
        private readonly FormulaElement element;

        static BracketElement()
        {
            OpenBracket = new FormattedText("(", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, ExprTreeVisualizer.FormulaFont, BracketSize, Foreground, 1);
            CloseBracket = new FormattedText(")", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, ExprTreeVisualizer.FormulaFont, BracketSize, Foreground, 1);
        }

        public BracketElement(FormulaElement element)
        {
            this.element = element;
            Width = element.Width + (BracketWidth + BracketGap) * 2;
            Height = element.Height;
        }

        public override void Draw(DrawingContext ctx)
        {
            DrawBracket(ctx, 0, 0, true);
            DrawBracket(ctx, element.Width + BracketWidth + BracketGap * 2, 0, false);
            element.Draw(ctx, BracketWidth + BracketGap, 0);
        }

        private void DrawBracket(DrawingContext ctx, double x, double y, bool open)
        {
            FormattedText text = open ? OpenBracket : CloseBracket;
            ctx.PushTransform(new ScaleTransform(BracketWidth / text.Width, Height / text.Height, x, y));
            ctx.DrawText(text, new Point(x, y));
            ctx.Pop();
        }
    }
}
