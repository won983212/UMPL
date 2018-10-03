using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class BinaryOperatorDef : OperatorDef
    {
        public readonly Type left_type;
        public readonly Type right_type;

        public BinaryOperatorDef(Type ret, Type left_type, Operator oper, Type right_type) : base(ret, oper)
        {
            this.left_type = left_type;
            this.right_type = right_type;
        }

        public override bool Equals(object obj)
        {
            var def = obj as BinaryOperatorDef;
            return def != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Type>.Default.Equals(left_type, def.left_type) &&
                   EqualityComparer<Type>.Default.Equals(right_type, def.right_type);
        }

        public override int GetHashCode()
        {
            var hashCode = -214507765;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(left_type);
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(right_type);
            return hashCode;
        }

        public override string ToString()
        {
            return "(" + return_type.Name + ") " + left_type.Name + op + right_type.Name;
        }
    }
}
