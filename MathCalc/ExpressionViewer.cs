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
            ExprTreeVisualizer.DrawTreeElements(drawingContext, ActualWidth, ActualHeight, null);
        }
    }
}
