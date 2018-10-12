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
        public const double BracketGap = 0;
        private static readonly FormattedText OpenBracket = new FormattedText("(", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, ExprTreeVisualizer.FormulaFont, 16, Foreground, 1);
        private static readonly FormattedText CloseBracket = new FormattedText(")", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, ExprTreeVisualizer.FormulaFont, 16, Foreground, 1);
        private readonly FormulaElement element;

        public static double OpenBracketWidth
        {
            get { return OpenBracket.Width; }
        }

        public static double CloseBracketWidth
        {
            get { return CloseBracket.Width; }
        }

        public BracketElement(FormulaElement element)
        {
            this.element = element;
            Width = element.Width + OpenBracket.Width + CloseBracket.Width + BracketGap * 2;
            Height = element.Height;
        }

        public override void Draw(DrawingContext ctx)
        {
            DrawBracket(ctx, 0, 0, Height, true);
            DrawBracket(ctx, Width - CloseBracket.Width, 0, Height, false);
            element.Draw(ctx, OpenBracket.Width + BracketGap, 0);
        }

        public static void DrawBracket(DrawingContext ctx, double x, double y, double height, bool open)
        {
            FormattedText text = open ? OpenBracket : CloseBracket;
            ctx.PushTransform(new ScaleTransform(1, height / text.Height, x, y));
            ctx.DrawText(text, new Point(x, y));
            ctx.Pop();
        }
    }
}
