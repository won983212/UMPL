using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Exceptions
{
    class ProcessingException : Exception
    {
        public ProcessingException()
        { }

        public ProcessingException(string message) : base(message)
        { }
    }
}
