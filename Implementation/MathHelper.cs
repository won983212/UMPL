using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    static class MathHelper
    {
        public static long GCD(long x, long y)
        {
            long temp;
            while(y != 0)
            {
                temp = x;
                x = y;
                y = temp % y;
            }

            return x;
        }

        public static long LCM(long x, long y)
        {
            return x * y / GCD(x, y);
        }
    }
}
