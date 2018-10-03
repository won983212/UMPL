using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class UnaryOperatorDef : OperatorDef
    {
        public readonly Type operand_type;

        public UnaryOperatorDef(Type ret, Operator oper, Type operand_type) : base(ret, oper)
        {
            this.operand_type = operand_type;
        }

        public override bool Equals(object obj)
        {
            var def = obj as UnaryOperatorDef;
            return def != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Type>.Default.Equals(operand_type, def.operand_type);
        }

        public override int GetHashCode()
        {
            var hashCode = -987730453;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(operand_type);
            return hashCode;
        }

        public override string ToString()
        {
            return "(" + return_type.Name + ") " + op + operand_type.Name;
        }
    }
}
