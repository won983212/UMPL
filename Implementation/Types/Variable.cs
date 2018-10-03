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
            return EqualityComparer<string>.Default.GetHashCode(var_name);
        }

        public override string ToString()
        {
            return var_name;
        }

        public static bool IsVariableCharacter(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '_';
        }
    }
}
