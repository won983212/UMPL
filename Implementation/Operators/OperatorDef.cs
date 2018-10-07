using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class OperatorDef
    {
        public readonly Operator op;

        public OperatorDef(Operator oper)
        {
            op = oper;
        }

        public override bool Equals(object obj)
        {
            var def = obj as OperatorDef;
            return def != null && op.Equals(def.op);
        }

        public override int GetHashCode()
        {
            return op.GetHashCode();
        }

        public override string ToString()
        {
            return "Not Implemented Operator " + op;
        }
    }
}
