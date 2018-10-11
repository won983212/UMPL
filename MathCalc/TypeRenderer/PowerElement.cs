using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathCalc.TypeRenderer
{
    class PowerElement : FormulaElement
    {
        private readonly FormulaElement value;
        private readonly FormulaElement degree;

        public PowerElement(FormulaElement value, FormulaElement degree)
        {
            this.value = value;
            this.degree = degree;
            Width = value.Width + degree.Width / 2;
            Height = value.Height + degree.Height / 4;
        }

        public override void Draw(DrawingContext ctx)
        {
            value.Draw(ctx, 0, degree.Height / 8);
            ctx.PushTransform(new ScaleTransform(0.5, 0.5, value.Width, 0));
            degree.Draw(ctx, value.Width, 0);
            ctx.Pop();
        }
    }
}
