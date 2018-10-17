using ExprCore.Exceptions;
using ExprCore.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Function : TokenType
    {
        public readonly string funcName;
        public readonly List<TokenType> parameters;

        public Function(string funcName, List<TokenType> parameters)
        {
            FunctionRegistry.CheckFunctionParamCount(funcName, parameters);
            this.funcName = funcName;
            this.parameters = new List<TokenType>(parameters);

            IsConstant = true;
            foreach(TokenType t in parameters)
            {
                if (!t.IsConstant)
                {
                    IsConstant = false;
                    break;
                }
            }
        }

        public override TokenType Evaluate(Dictionary<Variable, TokenType> var_values)
        {
            List<TokenType> evaluated = new List<TokenType>();
            foreach(TokenType t in parameters)
            {
                evaluated.Add(t.Evaluate(var_values));
            }

            return FunctionRegistry.ExecuteFunction(funcName, evaluated);
        }

        public override bool Equals(object obj)
        {
            Function function = obj as Function;
            return function != null && funcName == function.funcName;
        }

        public override int GetHashCode()
        {
            return funcName.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(funcName);
            int i = 0;

            sb.Append('(');
            foreach(TokenType t in parameters)
            {
                sb.Append(t);
                if(++i < parameters.Count)
                    sb.Append(", ");
            }
            sb.Append(')');

            return sb.ToString();
        }
    }
}
