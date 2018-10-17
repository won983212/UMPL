using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class Vec3Operators
    {
        public static Vec3 Add(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            Vector.CheckNumberic(l, r);
            return new Vec3(FractionOperators.Add(l.X, r.X), FractionOperators.Add(l.Y, r.Y), FractionOperators.Add(l.Z, r.Z));
        }

        public static Vec3 Subtract(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            Vector.CheckNumberic(l, r);
            return new Vec3(FractionOperators.Subtract(l.X, r.X), FractionOperators.Subtract(l.Y, r.Y), FractionOperators.Subtract(l.Z, r.Z));
        }

        public static Vec3 Scala(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Vec3 r = right as Vec3;
            Vector.CheckNumberic(r);
            return new Vec3(FractionOperators.Multiply(l, r.X), FractionOperators.Multiply(l, r.Y), FractionOperators.Multiply(l, r.Z));
        }

        public static Fraction Dot(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            Vector.CheckNumberic(r);
            return FractionOperators.Add(FractionOperators.Add(FractionOperators.Multiply(l.X, r.X), FractionOperators.Multiply(l.Y, r.Y)), FractionOperators.Multiply(l.Z, r.Z));
        }

        public static Vec3 Cross(TokenType left, TokenType right)
        {
            Vec3 l = left as Vec3;
            Vec3 r = right as Vec3;
            Vector.CheckNumberic(r);

            Fraction x = FractionOperators.Subtract(FractionOperators.Multiply(l.Y, r.Z), FractionOperators.Multiply(l.Z, r.Y));
            Fraction y = FractionOperators.Subtract(FractionOperators.Multiply(l.Z, r.X), FractionOperators.Multiply(l.X, r.Z));
            Fraction z = FractionOperators.Subtract(FractionOperators.Multiply(l.X, r.Y), FractionOperators.Multiply(l.Y, r.X));
            return new Vec3(x, y, z);
        }

        public static Vec3 Negative(TokenType operand)
        {
            Vec3 vec = operand as Vec3;
            Vector.CheckNumberic(vec);
            return new Vec3(FractionOperators.Negative(vec.X), FractionOperators.Negative(vec.Y), FractionOperators.Negative(vec.Z));
        }
    }
}
