using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    class ConstantFactory
    {
        private static Dictionary<string, double> constants = new Dictionary<string, double>();

        public static bool CanBeConstant(string key)
        {
            return constants.ContainsKey(key);
        }

        public static Constant CreateConstant(string key)
        {
            if (CanBeConstant(key))
            {
                return new Constant(constants[key]);
            }
            else throw new ExprCoreException("해당 상수를 찾을 수 없습니다: " + key);
        }

        static ConstantFactory()
        {
            constants.Add("pi", Math.PI);
            constants.Add("e", Math.E);
        }
    }
}
