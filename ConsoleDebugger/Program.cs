using ExprCore;
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
                /*foreach (TokenType t in ExpressionParser.Tokenize("7-(a2+pi*(2-x)/7)*y-2+4/2"))
                {
                    Console.WriteLine(t);
                }*/
            }
            catch (ExprCoreException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
