using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    class VariableEventArgs
    {
        public readonly Variable var;
        public readonly TokenType value;

        public VariableEventArgs(Variable var, TokenType value)
        {
            this.var = var;
            this.value = value;
        }
    }

    static class VariableManager
    {

        public static event EventHandler<VariableEventArgs> OnVariableAdded;
        public static event EventHandler<VariableEventArgs> OnVariableChanged;
        private static readonly List<Variable> variableOrders = new List<Variable>();
        private static readonly Dictionary<Variable, TokenType> variables = new Dictionary<Variable, TokenType>();

        public static Dictionary<Variable, TokenType> GetVariables()
        {
            return variables;
        }

        public static void AddNewVariable(Variable var, TokenType value)
        {
            variableOrders.Add(new Variable(var.var_name));
            variables.Add(var, value);
            OnVariableAdded?.Invoke(null, new VariableEventArgs(var, value));
        }

        public static bool HasVariable(Variable var)
        {
            return variables.ContainsKey(var);
        }

        public static void SetVariable(Variable var, TokenType value)
        {
            variables[var] = value;
            OnVariableChanged?.Invoke(null, new VariableEventArgs(var, value));
        }

        public static void DeleteVariable(Variable var)
        {
            variableOrders.Remove(var);
            variables.Remove(var);
        }

        public static TokenType EvaluateWithVariable(TokenType toEval)
        {
            Dictionary<Variable, TokenType> ret = new Dictionary<Variable, TokenType>();
            foreach (Variable ent in variableOrders)
            {
                TokenType type = variables[ent].Evaluate(ret);
                if (!type.IsConstant)
                    throw new ExprCoreException("변수 " + ent + "이(가) 상수가 아닙니다.");
                ret.Add(ent, type);
            }

            return toEval.Evaluate(ret);
        }

        public static TokenType Institute(TokenType left, TokenType right)
        {
            Variable var = left as Variable;
            if (HasVariable(var))
                SetVariable(var, right);
            else
                AddNewVariable(var, right);

            return right;
        }
    }
}
