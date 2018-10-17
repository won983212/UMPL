using ExprCore.Exceptions;
using ExprCore.Operators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class UnaryOperatorWrapper : TokenType
    {
        public Operator operation;
        public List<TokenType> temp_tokens;
        public TokenType token;
        public int depth;

        public UnaryOperatorWrapper(Operator op) : this(0, op)
        { }

        public UnaryOperatorWrapper(int defDepth, Operator op)
        {
            depth = defDepth;
            operation = op;
            temp_tokens = new List<TokenType>();
        }

        public TokenType Wrap()
        {
            if (temp_tokens.Count == 1)
                token = temp_tokens[0];
            else if (temp_tokens.Count == 0)
                throw new ExprCoreException("단항연산자 " + operation + "뒤에 피연산자가 없습니다.");
            else
                token = ExpressionParser.ParseExpression(temp_tokens);

            if (operation.op == '-' && token is Fraction frac)
                return new Fraction(-frac.numerator, frac.denomiator);

            return this;
        }

        public override TokenType Evaluate(Dictionary<Variable, Fraction> var_values)
        {
            return OperatorRegistry.ExecuteUnaryOperation(operation, token.Evaluate(var_values));
        }

        public override bool IsAcceptable(Type type)
        {
            return token.IsAcceptable(type);
        }

        public override string ToString()
        {
            if(token is Expression)
                return operation.op + "(" + token.ToString() + ")";
            return operation.op + token.ToString();
        }

        public override bool Equals(object obj)
        {
            var wrapper = obj as UnaryOperatorWrapper;
            return wrapper != null && operation.Equals(wrapper.operation) && token.Equals(wrapper.token);
        }

        public override int GetHashCode()
        {
            var hashCode = 1599181352;
            hashCode = hashCode * -1521134295 + EqualityComparer<Operator>.Default.GetHashCode(operation);
            hashCode = hashCode * -1521134295 + EqualityComparer<TokenType>.Default.GetHashCode(token);
            return hashCode;
        }
    }
}
