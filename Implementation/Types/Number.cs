using ExprCore.Exceptions;
using ExprCore.Operators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Number : TokenType
    {
        const double ERROR = 0.0000001;
        public long numerator;
        public long denomiator;

        public Number(long number) : this(number, 1) { }

        public Number(double realnumber)
        {
            if((realnumber - (long) realnumber) == 0)
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

        public Number(long numerator, long denomiator)
        {
            Initialize(numerator, denomiator);
        }

        private void Initialize(long n, long d)
        {
            if (n > 0 && d < 0)
            {
                numerator = d;
                denomiator = n;
            }
            else if (n < 0 && d < 0)
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

        public Number Reduce()
        {
            long k = MathHelper.GCD(numerator, denomiator);
            numerator /= k;
            denomiator /= k;
            return this;
        }

        public override bool Equals(object obj)
        {
            var number = obj as Number;
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

        // Operators
        public static TokenType Add(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            long lcm = MathHelper.LCM(l.denomiator, r.denomiator);
            return new Number(l.numerator * lcm / l.denomiator + r.numerator * lcm / r.denomiator, lcm).Reduce();
        }

        public static TokenType Subtract(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            long lcm = MathHelper.LCM(l.denomiator, r.denomiator);
            return new Number(l.numerator * lcm / l.denomiator - r.numerator * lcm / r.denomiator, lcm).Reduce();
        }

        public static TokenType Multiply(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            return new Number(l.numerator * r.numerator, l.denomiator * r.denomiator).Reduce();
        }

        public static TokenType Divide(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            return new Number(l.numerator * r.denomiator, l.denomiator * r.numerator).Reduce();
        }

        public static TokenType Power(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            return new Number(Math.Pow(l.GetValue(), r.GetValue())).Reduce();
        }

        public static TokenType Mod(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            return new Number(l.GetValue() % r.GetValue()).Reduce();
        }

        public static TokenType Negative(TokenType operand)
        {
            Number n = operand as Number;
            return new Number(-n.numerator, n.denomiator);
        }

        static Number()
        {
            OperatorRegistry.RegisterBinary(typeof(Number), typeof(Number), new Operator('+'), typeof(Number), Add);
            OperatorRegistry.RegisterBinary(typeof(Number), typeof(Number), new Operator('-'), typeof(Number), Subtract);
            OperatorRegistry.RegisterBinary(typeof(Number), typeof(Number), new Operator('*'), typeof(Number), Multiply);
            OperatorRegistry.RegisterBinary(typeof(Number), typeof(Number), new Operator('/'), typeof(Number), Divide);
            OperatorRegistry.RegisterBinary(typeof(Number), typeof(Number), new Operator('^'), typeof(Number), Power);
            OperatorRegistry.RegisterBinary(typeof(Number), typeof(Number), new Operator('%'), typeof(Number), Mod);
            OperatorRegistry.RegisterUnary(typeof(Number), new Operator('-'), typeof(Number), Negative);
        }
    }
}
