﻿using ExprCore;
using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDebugger
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 5+12*7*(1-(4-3*1)/6-5)+12*(1+3/(12-3*7)*12)-1
                Expression expr = ExpressionParser.ParseExpression("gaussbot(mat4_5(2,3,1,0-11,1,5,0-2,5,0-4,5,1,0-1,3,0-3,3,3,4,0-7,2,0-7))");
                Console.WriteLine(expr.Evaluate());
            }
            catch (ExprCoreException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
