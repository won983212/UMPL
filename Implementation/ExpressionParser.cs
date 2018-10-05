using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    class ExpressionParser
    {
        public static TypeTree ParseAsTree(string expr)
        {
            List<TokenType> postfix_expr = ConvertToPostfix(expr);
            Stack<TokenType> stack = new Stack<TokenType>();
            Node root;
            
            foreach (TokenType t in postfix_expr)
            {
                if(t is Operator)
                {
                    if (stack.Count <= 2)
                        throw new ExprCoreException("식이 잘못되었습니다.");

                    TokenType t2 = stack.Pop();
                    TokenType t1 = stack.Pop();
                }
                else
                {
                    stack.Push(t);
                }
            }
        }

        public static List<TokenType> ConvertToPostfix(string expr)
        {
            List<TokenType> tokens = Tokenize(expr);
            List<TokenType> postfix = new List<TokenType>();
            Stack<Operator> opstack = new Stack<Operator>();

            foreach (TokenType t in tokens)
            {
                Operator oper = t as Operator;
                if (oper != null)
                {
                    if (oper.op == '(')
                    {
                        opstack.Push(oper);
                    }
                    else if (oper.op == ')')
                    {
                        while (opstack.Count > 0)
                        {
                            Operator poped = opstack.Pop();
                            if (poped.op == '(')
                                break;
                            else postfix.Add(poped);
                        }
                    }
                    else
                    {
                        while (opstack.Count > 0)
                        {
                            if (opstack.Peek().priority >= oper.priority)
                            {
                                postfix.Add(opstack.Pop());
                            }
                            else break;
                        }
                        opstack.Push(oper);
                    }
                }
                else
                {
                    postfix.Add(t);
                }
            }

            while (opstack.Count > 0)
                postfix.Add(opstack.Pop());

            return postfix;
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
