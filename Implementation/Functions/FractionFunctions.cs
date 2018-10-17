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

        public static Fraction Sin(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Sin(p1.GetValue()));
        }

        public static Fraction Cos(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Cos(p1.GetValue()));
        }

        public static Fraction Tan(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Tan(p1.GetValue()));
        }

        public static Fraction Asin(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Asin(p1.GetValue()));
        }

        public static Fraction Acos(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Acos(p1.GetValue()));
        }

        public static Fraction Atan(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Atan(p1.GetValue()));
        }

        public static Fraction Sinh(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Sinh(p1.GetValue()));
        }

        public static Fraction Cosh(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Cosh(p1.GetValue()));
        }

        public static Fraction Tanh(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Tanh(p1.GetValue()));
        }

        public static Fraction Ceil(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Ceiling(p1.GetValue()));
        }

        public static Fraction Round(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Round(p1.GetValue()));
        }

        public static Fraction Floor(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Floor(p1.GetValue()));
        }

        public static Fraction Min(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            Fraction p2 = parameters[1] as Fraction;
            return new Fraction(p1.GetValue() >= p2.GetValue() ? p2 : p1);
        }

        public static Fraction Max(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            Fraction p2 = parameters[1] as Fraction;
            return new Fraction(p1.GetValue() <= p2.GetValue() ? p2 : p1);
        }

        public static Fraction Log(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Log(p1.GetValue()));
        }

        public static Fraction Log10(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Log10(p1.GetValue()));
        }

        public static Fraction Exp(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            return new Fraction(Math.Exp(p1.GetValue()));
        }
    }
}
