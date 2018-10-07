using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class TokenType
    {
        public bool IsConstant { get; protected set; } = false;

        public virtual bool IsAcceptable(Type type)
        {
            return type.IsAssignableFrom(GetType());
        }

        public virtual TokenType Evaluate(Dictionary<Variable, Number> var_values) { return this; }
    }
}
