using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Renderer
{
    public struct Size
    {
        public double Width;
        public double Height;

        public Size(double w, double h)
        {
            Width = w;
            Height = h;
        }
    }

    public interface IRenderContext
    {
        void DrawRectangle(int x, int y, int w, int h);

        void DrawString(string text, double x, double y);

        void DrawLine(int x1, int y1, int x2, int y2);

        void DrawBackground(Size size);

        Size MeasureString(string text);
    }
}
