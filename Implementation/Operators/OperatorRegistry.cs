using ExprCore.Exceptions;
using ExprCore.Operators;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class OperationData<T>
    {
        public readonly Type return_type;
        public readonly T func;

        public OperationData(Type return_type, T func)
        {
            this.return_type = return_type;
            this.func = func;
        }
    }

    static class OperatorRegistry
    {
        public delegate TokenType UnaryOperateFunc(TokenType operand);
        public delegate TokenType BinaryOperateFunc(TokenType left, TokenType right);

        private static Dictionary<UnaryOperatorDef, OperationData<UnaryOperateFunc>> unary_operators = new Dictionary<UnaryOperatorDef, OperationData<UnaryOperateFunc>>();
        private static Dictionary<BinaryOperatorDef, OperationData<BinaryOperateFunc>> binary_operators = new Dictionary<BinaryOperatorDef, OperationData<BinaryOperateFunc>>();

        public static void RegisterUnary(Type ret, Operator op, Type operandType, UnaryOperateFunc operation)
        {
            unary_operators.Add(new UnaryOperatorDef(op, operandType), new OperationData<UnaryOperateFunc>(ret, operation));
        }

        public static void RegisterBinary(Type ret, Type left, Operator op, Type right, BinaryOperateFunc operation)
        {
            binary_operators.Add(new BinaryOperatorDef(left, op, right), new OperationData<BinaryOperateFunc>(ret, operation));
        }

        public static TokenType ExecuteBinaryOperation(Operator op, TokenType param1, TokenType param2)
        {
            return ExecuteOperation(new BinaryOperatorDef(param1.GetType(), op, param2.GetType()), param1, param2);
        }

        public static TokenType ExecuteUnaryOperation(Operator op, TokenType param)
        {
            return ExecuteOperation(new UnaryOperatorDef(op, param.GetType()), param, null);
        }

        private static TokenType ExecuteOperation(OperatorDef op, TokenType param1, TokenType param2)
        {
            if (op is UnaryOperatorDef)
            {
                UnaryOperatorDef def = op as UnaryOperatorDef;
                if (unary_operators.ContainsKey(def))
                {
                    return unary_operators[def].func(param1);
                }
            }
            else if(op is BinaryOperatorDef)
            {
                BinaryOperatorDef def = op as BinaryOperatorDef;
                if (binary_operators.ContainsKey(def))
                {
                    return binary_operators[def].func(param1, param2);
                }
            }

            throw new ExprCoreException("인식할 수 없는 연산자입니다. [" + op + "]");
        }
    }
}
