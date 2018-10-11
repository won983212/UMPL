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
        public readonly Node root;
        public readonly List<TokenType> original;

        public TypeTree(List<TokenType> original, Node root)
        {
            this.original = original;
            this.root = root;
        }

        // OperatorProcess: 현재 노드가 operator면 스택에 넣을 T
        // OperatorProcess: 현재 노드가 operator가 아니면 스택에 넣을 T
        // Node, T, T, T : 현재 노드, 팝된 1번째 노드, 팝된 2번째 노드, stack에 추가할 T
        public T ProcessCalculate<T>(Func<Node, T, T, T> OperatorProcess, Func<Node, T> NonOperatorProcess)
        {
            Stack<T> stack = new Stack<T>();

            root.VisitPostOrder((node) =>
            {
                if (node.data is Operator)
                {
                    T p2 = stack.Pop();
                    T p1 = stack.Pop();
                    stack.Push(OperatorProcess(node, p1, p2));
                }
                else
                {
                    stack.Push(NonOperatorProcess(node));
                }
            });

            if (stack.Count != 1)
                throw new ExprCoreException("식이 잘못되었습니다.");

            return stack.Pop();
        }

        public TokenType Evaluate(Dictionary<Variable, Number> var_values)
        {
            return ProcessCalculate((node, p1, p2) => {
                return OperatorRegistry.ExecuteBinaryOperation((Operator)node.data, p1.Evaluate(var_values), p2.Evaluate(var_values));
            },
            (node) => {
                return node.data;
            }).Evaluate(var_values);
        }

        public override string ToString()
        {
            return ProcessCalculate((node, e1, e2) => {
                Operator op = (Operator)node.data;
                if (e1.priority != -1 && e1.priority < op.priority)
                    e1.PutBracket();
                if (e2.priority != -1 && (e2.priority < op.priority || (e2.priority == op.priority && op.op == '-')))
                    e2.PutBracket();

                return new ExprNode(op.priority, e1 + node.data.ToString() + e2);
            },
            (node) => {
                return new ExprNode(-1, node.data.ToString());
            }).Expr;
        }
    }
}
