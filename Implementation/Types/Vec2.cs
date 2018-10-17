using ExprCore.Exceptions;
using ExprCore.Functions;
using ExprCore.Operators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Vec2 : Vector
    {
        public TokenType X => vecData[0];
        public TokenType Y => vecData[1];

        public Vec2(List<TokenType> elements) : base(new TokenType[] { elements[0], elements[1] })
        {
            if (elements.Count != 2)
                throw new ExprCoreException("Vec2는 인자가 2개 필요합니다.");
        }

        public Vec2(TokenType x, TokenType y) : base(new TokenType[] { x, y })
        {
        }

        public override string ToString()
        {
            return "Vec2(" + X + ", " + Y + ")";
        }

        public override TokenType Evaluate(Dictionary<Variable, Fraction> var_values)
        {
            return new Vec2(X.Evaluate(var_values), Y.Evaluate(var_values));
        }
    }
}
