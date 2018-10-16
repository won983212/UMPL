using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCalc
{
    class ErrorTextTokenType : TokenType
    {
        public string ErrorMessage;

        public ErrorTextTokenType(string error)
        {
            ErrorMessage = error;
        }
    }
}
