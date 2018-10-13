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
        public const double ElementGap = 3;
        public const double BracketWidth = 3;
        private readonly double rowHeight;
        private readonly double columnWidth;
        private readonly int rows;
        private readonly int columns;
        private readonly FormulaElement[] elements;

        public MatrixElement(double rowHeight, double columnWidth, int rows, int columns, FormulaElement[] elements)
        {
            if (rowHeight > columnWidth)
                columnWidth = rowHeight;

            this.rowHeight = rowHeight;
            this.columnWidth = columnWidth;
            this.rows = rows;
            this.columns = columns;
            this.elements = elements;

            Width = (columns + 1) * ElementGap + columns * columnWidth + BracketWidth * 2;
            Height = (rows - 1) * ElementGap + rows * rowHeight;
        }

        public override void Draw(DrawingContext ctx)
        {
            double x = 0;
            double y = 0;
            int i = 0;

            ctx.DrawLine(LinePen, new Point(0, 0), new Point(0, Height));
            ctx.DrawLine(LinePen, new Point(0, 0), new Point(BracketWidth, 0));
            ctx.DrawLine(LinePen, new Point(0, Height), new Point(BracketWidth, Height));

            ctx.DrawLine(LinePen, new Point(Width, 0), new Point(Width, Height));
            ctx.DrawLine(LinePen, new Point(Width - BracketWidth, 0), new Point(Width, 0));
            ctx.DrawLine(LinePen, new Point(Width - BracketWidth, Height), new Point(Width, Height));

            for (int r = 0; r < rows; r++)
            {
                x = BracketWidth + ElementGap;
                for(int c = 0; c < columns; c++)
                {
                    FormulaElement element = elements[i++];
                    element.Draw(ctx, x + (columnWidth - element.Width) / 2, y + (rowHeight - element.Height) / 2);
                    x += columnWidth + ElementGap;
                }
                y += rowHeight + ElementGap;
            }
        }
    }
}
