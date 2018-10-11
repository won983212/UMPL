using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MathCalc.TypeRenderer
{
    class MatrixElement : FormulaElement
    {
        public const double ElementGap = 5;
        private readonly double[] rowMax;
        private readonly double[] columnMax;
        private readonly FormulaElement[] elements;

        public MatrixElement(double[] rowMax, double[] columnMax, FormulaElement[] elements)
        {
            this.rowMax = rowMax;
            this.columnMax = columnMax;
            this.elements = elements;

            Width = (columnMax.Length + 1) * ElementGap;
            Height = (rowMax.Length - 1) * ElementGap;

            foreach (double value in columnMax)
                Width += value;
            foreach (double value in rowMax)
                Height += value;
        }

        public override void Draw(DrawingContext ctx)
        {
            double x = 0;
            double y = 0;
            int i = 0;

            ctx.DrawLine(LineColor, new Point(0, 0), new Point(0, Height));
            ctx.DrawLine(LineColor, new Point(0, 0), new Point(3, 0));
            ctx.DrawLine(LineColor, new Point(0, Height), new Point(3, Height));

            ctx.DrawLine(LineColor, new Point(Width, 0), new Point(Width, Height));
            ctx.DrawLine(LineColor, new Point(Width - 3, 0), new Point(Width, 0));
            ctx.DrawLine(LineColor, new Point(Width - 3, Height), new Point(Width, Height));

            for (int r = 0; r < rowMax.Length; r++)
            {
                x = ElementGap;
                for(int c = 0; c < columnMax.Length; c++)
                {
                    FormulaElement element = elements[i++];
                    element.Draw(ctx, x + (columnMax[c] - element.Width) / 2, y + (rowMax[r] - element.Height) / 2);
                    x += columnMax[c] + ElementGap;
                }
                y += rowMax[r] + 5;
            }
        }
    }
}
