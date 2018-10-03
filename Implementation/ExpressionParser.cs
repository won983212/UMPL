using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    class ExpressionParser
    {
        public static void ParseExpression(TypeTree exprTree, string expr)
        {
            List<TokenType> tokens = Tokenize(expr);
        }

        private static void AppendParsedToken(List<TokenType> tokens, bool? isDigitStart, StringBuilder buffer)
        {
            if (isDigitStart == false)
            {
                string name = buffer.ToString();
                if (ConstantFactory.CanBeConstant(name))
                    tokens.Add(ConstantFactory.CreateConstant(name));
                else
                    tokens.Add(new Variable(name));
            }
            else if (isDigitStart == true)
                tokens.Add(new Number(Double.Parse(buffer.ToString())));

            if (isDigitStart != null)
                buffer.Clear();
        }

        private static List<TokenType> Tokenize(string expr)
        {
            List<TokenType> tokens = new List<TokenType>();
            StringBuilder buffer = new StringBuilder();
            bool? isDigitStart = null;

            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (Operator.IsOperatorCharacter(c))
                {
                    AppendParsedToken(tokens, isDigitStart, buffer);
                    isDigitStart = null;
                    tokens.Add(new Operator(c));
                }
                else if (Variable.IsVariableCharacter(c))
                {
                    if (isDigitStart == null)
                        isDigitStart = false;
                    else if (isDigitStart == true)
                        throw new ExprCoreException("이름은 숫자로 시작하면 안됩니다: " + buffer.ToString() + c + "...");
                    buffer.Append(c);
                }
                else if (IsNumberElement(c))
                {
                    if(isDigitStart == null)
                        isDigitStart = true;
                    buffer.Append(c);
                }
                else if (!IsAllowedSpecialCharacter(c))
                    throw new ExprCoreException("허용되지 않는 문자를 사용했습니다: " + c);
            }

            AppendParsedToken(tokens, isDigitStart, buffer);
            return tokens;
        }

        private static bool IsAllowedSpecialCharacter(char c)
        {
            return " ".IndexOf(c) >= 0;
        }

        private static bool IsNumberElement(char c)
        {
            return c >= '0' && c <= '9' || c == '.';
        }
    }
}
