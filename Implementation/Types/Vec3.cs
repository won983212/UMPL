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

        public override TokenType Evaluate(Dictionary<Variable, Fraction> var_values)
        {
            return new Vec3(X.Evaluate(var_values), Y.Evaluate(var_values), Z.Evaluate(var_values));
        }

        // Operators
        public static Vec3 Add(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            CheckNumber(l, r);
            return new Vec3(Fraction.Add(l.X, r.X), Fraction.Add(l.Y, r.Y), Fraction.Add(l.Z, r.Z));
        }

        public static Vec3 Subtract(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            CheckNumber(l, r);
            return new Vec3(Fraction.Subtract(l.X, r.X), Fraction.Subtract(l.Y, r.Y), Fraction.Subtract(l.Z, r.Z));
        }

        public static Vec3 Scala(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Vec3 r = right as Vec3;
            CheckNumber(r);
            return new Vec3(Fraction.Multiply(l, r.X), Fraction.Multiply(l, r.Y), Fraction.Multiply(l, r.Z));
        }

        public static Fraction Dot(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            CheckNumber(r);
            return Fraction.Add(Fraction.Add(Fraction.Multiply(l.X, r.X), Fraction.Multiply(l.Y, r.Y)), Fraction.Multiply(l.Z, r.Z));
        }

        public static Vec3 Cross(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            CheckNumber(r);

            Fraction x = Fraction.Subtract(Fraction.Multiply(l.Y, r.Z), Fraction.Multiply(l.Z, r.Y));
            Fraction y = Fraction.Subtract(Fraction.Multiply(l.Z, r.X), Fraction.Multiply(l.X, r.Z));
            Fraction z = Fraction.Subtract(Fraction.Multiply(l.X, r.Y), Fraction.Multiply(l.Y, r.X));
            return new Vec3(x, y, z);
        }
    }
}
