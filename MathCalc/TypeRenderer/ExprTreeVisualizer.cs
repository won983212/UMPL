using ExprCore;
using ExprCore.Exceptions;
using ExprCore.Types;
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
    static class ExprTreeVisualizer
    {
        public static readonly Typeface FormulaFont = new Typeface("Cambria Math");
        public static void DrawTreeElements(DrawingContext ctx, double x, double y, double parents_width, double parents_height, List<TokenType> expr)
        {
            // Test
            expr = ExpressionParser.Tokenize("2*a+4-7*(2+5-6)/3");

            ConvertAsFormulaElement(expr, 14).Draw(ctx, x, y);
        }

        private static FormulaElement ConvertAsFormulaElement(List<TokenType> types, int size)
        {
            List<FormulaElement> elements = new List<FormulaElement>();
            elements.Add(new PowerElement(new FractionElement(new TextElement("1", size), new TextElement("3", size)), new TextElement("21", size)));
            elements.Add(new TextElement("+", size));
            elements.Add(new TextElement("10", size));

            FormulaElement[] fels = new FormulaElement[4];
            fels[0] = new TextElement("3", size);
            fels[1] = new TextElement("3", size);
            fels[2] = new TextElement("41", size);
            fels[3] = new FractionElement(new TextElement("12", size), new TextElement("21", size));
            double[] rows = new double[] { Math.Max(fels[0].Height, fels[1].Height), Math.Max(fels[2].Height, fels[3].Height) };
            double[] columns = new double[] { Math.Max(fels[0].Width, fels[2].Width), Math.Max(fels[1].Width, fels[3].Width) };

            MatrixElement el = new MatrixElement(rows, columns, fels);

            return new SequenceElement(new List<FormulaElement>() { new PowerElement(new TextElement("sin", size), new TextElement("-1", size)), new BracketElement(new FractionElement(new TextElement("1", size), new TextElement("2", size))) });
        }
    }
}
