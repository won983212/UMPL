using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Operator : TokenType
    {
        char op;

        public Operator(char op)
        {
            this.op = op;
        }

        public override bool Equals(object obj)
        {
            var @operator = obj as Operator;
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
            return "+-*/%^=@!()".IndexOf(c) >= 0;
        }
    }
}
