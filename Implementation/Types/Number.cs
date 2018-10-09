﻿using ExprCore.Exceptions;
using ExprCore.Functions;
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

        public Number(Number number) : this(number.numerator, number.denomiator) { }

        public Number(double realnumber)
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

        public Number(long numerator, long denomiator)
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

        public Number Reduce()
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

        public override bool IsAcceptable(Type type)
        {
            if (type == typeof(Constant))
                return true;
            return base.IsAcceptable(type);
        }

        public static implicit operator Number(double d)
        {
            return new Number(d);
        }

        // Operators
        public static Number Add(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            long lcm = MathHelper.LCM(l.denomiator, r.denomiator);
            return new Number(l.numerator * lcm / l.denomiator + r.numerator * lcm / r.denomiator, lcm).Reduce();
        }

        public static Number Subtract(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            long lcm = MathHelper.LCM(l.denomiator, r.denomiator);
            return new Number(l.numerator * lcm / l.denomiator - r.numerator * lcm / r.denomiator, lcm).Reduce();
        }

        public static Number Multiply(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            return new Number(l.numerator * r.numerator, l.denomiator * r.denomiator).Reduce();
        }

        public static Number Divide(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            return new Number(l.numerator * r.denomiator, l.denomiator * r.numerator).Reduce();
        }

        public static Number Power(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            return new Number(Math.Pow(l.GetValue(), r.GetValue())).Reduce();
        }

        public static Number Mod(TokenType left, TokenType right)
        {
            Number l = left as Number;
            Number r = right as Number;
            return new Number(l.GetValue() % r.GetValue()).Reduce();
        }

        public static Number Negative(TokenType operand)
        {
            Number n = operand as Number;
            return new Number(-n.numerator, n.denomiator);
        }

        // Functions
        private static void CheckIfBothInteger(Number p1, Number p2)
        {
            if (!(p1.IsInteger() && p2.IsInteger()))
                throw new ExprCoreException("정수에 대해서만 GCD연산이 가능합니다.");
        }

        public static Number Gcd(List<TokenType> parameters)
        {
            Number p1 = parameters[0] as Number;
            Number p2 = parameters[1] as Number;
            CheckIfBothInteger(p1, p2);
            return new Number(MathHelper.GCD((long) p1.GetValue(), (long)p2.GetValue()));
        }

        public static Number Lcm(List<TokenType> parameters)
        {
            Number p1 = parameters[0] as Number;
            Number p2 = parameters[1] as Number;
            CheckIfBothInteger(p1, p2);
            return new Number(MathHelper.LCM((long)p1.GetValue(), (long)p2.GetValue()));
        }

        public static Number Reduce(List<TokenType> parameters)
        {
            Number p1 = parameters[0] as Number;
            return new Number(p1).Reduce();
        }

        // Untility
        public static Number Sqrt(Number n)
        {
            return new Number(Math.Sqrt(n.GetValue()));
        }
    }
}
