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
    class FormulaToken
    {
        public readonly int priority;
        private List<FormulaElement> elements;

        public FormulaToken(FormulaElement startElement)
        {
            priority = -1;
            elements = new List<FormulaElement>() { startElement };
        }

        public FormulaToken(int priority, FormulaToken token1, FormulaToken token2, TextElement operatorElement)
        {
            this.priority = priority;
            elements = new List<FormulaElement>();
            elements.AddRange(token1.elements);
            elements.Add(operatorElement);
            elements.AddRange(token2.elements);
        }

        public void PutBracket()
        {
            elements = new List<FormulaElement>() { new BracketElement(ToFormulaElement()) };
        }

        public bool IsType(Type type)
        {
            if (elements.Count == 1)
                return type.IsAssignableFrom(elements[0].GetType());
            return type == typeof(SequenceElement);
        }

        public FormulaElement ToFormulaElement()
        {
            if (elements.Count == 1)
                return elements[0];
            else
                return new SequenceElement(elements);
        }
    }

    static class ExprTreeVisualizer
    {
        public static readonly Typeface FormulaFont = new Typeface("Cambria Math");

        public static Size DrawElements(DrawingContext ctx, double x, double y, int fontSize, TokenType type)
        {
            FormulaElement fe = CreateFormulaElement(type, fontSize);
            Size size = new Size(fe.Width, fe.Height);
            fe.Draw(ctx, x, y);
            return size;
        }

        private static FormulaElement ConvertAsFormulaElement(TypeTree expr, int size)
        {
            return expr.ProcessCalculate(
            (op, e1, e2) =>
            {
                if(op.op == '/')
                    return new FormulaToken(new FractionElement(e1.ToFormulaElement(), e2.ToFormulaElement()));
                else if(op.op == '^')
                    return new FormulaToken(new PowerElement(e1.ToFormulaElement(), e2.ToFormulaElement()));
                else
                {
                    if (e1.priority != -1 && e1.priority < op.priority)
                        e1.PutBracket();
                    if (e2.priority != -1 && (e2.priority < op.priority || (e2.priority == op.priority && op.op == '-')))
                        e2.PutBracket();

                    string opText = op.ToString();
                    if (op.op == '*')
                        if (e1.IsType(typeof(MatrixElement)) && e2.IsType(typeof(MatrixElement)))
                            opText = "×";
                        else
                            opText = "·";

                    return new FormulaToken(op.priority, e1, e2, new TextElement(opText, size));
                }
            },
            (node) =>
            {
                return new FormulaToken(CreateFormulaElement(node, size));
            }).ToFormulaElement();
        }

        private static FormulaElement CreateFormulaElement(TokenType type, int size)
        {
            int i = 0;
            if(type is ExprCore.Types.Expression expr)
            {
                return ConvertAsFormulaElement(expr.ExprTree, size);
            }
            else if(type is Function func)
            {
                if (func.funcName == "sqrt")
                    return new SquareRootElement(CreateFormulaElement(func.parameters[0], size));
                else if (func.funcName == "abs")
                    return new AbsoluteElement(CreateFormulaElement(func.parameters[0], size));
                else
                {
                    FormulaElement[] parameters = new FormulaElement[func.parameters.Count];
                    foreach (TokenType t in func.parameters)
                    {
                        parameters[i++] = CreateFormulaElement(t, size);
                    }
                    return new SequenceElement(new List<FormulaElement>() { new TextElement(func.funcName, size), new CommaBracketElement(parameters) });
                }
            }
            else if(type is ExprCore.Types.Matrix mat)
            {
                double rowMax = 0;
                double columnMax = 0;
                FormulaElement[] elements = new FormulaElement[mat.rows * mat.columns];
                for(int r = 0; r < mat.rows; r++)
                {
                    for(int c = 0; c < mat.columns; c++)
                    {
                        elements[i] = CreateFormulaElement(mat.data[r, c], size);
                        rowMax = Math.Max(rowMax, elements[i].Height);
                        columnMax = Math.Max(columnMax, elements[i].Width);
                        i++;
                    }
                }
                return new MatrixElement(rowMax, columnMax, mat.rows, mat.columns, elements);
            }
            else if(type is ExprCore.Types.Vector vec)
            {
                FormulaElement[] elements = new FormulaElement[vec.vecData.Length];
                foreach (TokenType t in vec.vecData)
                {
                    elements[i++] = CreateFormulaElement(t, size);
                }
                return new CommaBracketElement(elements);
            }
            else if(type is Fraction frac)
            {
                FormulaElement n = new TextElement(frac.numerator.ToString(), size);
                FormulaElement d = new TextElement(frac.denomiator.ToString(), size);
                if (frac.denomiator == 1)
                    return n;
                else
                    return new FractionElement(n, d);
            }
            else
            {
                return new TextElement(type.ToString(), size);
            }
        }
    }
}
