﻿using ExprCore.Exceptions;
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
        public readonly bool isCommutative;

        public OperationData(Type return_type, T func, bool isCommutative)
        {
            this.return_type = return_type;
            this.func = func;
            this.isCommutative = isCommutative;
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
            unary_operators.Add(new UnaryOperatorDef(op, operandType), new OperationData<UnaryOperateFunc>(ret, operation, false));
        }

        public static void RegisterBinary(Type ret, Type left, Operator op, Type right, BinaryOperateFunc operation)
        {
            binary_operators.Add(new BinaryOperatorDef(left, op, right), new OperationData<BinaryOperateFunc>(ret, operation, false));
        }

        public static void RegisterBinaryCommutative(Type ret, Type left, Operator op, Type right, BinaryOperateFunc operation)
        {
            binary_operators.Add(new BinaryOperatorDef(left, op, right), new OperationData<BinaryOperateFunc>(ret, operation, true));
        }

        public static TokenType ExecuteBinaryOperation(Operator op, TokenType param1, TokenType param2)
        {
            Type pType1 = param1.GetType();
            Type pType2 = param2.GetType();
            BinaryOperatorDef def = new BinaryOperatorDef(pType1, op, pType2);
            if (binary_operators.ContainsKey(def))
                return binary_operators[def].func(param1, param2);
            else
            {
                def = new BinaryOperatorDef(pType2, op, pType1);
                if (binary_operators.ContainsKey(def))
                {
                    OperationData<BinaryOperateFunc> data = binary_operators[def];
                    if (data.isCommutative)
                        return data.func(param2, param1);
                }
                else
                {
                    def = new BinaryOperatorDef(pType1, op, typeof(TokenType));
                    if (binary_operators.ContainsKey(def))
                        return binary_operators[def].func(param1, param2);
                }
            }

            throw new ExprCoreException("인식할 수 없는 연산자입니다. (" + pType1.Name + ") " + op + " (" + pType2.Name + ")");
        }

        public static TokenType ExecuteUnaryOperation(Operator op, TokenType param)
        {
            Type pType = param.GetType();
            UnaryOperatorDef def = new UnaryOperatorDef(op, pType);
            if (unary_operators.ContainsKey(def))
            {
                return unary_operators[def].func(param);
            }

            throw new ExprCoreException("인식할 수 없는 연산자입니다. " + op + "(" + pType.Name + ")");
        }

        static OperatorRegistry()
        {
            // Number
            RegisterBinary(typeof(Fraction), typeof(Fraction), new Operator('+'), typeof(Fraction), FractionOperators.Add);
            RegisterBinary(typeof(Fraction), typeof(Fraction), new Operator('-'), typeof(Fraction), FractionOperators.Subtract);
            RegisterBinary(typeof(Fraction), typeof(Fraction), new Operator('*'), typeof(Fraction), FractionOperators.Multiply);
            RegisterBinary(typeof(Fraction), typeof(Fraction), new Operator('/'), typeof(Fraction), FractionOperators.Divide);
            RegisterBinary(typeof(Fraction), typeof(Fraction), new Operator('^'), typeof(Fraction), FractionOperators.Power);
            RegisterBinary(typeof(Fraction), typeof(Fraction), new Operator('%'), typeof(Fraction), FractionOperators.Mod);
            RegisterUnary(typeof(Fraction), new Operator('-'), typeof(Fraction), FractionOperators.Negative);

            // Vec2
            RegisterBinary(typeof(Vec2), typeof(Vec2), new Operator('+'), typeof(Vec2), Vec2Operators.Add);
            RegisterBinary(typeof(Vec2), typeof(Vec2), new Operator('-'), typeof(Vec2), Vec2Operators.Subtract);
            RegisterBinaryCommutative(typeof(Vec2), typeof(Fraction), new Operator('*'), typeof(Vec2), Vec2Operators.Scala);
            RegisterBinary(typeof(Vec2), typeof(Vec2), new Operator('*'), typeof(Vec2), Vec2Operators.Dot);
            RegisterUnary(typeof(Vec2), new Operator('-'), typeof(Vec2), Vec2Operators.Negative);

            // Vec3
            RegisterBinary(typeof(Vec3), typeof(Vec3), new Operator('+'), typeof(Vec3), Vec3Operators.Add);
            RegisterBinary(typeof(Vec3), typeof(Vec3), new Operator('-'), typeof(Vec3), Vec3Operators.Subtract);
            RegisterBinaryCommutative(typeof(Vec3), typeof(Fraction), new Operator('*'), typeof(Vec3), Vec3Operators.Scala);
            RegisterBinary(typeof(Vec3), typeof(Vec3), new Operator('*'), typeof(Vec3), Vec3Operators.Dot);
            RegisterBinary(typeof(Vec3), typeof(Vec3), new Operator('@'), typeof(Vec3), Vec3Operators.Cross);
            RegisterUnary(typeof(Vec3), new Operator('-'), typeof(Vec3), Vec3Operators.Negative);

            // Matrix
            RegisterBinary(typeof(Matrix), typeof(Matrix), new Operator('+'), typeof(Matrix), MatrixOperators.Add);
            RegisterBinary(typeof(Matrix), typeof(Matrix), new Operator('-'), typeof(Matrix), MatrixOperators.Subtract);
            RegisterBinary(typeof(Matrix), typeof(Matrix), new Operator('*'), typeof(Matrix), MatrixOperators.Multiply);
            RegisterBinaryCommutative(typeof(Matrix), typeof(Fraction), new Operator('*'), typeof(Matrix), MatrixOperators.Scala);
            RegisterUnary(typeof(Matrix), new Operator('-'), typeof(Matrix), MatrixOperators.Negative);

            // Expression
            RegisterBinary(typeof(TokenType), typeof(Variable), new Operator('='), typeof(TokenType), VariableManager.Institute);
        }
    }
}
