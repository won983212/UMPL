using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Functions
{
    delegate TokenType FuncImpl(List<TokenType> parameters);

    class FuncRegistryValue
    {
        public readonly FuncImpl body;
        public readonly Type[] parameterTypes;

        public FuncRegistryValue(FuncImpl body, Type[] parameterTypes)
        {
            this.body = body;
            this.parameterTypes = parameterTypes;
        }
    }

    static class FunctionRegistry
    {
        private static readonly Dictionary<string, FuncRegistryValue> functions = new Dictionary<string, FuncRegistryValue>();

        public static void RegisterFunction(string funcName, Type[] parameterTypes, FuncImpl body)
        {
            functions.Add(funcName.ToLower(), new FuncRegistryValue(body, parameterTypes));
        }

        private static string FuncToString(string funcName, List<TokenType> parameters)
        {
            List<Type> t = new List<Type>();
            foreach (TokenType type in parameters)
                t.Add(type.GetType());
            return FuncToString(funcName, t.ToArray());
        }

        private static string FuncToString(string funcName, Type[] types)
        {
            StringBuilder input = new StringBuilder(funcName);
            int j = 0;

            input.Append('(');
            foreach (Type type in types)
            {
                input.Append(type.Name);
                if (++j < types.Length)
                    input.Append(", ");
            }
            input.Append(')');

            return input.ToString();
        }

        private static FuncRegistryValue GetFunction(string funcName)
        {
            funcName = funcName.ToLower();
            if (functions.ContainsKey(funcName))
            {
                return functions[funcName];
            }

            throw new ExprCoreException("등록되어있지 않은 함수입니다: " + funcName);
        }

        public static void CheckFunctionParamCount(string funcName, List<TokenType> parameters)
        {
            Type[] types = GetFunction(funcName).parameterTypes;

            if (types.Length > parameters.Count)
                throw new ExprCoreException("함수의 매개변수가 너무 적습니다: " + FuncToString(funcName, parameters));
            if (types.Length < parameters.Count)
                throw new ExprCoreException("함수의 매개변수가 너무 많습니다: " + FuncToString(funcName, parameters));
        }

        public static void CheckFunctionType(string funcName, List<TokenType> parameters)
        {
            int i = 0;
            Type[] types = GetFunction(funcName).parameterTypes;

            foreach (Type t in types)
            {
                if (!parameters[i++].IsAcceptable(t))
                {
                    throw new ExprCoreException("입력한 함수 " + FuncToString(funcName, parameters) + "는 " + FuncToString(funcName, types) + "와 매개변수 타입이 맞지 않습니다.");
                }
            }
        }

        public static TokenType ExecuteFunction(string funcName, List<TokenType> parameters)
        {
            CheckFunctionType(funcName, parameters);
            return GetFunction(funcName).body(parameters);
        }

        static FunctionRegistry()
        {
            // Number
            RegisterFunction("gcd", new Type[] { typeof(Fraction), typeof(Fraction) }, Fraction.Gcd);
            RegisterFunction("lcm", new Type[] { typeof(Fraction), typeof(Fraction) }, Fraction.Lcm);
            RegisterFunction("reduce", new Type[] { typeof(Fraction) }, Fraction.Reduce);
            RegisterFunction("sqrt", new Type[] { typeof(Fraction) }, Fraction.Sqrt);
            RegisterFunction("abs", new Type[] { typeof(Fraction) }, Fraction.Abs);

            // Vector
            RegisterFunction("length", new Type[] { typeof(Vector) }, Vector.Length);
            RegisterFunction("norm", new Type[] { typeof(Vector) }, Vector.Normalize);

            // Matrix
            RegisterFunction("gaussbot", new Type[] { typeof(Matrix) }, Matrix.GaussBot);
            RegisterFunction("gausstop", new Type[] { typeof(Matrix) }, Matrix.GaussTop);
            RegisterFunction("gauss", new Type[] { typeof(Matrix) }, Matrix.Gauss);
            RegisterFunction("det", new Type[] { typeof(Matrix) }, Matrix.Det);
            RegisterFunction("trans", new Type[] { typeof(Matrix) }, Matrix.Transpose);
            RegisterFunction("rank", new Type[] { typeof(Matrix) }, Matrix.Rank);
            RegisterFunction("inv", new Type[] { typeof(Matrix) }, Matrix.Inverse);
        }
    }
}
