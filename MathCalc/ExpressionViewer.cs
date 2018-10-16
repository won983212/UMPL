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
        public static readonly DependencyProperty TargerViewProperty =
                DependencyProperty.Register("TargetView", typeof(TokenType), typeof(ExpressionViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnViewChanged)));
        public static readonly DependencyProperty TextSizeProperty =
                DependencyProperty.Register("TextSize", typeof(int), typeof(ExpressionViewer), new FrameworkPropertyMetadata(14, new PropertyChangedCallback(OnViewChanged)));
        private static readonly Typeface font = new Typeface("맑은 고딕");

        public TokenType TargetView
        {
            get { return (TokenType)GetValue(TargerViewProperty); }
            set { SetValue(TargerViewProperty, value); }
        }

        public int TextSize
        {
            get { return (int)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }

        private DrawingGroup group = new DrawingGroup();
        private Size size = new Size(0, 0);

        private static void OnViewChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            ExpressionViewer t = obj as ExpressionViewer;
            t.RenderDrawing();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            RenderDrawing();
            drawingContext.DrawDrawing(group);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return size;
        }

        private void RenderDrawing()
        {
            using (DrawingContext ctx = group.Open())
            {
                string warn = null;

                if (TargetView == null)
                    warn = "식이 비어있습니다.";

                if (TargetView is ErrorTextTokenType error)
                    warn = error.ErrorMessage;

                if (warn != null)
                {
                    FormattedText warnText = new FormattedText(warn, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, font, TextSize, Brushes.Red, 1);
                    ctx.DrawText(warnText, new Point(0, 0));
                    size = new Size(warnText.Width, warnText.Height);
                }
                else
                {
                    size = ExprTreeVisualizer.DrawElements(ctx, 0, 0, TextSize, TargetView);
                }
            }
        }
    }
}
