using ExprCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Variable : TokenType
    {
        public readonly string var_name;

        public Variable(string var_name)
        {
            this.var_name = var_name;
        }

        public override bool Equals(object obj)
        {
            var variable = obj as Variable;
            return variable != null && var_name == variable.var_name;
        }

        public override int GetHashCode()
        {
            return var_name.GetHashCode();
        }

        public override string ToString()
        {
            return var_name;
        }

        public override TokenType Evaluate(Dictionary<Variable, Fraction> var_values)
        {
            if (var_values.ContainsKey(this))
            {
                return new Fraction(var_values[this]);
            }
            else throw new ExprCoreException("변수 " + var_name + "의 값을 찾을 수 없습니다.");
        }

        public static bool IsVariableCharacter(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_';
        }
    }
}
