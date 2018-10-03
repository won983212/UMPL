using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Exceptions
{
    class ExprCoreException : Exception
    {
        public ExprCoreException()
        { }

        public ExprCoreException(string message) : base(message)
        { }
    }
}
