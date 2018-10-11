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
    class TextElement : FormulaElement
    {
        private readonly FormattedText text;

        public TextElement(string text, int size) : base()
        {
            this.text = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, ExprTreeVisualizer.FormulaFont, size, Foreground, 1);
            Width = this.text.Width;
            Height = this.text.Height;
        }

        public override void Draw(DrawingContext ctx)
        {
            ctx.DrawText(text, new Point(0, 0));
        }
    }
}
