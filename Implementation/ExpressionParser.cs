using ExprCore.Exceptions;
using ExprCore.Operators;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    class ExpressionParser
    {
        public static TokenType ParseExpression(string expr)
        {
            if (expr.Length == 0)
                throw new ExprCoreException("식이 비어있습니다.");

            return ParseExpression(Tokenize(expr));
        }

        public static TokenType ParseExpression(List<TokenType> tokenizedTokens)
        {
            if (tokenizedTokens.Count == 0)
                throw new ExprCoreException("식이 비어있습니다.");

            List<TokenType> postfix_expr = ConvertToPostfix(WrapUnaryOperators(tokenizedTokens));
            Stack<Node> stack = new Stack<Node>();
            bool isConst = true;

            if (postfix_expr.Count == 1)
                return postfix_expr[0];

            foreach (TokenType t in postfix_expr)
            {
                if (t is Operator)
                {
                    if (stack.Count < 2)
                        throw new ExprCoreException("식이 잘못되었습니다.");

                    Node t2 = stack.Pop();
                    Node t1 = stack.Pop();
                    Node nNode = new Node(t, t1, t2);
                    stack.Push(nNode);
                }
                else
                {
                    if (!t.IsConstant)
                        isConst = false;
                    stack.Push(new Node(t));
                }
            }

            if (stack.Count != 1)
                throw new ExprCoreException("식이 잘못되었습니다.");

            return new Expression(new TypeTree(tokenizedTokens, stack.Pop()), isConst);
        }

        // unary operator 앞엔 아무것도 없거나 || operator만 존재가능함
        // 바로 뒤의 element와 붙는다. 다만 괄호가 있다면 괄호안의 elements들과 붙는다.
        private static List<TokenType> WrapUnaryOperators(List<TokenType> tokens)
        {
            TokenType prev = null;
            Stack<UnaryOperatorWrapper> stack = new Stack<UnaryOperatorWrapper>();
            UnaryOperatorWrapper unary;

            stack.Push(new UnaryOperatorWrapper(-1, null));
            foreach (TokenType t in tokens)
            {
                unary = stack.Peek();

                if (t is Operator oper)
                {
                    if (Operator.IsUnaryOperatorCharacter(oper.op) && (prev == null || prev is Operator prevOp && prevOp.op != ')'))
                    {
                        stack.Push(new UnaryOperatorWrapper(oper));
                        prev = t;
                        continue;
                    }

                    if (stack.Count > 1)
                    {
                        if (oper.op == '(')
                        {
                            unary.depth++;
                        }
                        else if (oper.op == ')')
                        {
                            if (unary.depth > 0)
                                unary.depth--;
                        }
                    }
                }

                unary.temp_tokens.Add(t);
                if (unary.depth == 0)
                {
                    unary = stack.Pop();
                    stack.Peek().temp_tokens.Add(unary.Wrap());
                }

                prev = t;
            }

            while(stack.Count > 1)
            {
                unary = stack.Pop();
                stack.Peek().temp_tokens.Add(unary.Wrap());
            }

            return stack.Pop().temp_tokens;
        }

        private static List<TokenType> ConvertToPostfix(List<TokenType> tokenizedTokens)
        {
            List<TokenType> postfix = new List<TokenType>();
            Stack<Operator> opstack = new Stack<Operator>();

            foreach (TokenType t in tokenizedTokens)
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

        private static void FlushTokenBuffer(List<TokenType> parameterBuffer, List<TokenType> tokenBuffer)
        {
            if (tokenBuffer.Count == 1)
                parameterBuffer.Add(tokenBuffer[0]);
            else
                parameterBuffer.Add(ParseExpression(ParseComplex(tokenBuffer)));
        }

        // Parse Function, Matrix, Vector... etc
        private static List<TokenType> ParseComplex(List<TokenType> semiTokenizedTypes)
        {
            List<TokenType> list = new List<TokenType>();
            List<TokenType> parameterBuffer = new List<TokenType>();
            List<TokenType> tokenBuffer = new List<TokenType>();
            Variable markedVar = null;
            string funcName = null;
            int funcDepth = -1;
            int bracketDepth = 0;

            foreach (TokenType t in semiTokenizedTypes)
            {
                if (t is Variable)
                {
                    if (funcName != null)
                        tokenBuffer.Add(t);
                    else markedVar = (Variable)t;
                    continue;
                }
                else if (t is Operator)
                {
                    Operator oper = t as Operator;

                    if (oper.op == '{' || oper.op == '[')
                        oper = new Operator('(');
                    if (oper.op == '}' || oper.op == ']')
                        oper = new Operator(')');

                    if (oper.op == '(')
                    {
                        bracketDepth++;
                        if (markedVar != null)
                        {
                            funcName = markedVar.var_name;
                            funcDepth = bracketDepth;
                            markedVar = null;
                            continue;
                        }
                    }
                    else if (oper.op == ')')
                    {
                        if (funcName != null && funcDepth == bracketDepth)
                        {
                            FlushTokenBuffer(parameterBuffer, tokenBuffer);

                            if (funcName.ToLower() == "vec")
                            {
                                if (parameterBuffer.Count == 2)
                                    list.Add(new Vec2(parameterBuffer));
                                else if (parameterBuffer.Count == 3)
                                    list.Add(new Vec3(parameterBuffer));
                                else
                                    list.Add(new Function(funcName, parameterBuffer));
                            }
                            else if (funcName.ToLower().StartsWith("mat"))
                            {
                                string[] sizeText = funcName.Substring(3).Split('_');
                                int rows = 0, columns = 0;

                                if (sizeText.Length != 2)
                                    throw new ExprCoreException("Matrix의 크기를 입력하지 않았습니다: " + funcName);
                                if (!int.TryParse(sizeText[0], out rows))
                                    throw new ExprCoreException("Matrix의 '행'이 정수가 아닙니다: " + sizeText[0]);
                                if (!int.TryParse(sizeText[1], out columns))
                                    throw new ExprCoreException("Matrix의 '열'이 정수가 아닙니다: " + sizeText[1]);

                                list.Add(new Matrix(rows, columns, parameterBuffer));
                            }
                            else list.Add(new Function(funcName, parameterBuffer));

                            parameterBuffer.Clear();
                            tokenBuffer.Clear();
                            funcName = null;
                            bracketDepth--;
                            continue;
                        }
                        bracketDepth--;
                    }
                    else if (oper.op == ',')
                    {
                        if (funcName != null && funcDepth == bracketDepth)
                        {
                            FlushTokenBuffer(parameterBuffer, tokenBuffer);
                            tokenBuffer.Clear();
                            continue;
                        }
                    }
                }

                if (markedVar != null && t != markedVar)
                {
                    list.Add(markedVar);
                    markedVar = null;
                }

                if (funcName != null)
                    tokenBuffer.Add(t);
                else list.Add(t);
            }

            if (markedVar != null)
                list.Add(markedVar);

            if (bracketDepth > 0)
                throw new ExprCoreException("괄호가 완전히 닫히지 않았습니다.");
            else if (bracketDepth < 0)
                throw new ExprCoreException("열린 괄호보다 닫힌 괄호가 더 많습니다.");

            return list;
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
                tokens.Add(new Fraction(double.Parse(buffer.ToString())));

            if (isDigitStart != null)
                buffer.Clear();
        }

        // Parse Operator, Number, Variable, Constant
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
                    if (isDigitStart == null)
                        isDigitStart = true;
                    buffer.Append(c);
                }
                else if (!IsAllowedSpecialCharacter(c))
                    throw new ExprCoreException("허용되지 않는 문자를 사용했습니다: " + c);
            }

            AppendParsedToken(tokens, isDigitStart, buffer);
            tokens = ParseComplex(tokens);
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
