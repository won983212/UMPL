using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class FractionOperators
    {
        public static Fraction Add(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Fraction r = right as Fraction;
            long lcm = MathHelper.LCM(l.denomiator, r.denomiator);
            return new Fraction(l.numerator * lcm / l.denomiator + r.numerator * lcm / r.denomiator, lcm).Reduce();
        }

        public static Fraction Subtract(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Fraction r = right as Fraction;
            long lcm = MathHelper.LCM(l.denomiator, r.denomiator);
            return new Fraction(l.numerator * lcm / l.denomiator - r.numerator * lcm / r.denomiator, lcm).Reduce();
        }

        public static Fraction Multiply(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Fraction r = right as Fraction;
            return new Fraction(l.numerator * r.numerator, l.denomiator * r.denomiator).Reduce();
        }

        public static Fraction Divide(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Fraction r = right as Fraction;
            return new Fraction(l.numerator * r.denomiator, l.denomiator * r.numerator).Reduce();
        }

        public static Fraction Power(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Fraction r = right as Fraction;
            return new Fraction(Math.Pow(l.GetValue(), r.GetValue())).Reduce();
        }

        public static Fraction Mod(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Fraction r = right as Fraction;
            return new Fraction(l.GetValue() % r.GetValue()).Reduce();
        }

        public static Fraction Negative(TokenType operand)
        {
            Fraction n = operand as Fraction;
            return new Fraction(-n.numerator, n.denomiator);
        }
    }
}
