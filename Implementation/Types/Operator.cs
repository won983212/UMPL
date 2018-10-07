using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Operator : TokenType
    {
        private static readonly string Operators = "+-*/%^=@!(){}[],";
        private static readonly int[] OperatorPriority = new int[] { 3, 3, 4, 4, 2, 5, 1, 4, 5, 0, 0, 0, 0, 0, 0, 0 };
        public readonly char op;
        public readonly int priority;

        public Operator(char op)
        {
            this.op = op;
            priority = GetOperatorPriority(op);
        }

        public override bool Equals(object obj)
        {
            Operator @operator = obj as Operator;
            return @operator != null && op == @operator.op;
        }

        public override int GetHashCode()
        {
            return op.GetHashCode();
        }

        public override string ToString()
        {
            return op.ToString();
        }

        public static bool IsOperatorCharacter(char c)
        {
            return Operators.IndexOf(c) >= 0;
        }

        public static int GetOperatorPriority(char c)
        {
            int i = Operators.IndexOf(c);
            if(i >= 0)
                return OperatorPriority[i];
            return -1;
        }
    }
}
