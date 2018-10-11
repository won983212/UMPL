using ExprCore;
using ExprCore.Exceptions;
using ExprCore.Renderer;
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
    class TreeViewer : FrameworkElement
    {
        public static readonly DependencyProperty ExprTextProperty = 
            DependencyProperty.Register("ExprText", typeof(string), typeof(TreeViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnExprTextChanged)));

        public string ExprText
        {
            get { return (string) GetValue(ExprTextProperty); }
            set { SetValue(ExprTextProperty, value); }
        }

        private DrawingGroup group = new DrawingGroup();
        private TypeTree expr;

        public TreeViewer()
        {
            RenderOptions.SetEdgeMode(group, EdgeMode.Aliased);
        }

        private static void OnExprTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            TreeViewer t = obj as TreeViewer;
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
                this.expr = ExpressionParser.ParseExpression(expr).ExprTree;
            } 
            catch(ExprCoreException e)
            {
                warn = e.Message;
            }

            RenderDrawing(warn);
        }

        private void RenderDrawing(string warning)
        {
            if (expr == null)
                return;

            using (DrawingContext ctx = group.Open())
            {
                if (warning != null)
                {
                    Typeface type = new Typeface("맑은 고딕");
                    FormattedText warningText = new FormattedText(warning, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, type, 12, Brushes.Red, 1);
                    ctx.DrawText(warningText, new Point(5, 0));
                    ctx.PushTransform(new TranslateTransform(0, 20));
                }

                TreeRenderer.RenderTree(new WpfTreeRenderContext(ctx), expr);

                if (warning != null)
                {
                    ctx.Pop();
                }
            }
        }
    }
}
