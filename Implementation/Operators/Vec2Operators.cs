using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class Vec2Operators
    {
        public static Vec2 Add(TokenType left, TokenType right)
        {
            Vec2 l = left as Vec2;
            Vec2 r = right as Vec2;
            Vector.CheckNumberic(l, r);
            return new Vec2(FractionOperators.Add(l.X, r.X), FractionOperators.Add(l.Y, r.Y));
        }

        public static Vec2 Subtract(TokenType left, TokenType right)
        {
            Vec2 l = left as Vec2;
            Vec2 r = right as Vec2;
            Vector.CheckNumberic(l, r);
            return new Vec2(FractionOperators.Subtract(l.X, r.X), FractionOperators.Subtract(l.Y, r.Y));
        }

        public static Vec2 Scala(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Vec2 r = right as Vec2;
            Vector.CheckNumberic(r);
            return new Vec2(FractionOperators.Multiply(l, r.X), FractionOperators.Multiply(l, r.Y));
        }

        public static Fraction Dot(TokenType left, TokenType right)
        {
            Vec2 l = left as Vec2;
            Vec2 r = right as Vec2;
            Vector.CheckNumberic(l, r);
            return FractionOperators.Add(FractionOperators.Multiply(l.X, r.X), FractionOperators.Multiply(l.Y, r.Y));
        }

        public static Vec2 Negative(TokenType operand)
        {
            Vec2 vec = operand as Vec2;
            Vector.CheckNumberic(vec);
            return new Vec2(FractionOperators.Negative(vec.X), FractionOperators.Negative(vec.Y));
        }
    }
}
