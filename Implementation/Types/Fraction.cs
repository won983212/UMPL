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
        const double ERROR = 0.000001;
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
            else if(realnumber < ERROR)
            {
                Initialize(0, 1);
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
        
        public static Fraction Sqrt(Fraction n)
        {
            return new Fraction(Math.Sqrt(n.GetValue()));
        }

        public static void CheckIfBothInteger(Fraction p1, Fraction p2)
        {
            if (!(p1.IsInteger() && p2.IsInteger()))
                throw new ExprCoreException("두 수 " + p1 + ", " + p2 + "가 정수가 아닙니다.");
        }
    }
}
