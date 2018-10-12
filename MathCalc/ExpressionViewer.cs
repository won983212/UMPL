using ExprCore;
using ExprCore.Exceptions;
using MathCalc.TypeRenderer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MathCalc
{
    class ExpressionViewer : FrameworkElement
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            TypeTree expr = ExpressionParser.ParseExpression("(b+sqrt(b^2-4/3*a*c))/(2*a)").ExprTree;
            ExprTreeVisualizer.DrawTreeElements(drawingContext, 10, 10, 14, expr);
        }
    }
}
