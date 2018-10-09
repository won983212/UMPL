using ExprCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Vec3 : Vector
    {
        public TokenType X => vecData[0];
        public TokenType Y => vecData[1];
        public TokenType Z => vecData[2];

        public Vec3(List<TokenType> elements) : base(new TokenType[] { elements[0], elements[1], elements[2] })
        {
            if (elements.Count != 3)
                throw new ExprCoreException("Vec3는 인자가 3개 필요합니다.");
        }

        public Vec3(TokenType x, TokenType y, TokenType z) : base(new TokenType[] { x, y, z })
        { }

        public override string ToString()
        {
            return "Vec3(" + X + ", " + Y + ", " + Z + ")";
        }

        public override TokenType Evaluate(Dictionary<Variable, Number> var_values)
        {
            return new Vec3(X.Evaluate(var_values), Y.Evaluate(var_values), Z.Evaluate(var_values));
        }

        // Operators
        public static Vec3 Add(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            CheckNumber(l, r);
            return new Vec3(Number.Add(l.X, r.X), Number.Add(l.Y, r.Y), Number.Add(l.Z, r.Z));
        }

        public static Vec3 Subtract(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            CheckNumber(l, r);
            return new Vec3(Number.Subtract(l.X, r.X), Number.Subtract(l.Y, r.Y), Number.Subtract(l.Z, r.Z));
        }

        public static Vec3 Scala(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Vec3 r = right as Vec3;
            CheckNumber(r);
            return new Vec3(Number.Multiply(l, r.X), Number.Multiply(l, r.Y), Number.Multiply(l, r.Z));
        }

        public static Number Dot(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            CheckNumber(r);
            return Number.Add(Number.Add(Number.Multiply(l.X, r.X), Number.Multiply(l.Y, r.Y)), Number.Multiply(l.Z, r.Z));
        }

        public static Vec3 Cross(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            CheckNumber(r);

            Number x = Number.Subtract(Number.Multiply(l.Y, r.Z), Number.Multiply(l.Z, r.Y));
            Number y = Number.Subtract(Number.Multiply(l.Z, r.X), Number.Multiply(l.X, r.Z));
            Number z = Number.Subtract(Number.Multiply(l.X, r.Y), Number.Multiply(l.Y, r.X));
            return new Vec3(x, y, z);
        }
    }
}
