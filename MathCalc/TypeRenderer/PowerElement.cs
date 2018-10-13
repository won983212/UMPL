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
        private const double DegreeRatio = 11 / 18.0;
        private readonly FormulaElement value;
        private readonly FormulaElement degree;

        public PowerElement(FormulaElement value, FormulaElement degree)
        {
            this.value = value;
            this.degree = degree;
            Width = value.Width + degree.Width * DegreeRatio;
            Height = Math.Max(value.Height, degree.Height * DegreeRatio);
        }

        public override void Draw(DrawingContext ctx)
        {
            value.Draw(ctx, 0, 0);
            ctx.PushTransform(new ScaleTransform(DegreeRatio, DegreeRatio, value.Width, 0));
            degree.Draw(ctx, value.Width, 0);
            ctx.Pop();
        }
    }
}
