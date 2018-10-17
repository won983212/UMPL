using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Expression : TokenType
    {
        public TypeTree ExprTree { get; private set; }

        public Expression(TypeTree tree, bool isConstant)
        {
            ExprTree = tree;
            IsConstant = isConstant;
        }

        public TokenType Evaluate()
        {
            return Evaluate(new Dictionary<Variable, TokenType>());
        }

        public override TokenType Evaluate(Dictionary<Variable, TokenType> var_values)
        {
            return ExprTree.Evaluate(var_values);
        }

        public override string ToString()
        {
            return ExprTree.ToString();
        }
    }
}
