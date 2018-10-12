using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathCalc.TypeRenderer
{
    class SequenceElement : FormulaElement
    {
        public const double Gap = 3;
        private readonly List<FormulaElement> elements;

        public SequenceElement(List<FormulaElement> elements) : base()
        {
            this.elements = elements;

            Width = -Gap;
            Height = 0;
            foreach (FormulaElement e in elements)
            {
                Width += e.Width + Gap;
                Height = Math.Max(Height, e.Height);
            }
        }

        public override void Draw(DrawingContext ctx)
        {
            double x = 0;
            foreach (FormulaElement e in elements)
            {
                e.Draw(ctx, x, (Height - e.Height) / 2);
                x += e.Width + Gap;
            }
        }
    }
}
