using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Constant : TokenType
    {
        public readonly string label;
        public readonly TokenType value;

        public Constant(Constant inst) : this(inst.label, inst.value) { }

        public Constant(string label, TokenType value)
        {
            this.label = label;
            this.value = value;
            IsConstant = true;
        }

        public override bool Equals(object obj)
        {
            var constant = obj as Constant;
            return constant != null && label == constant.label;
        }

        public override int GetHashCode()
        {
            return label.GetHashCode();
        }

        public override string ToString()
        {
            return label;
        }

        public override TokenType Evaluate(Dictionary<Variable, TokenType> var_values)
        {
            return value.Evaluate(var_values);
        }

        public override bool IsAcceptable(Type type)
        {
            return value.IsAcceptable(type);
        }
    }
}
