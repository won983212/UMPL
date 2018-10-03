using ExprCore.Exceptions;
using ExprCore.Operators;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    static class OperatorRegistry
    {
        public delegate TokenType UnaryOperateFunc(TokenType operand);
        public delegate TokenType BinaryOperateFunc(TokenType left, TokenType right);

        private static Dictionary<UnaryOperatorDef, UnaryOperateFunc> unary_operators = new Dictionary<UnaryOperatorDef, UnaryOperateFunc>();
        private static Dictionary<BinaryOperatorDef, BinaryOperateFunc> binary_operators = new Dictionary<BinaryOperatorDef, BinaryOperateFunc>();

        public static void RegisterUnary(Type ret, Operator op, Type operandType, UnaryOperateFunc operation)
        {
            unary_operators.Add(new UnaryOperatorDef(ret, op, operandType), operation);
        }

        public static void RegisterBinary(Type ret, Type left, Operator op, Type right, BinaryOperateFunc operation)
        {
            binary_operators.Add(new BinaryOperatorDef(ret, left, op, right), operation);
        }

        public static TokenType ExecuteOperation(OperatorDef op, TokenType param1, TokenType param2)
        {
            if (op is UnaryOperatorDef)
            {
                UnaryOperatorDef def = op as UnaryOperatorDef;
                if (unary_operators.ContainsKey(def))
                {
                    return unary_operators[def](param1);
                }
            }
            else if(op is BinaryOperatorDef)
            {
                BinaryOperatorDef def = op as BinaryOperatorDef;
                if (binary_operators.ContainsKey(def))
                {
                    return binary_operators[def](param1, param2);
                }
            }

            throw new ExprCoreException("인식할 수 없는 연산자입니다. [" + op + "]");
        }
    }
}
