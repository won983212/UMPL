using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Constant : TokenType
    {
        public readonly string label;
        public readonly double value;

        public Constant(Constant inst) : this(inst.label, inst.value) { }

        public Constant(string label, double value)
        {
            this.label = label;
            this.value = value;
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
    }
}
