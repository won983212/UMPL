using ExprCore.Exceptions;
using ExprCore.Functions;
using ExprCore.Operators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Fraction : TokenType
    {
        const double ERROR = 0.0000001;
        public long numerator;
        public long denomiator;

        public Fraction(long number) : this(number, 1) { }

        public Fraction(Fraction number) : this(number.numerator, number.denomiator) { }

        public Fraction(double realnumber)
        {
            IsConstant = true;
            if ((realnumber - (long)realnumber) == 0)
            {
                Initialize((long)realnumber, 1);
                return;
            }

            numerator = 1;
            denomiator = 0;

            bool neg = realnumber < 0;
            double b = neg ? -realnumber : realnumber;
            long pn = 0, pd = 1;
            long a, temp;

            do
            {
                a = (int)b;

                temp = numerator;
                numerator = a * numerator + pn;
                pn = temp;

                temp = denomiator;
                denomiator = a * denomiator + pd;
                pd = temp;

                b = 1 / (b - a);
            } while (Math.Abs(realnumber - (double)numerator / denomiator) > realnumber * ERROR);

            if (neg)
                numerator *= -1;

            Reduce();
        }

        public Fraction(long numerator, long denomiator)
        {
            Initialize(numerator, denomiator);
            IsConstant = true;
        }

        private void Initialize(long n, long d)
        {
            if (n < 0 && d < 0 || n > 0 && d < 0)
            {
                numerator = -n;
                denomiator = -d;
            }
            else
            {
                numerator = n;
                denomiator = d;
            }
        }

        public bool IsInteger()
        {
            return GetValue() == (long)GetValue();
        }

        public double GetValue()
        {
            return (double)numerator / denomiator;
        }

        public Fraction Reduce()
        {
            long k = MathHelper.GCD(Math.Abs(numerator), Math.Abs(denomiator));
            numerator /= k;
            denomiator /= k;

            if (denomiator < 0)
            {
                numerator *= -1;
                denomiator *= -1;
            }

            return this;
        }

        public override bool Equals(object obj)
        {
            var number = obj as Fraction;
            return number != null &&
                   numerator == number.numerator &&
                   denomiator == number.denomiator;
        }

        public override int GetHashCode()
        {
            var hashCode = -1076504861;
            hashCode = hashCode * -1521134295 + numerator.GetHashCode();
            hashCode = hashCode * -1521134295 + denomiator.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            if (denomiator == 1)
            {
                return numerator.ToString();
            }

            return numerator + "/" + denomiator;
        }

        public override bool IsAcceptable(Type type)
        {
            if (type == typeof(Constant))
                return true;
            return base.IsAcceptable(type);
        }

        public static implicit operator Fraction(double d)
        {
            return new Fraction(d);
        }

        // Operators
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

        // Functions
        private static void CheckIfBothInteger(Fraction p1, Fraction p2)
        {
            if (!(p1.IsInteger() && p2.IsInteger()))
                throw new ExprCoreException("정수에 대해서만 GCD연산이 가능합니다.");
        }

        public static Fraction Gcd(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            Fraction p2 = parameters[1] as Fraction;
            CheckIfBothInteger(p1, p2);
            return new Fraction(MathHelper.GCD((long) p1.GetValue(), (long)p2.GetValue()));
        }

        public static Fraction Lcm(List<TokenType> parameters)
        {
            Fraction p1 = parameters[0] as Fraction;
            Fraction p2 = parameters[1] as Fraction;
            CheckIfBothInteger(p1, p2);
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

        // Untility
        public static Fraction Sqrt(Fraction n)
        {
            return new Fraction(Math.Sqrt(n.GetValue()));
        }
    }
}
