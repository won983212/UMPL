using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Functions
{
    class FractionFunctions
    {
        public static Fraction Gcd(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            Fraction p2 = parameters[1] as Fraction;
            Fraction.CheckIfBothInteger(p1, p2);
            return new Fraction(MathHelper.GCD((long)p1.GetValue(), (long)p2.GetValue()));
        }

        public static Fraction Lcm(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            Fraction p2 = parameters[1] as Fraction;
            Fraction.CheckIfBothInteger(p1, p2);
            return new Fraction(MathHelper.LCM((long)p1.GetValue(), (long)p2.GetValue()));
        }

        public static Fraction Reduce(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(p1).Reduce();
        }

        public static Fraction Sqrt(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Sqrt(p1.GetValue()));
        }

        public static Fraction Abs(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Abs(p1.numerator), Math.Abs(p1.denomiator));
        }

    }
}
