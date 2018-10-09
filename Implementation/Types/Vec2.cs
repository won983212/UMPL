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

        public override TokenType Evaluate(Dictionary<Variable, Number> var_values)
        {
            return new Vec2(X.Evaluate(var_values), Y.Evaluate(var_values));
        }

        // Operators
        public static Vec2 Add(TokenType left, TokenType right)
        {
            Vec2 l = left as Vec2;
            Vec2 r = right as Vec2;
            CheckNumber(l, r);
            return new Vec2(Number.Add(l.X, r.X), Number.Add(l.Y, r.Y));
        }

        public static Vec2 Subtract(TokenType left, TokenType right)
        {
            Vec2 l = left as Vec2;
            Vec2 r = right as Vec2;
            CheckNumber(l, r);
            return new Vec2(Number.Subtract(l.X, r.X), Number.Subtract(l.Y, r.Y));
        }

        public static Vec2 Scala(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Vec2 r = right as Vec2;
            CheckNumber(r);
            return new Vec2(Number.Multiply(l, r.X), Number.Multiply(l, r.Y));
        }

        public static Number Dot(TokenType left, TokenType right)
        {
            Vec2 l = left as Vec2;
            Vec2 r = right as Vec2;
            CheckNumber(l, r);
            return Number.Add(Number.Multiply(l.X, r.X), Number.Multiply(l.Y, r.Y));
        }
    }
}
