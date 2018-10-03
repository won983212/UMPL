using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class OperatorDef
    {
        public readonly Type return_type;
        public readonly Operator op;

        public OperatorDef(Type ret, Operator oper)
        {
            return_type = ret;
            op = oper;
        }

        public override bool Equals(object obj)
        {
            var def = obj as OperatorDef;
            return def != null &&
                   EqualityComparer<Type>.Default.Equals(return_type, def.return_type) &&
                   op.Equals(def.op);
        }

        public override int GetHashCode()
        {
            var hashCode = -982856206;
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(return_type);
            hashCode = hashCode * -1521134295 + op.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return "Not Implemented Operator (" + return_type.Name + ") " + op;
        }
    }
}
