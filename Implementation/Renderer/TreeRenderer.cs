using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Renderer
{
    class TreeRenderer
    {
        public const int SquareSize = 30;
        public const int DefaultDistance = 40;

        public static void RenderTree(IRenderContext g, TypeTree tree)
        {
            RenderNode rRoot = RenderNode.ConvertFromNode(tree.root);
            Size size = rRoot.GetSize();

            g.DrawBackground(size);
            DrawTree(g, rRoot, rRoot.GetOriginX((int)size.Width), SquareSize);
        }

        private static Size DrawTree(IRenderContext g, RenderNode rRoot, int x, int y)
        {
            RenderNode left;
            RenderNode right;
            Size size;

            foreach (RenderNode n in rRoot.GetAllNodes())
            {
                bool hasChild = false;
                int nx = n.x + x;
                int ny = n.y + y;

                g.DrawRectangle(nx - SquareSize / 2, ny - SquareSize / 2, SquareSize, SquareSize);
                size = g.MeasureString(n.display);
                g.DrawString(n.display, nx - size.Width / 2, ny - size.Height / 2);

                if (!n.isRoot)
                    g.DrawLine(nx, ny - SquareSize / 2, nx, ny - DefaultDistance / 2);

                left = n.GetLeft();
                right = n.GetRight();

                if (left != null)
                {
                    g.DrawLine(nx, ny + DefaultDistance / 2, left.x + x, ny + DefaultDistance / 2);
                    hasChild = true;
                }

                if (right != null)
                {
                    g.DrawLine(nx, ny + DefaultDistance / 2, right.x + x, ny + DefaultDistance / 2);
                    hasChild = true;
                }

                if (hasChild)
                    g.DrawLine(nx, ny + SquareSize / 2, nx, ny + DefaultDistance / 2);
            }

            return rRoot.GetSize();
        }
    }

    class RenderNode
    {
        public int x = 0;
        public int y = 0;
        public string display;
        public bool isRoot = false;

        int xMin = -1;
        int xMax = -1;
        int xOffset = 0;
        int height = -1;
        RenderNode left = null;
        RenderNode right = null;

        public RenderNode(Node node)
        {
            display = node.data.ToString();
        }

        public RenderNode GetLeft()
        {
            return left;
        }

        public RenderNode GetRight()
        {
            return right;
        }

        public string GetDisplayedXBounds()
        {
            return xMin + "~" + xMax;
        }

        public int GetOriginX(int width)
        {
            return width - xMax - TreeRenderer.SquareSize;
        }

        public Size GetSize()
        {
            if (height == -1) MeasureHeight();
            return new Size(xMax - xMin + TreeRenderer.SquareSize * 2, height * TreeRenderer.DefaultDistance + TreeRenderer.SquareSize * 2);
        }

        // pre-order iterate
        public List<RenderNode> GetAllNodes()
        {
            List<RenderNode> nodes = new List<RenderNode>();
            Stack<RenderNode> stack = new Stack<RenderNode>();

            stack.Push(this);
            while (stack.Count > 0)
            {
                RenderNode node = stack.Pop();
                nodes.Add(node);
                if (node.right != null)
                    stack.Push(node.right);
                if (node.left != null)
                    stack.Push(node.left);
            }

            return nodes;
        }

        private void EstimateXBounds()
        {
            if (left != null)
            {
                left.EstimateXBounds();
                MergeBounds(left, false);
            }

            if (right != null)
            {
                right.EstimateXBounds();
                MergeBounds(right, false);
            }

            if (xMin == -1 || xMin > x)
                xMin = x;

            if (xMax == -1 || xMax < x)
                xMax = x;
        }

        private void Arrange()
        {
            if (left != null)
                left.Arrange();

            if (right != null)
                right.Arrange();

            if (left != null && right != null)
            {
                int overlapped = left.xMax - right.xMin + TreeRenderer.SquareSize + TreeRenderer.DefaultDistance;

                if (overlapped > 0)
                {
                    left.xOffset -= overlapped / 2;
                    MergeBounds(left, true);

                    right.xOffset += overlapped / 2;
                    MergeBounds(right, true);
                }
            }
        }

        private void ApplyXOffset()
        {
            ApplyXOffset(xOffset);
        }

        private void ApplyXOffset(int offsetStack)
        {
            x += offsetStack;
            xMin += offsetStack;
            xMax += offsetStack;

            if (left != null)
            {
                left.ApplyXOffset(offsetStack + left.xOffset);
                MergeBounds(left, false);
            }

            if (right != null)
            {
                right.ApplyXOffset(offsetStack + right.xOffset);
                MergeBounds(right, false);
            }
        }

        private void MergeBounds(RenderNode node, bool withOffset)
        {
            int offset = withOffset ? node.xOffset : 0;

            if (node.xMin + offset < xMin || xMin == -1)
                xMin = node.xMin + offset;

            if (node.xMax + offset > xMax || xMax == -1)
                xMax = node.xMax + offset;
        }

        private void MeasureHeight()
        {
            IterateHeight(this, 1);
        }

        private void IterateHeight(RenderNode root, int height)
        {
            if (left != null)
                left.IterateHeight(root, height + 1);

            if (right != null)
                right.IterateHeight(root, height + 1);

            if (root.height < height)
                root.height = height;
        }

        public static RenderNode ConvertFromNode(Node root)
        {
            RenderNode newNode = MakeNode(root, 0, 0);
            newNode.isRoot = true;
            newNode.EstimateXBounds();
            newNode.Arrange();
            newNode.ApplyXOffset();

            return newNode;
        }

        private static RenderNode MakeNode(Node node, int px, int py)
        {
            RenderNode rNode = new RenderNode(node);
            rNode.x = px;
            rNode.y = py;

            if (node.left != null)
                rNode.left = MakeNode(node.left, px - TreeRenderer.DefaultDistance, py + TreeRenderer.DefaultDistance);

            if (node.right != null)
                rNode.right = MakeNode(node.right, px + TreeRenderer.DefaultDistance, py + TreeRenderer.DefaultDistance);

            return rNode;
        }
    }
}
