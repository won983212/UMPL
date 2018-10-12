using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathCalc.TypeRenderer
{
    abstract class FormulaElement
    {
        public static readonly Pen LinePen = new Pen(Brushes.Black, 1);
        public static readonly Brush Foreground = Brushes.Black;
        public double Width;
        public double Height;

        static FormulaElement()
        {
            LinePen.Freeze();
        }

        public abstract void Draw(DrawingContext ctx);

        public void Draw(DrawingContext ctx, double offsetX, double offsetY)
        {
            ctx.PushTransform(new TranslateTransform(offsetX, offsetY));
            Draw(ctx);
            ctx.Pop();
        }
    }
}
