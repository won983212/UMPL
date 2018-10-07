using ExprCore.Exceptions;
using ExprCore.Operators;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    delegate void Visitor(Node node);

    // 식을 계산하고 저장할 때 사용하는 노드
    class Node
    {
        public TokenType data;
        public Node left;
        public Node right;

        public Node(TokenType data)
        {
            this.data = data;
        }

        public Node(TokenType data, Node left, Node right)
        {
            this.data = data;
            this.left = left;
            this.right = right;
        }

        public void VisitPostOrder(Visitor visitor)
        {
            if (left != null)
                left.VisitPostOrder(visitor);
            if (right != null)
                right.VisitPostOrder(visitor);

            visitor(this);
        }
    }

    // 식을 print할 때 사용하는 노드
    class ExprNode
    {
        public readonly int priority;
        public string Expr { get; private set; }

        public ExprNode(int priority, string expr)
        {
            this.priority = priority;
            Expr = expr;
        }

        public void PutBracket()
        {
            Expr = '(' + Expr + ')';
        }

        public override string ToString()
        {
            return Expr;
        }
    }

    class TypeTree
    {
        public Node root;

        public TypeTree(Node root)
        {
            this.root = root;
        }

        public TokenType Evaluate(Dictionary<Variable, Number> var_values)
        {
            Stack<TokenType> stack = new Stack<TokenType>();

            root.VisitPostOrder((node) =>
            {
                if(node.data is Operator)
                {
                    TokenType p2 = stack.Pop().Evaluate(var_values);
                    TokenType p1 = stack.Pop().Evaluate(var_values);
                    stack.Push(OperatorRegistry.ExecuteBinaryOperation((Operator) node.data, p1, p2));
                }
                else
                {
                    stack.Push(node.data);
                }
            });

            if (stack.Count != 1)
                throw new ExprCoreException("식이 잘못되었습니다.");

            return stack.Pop().Evaluate(var_values);
        }

        public override string ToString()
        {
            Stack<ExprNode> stack = new Stack<ExprNode>();

            root.VisitPostOrder((node) =>
            {
                if(node.data is Operator)
                {
                    Operator op = (Operator)node.data;
                    ExprNode e2 = stack.Pop();
                    ExprNode e1 = stack.Pop();
                    
                    if (e1.priority != -1 && e1.priority < op.priority)
                        e1.PutBracket();
                    if (e2.priority != -1 && (e2.priority < op.priority || (e2.priority == op.priority && op.op == '-')))
                        e2.PutBracket();

                    stack.Push(new ExprNode(op.priority, e1 + node.data.ToString() + e2));
                }
                else
                {
                    stack.Push(new ExprNode(-1, node.data.ToString()));
                }
            });

            if (stack.Count != 1)
                throw new ExprCoreException("식이 잘못되었습니다.");

            return stack.Pop().Expr;
        }
    }
}
