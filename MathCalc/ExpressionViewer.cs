using ExprCore;
using ExprCore.Exceptions;
using ExprCore.Types;
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
        public static readonly DependencyProperty ExprTextProperty =
                DependencyProperty.Register("ExprText", typeof(string), typeof(ExpressionViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnExprTextChanged)));
        private static readonly Typeface font = new Typeface("맑은 고딕");

        public string ExprText
        {
            get { return (string)GetValue(ExprTextProperty); }
            set { SetValue(ExprTextProperty, value); }
        }

        private DrawingGroup group = new DrawingGroup();
        private ExprCore.Types.Expression input;
        private TokenType target;

        private static void OnExprTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            ExpressionViewer t = obj as ExpressionViewer;
            t.UpdateExpression(t.ExprText);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            RenderDrawing(null);
            drawingContext.DrawDrawing(group);
        }

        public void UpdateExpression(string expr)
        {
            string warn = null;

            try
            {
                input = ExpressionParser.ParseExpression(expr);
                target = input.Evaluate();
            }
            catch (ExprCoreException e)
            {
                warn = e.Message;
            }

            RenderDrawing(warn);
        }

        private void RenderDrawing(string warning)
        {
            using (DrawingContext ctx = group.Open())
            {
                if (warning != null)
                {
                    FormattedText warningText = new FormattedText(warning, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, font, 12, Brushes.Red, 1);
                    ctx.DrawText(warningText, new Point(5, 0));
                    ctx.PushTransform(new TranslateTransform(0, 20));
                }

                if (input != null && target != null)
                {
                    double height = ExprTreeVisualizer.DrawElements(ctx, 10, 10, 14, input).Height;
                    ExprTreeVisualizer.DrawElements(ctx, 10, 20 + height, 14, target);
                }

                if (warning != null)
                {
                    ctx.Pop();
                }
            }
        }
    }
}
