using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    class Node
    {
        public TokenType data;
        public Node left;
        public Node right;

        public Node(TokenType data, Node left, Node right)
        {
            this.data = data;
            this.left = left;
            this.right = right;
        }
    }

    class TypeTree
    {
        public Node rootNode;

        public TypeTree(Node root)
        {
            this.rootNode = root;
        }
    }
}
