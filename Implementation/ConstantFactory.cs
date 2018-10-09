using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore
{
    class ConstantFactory
    {
        private static Dictionary<string, Constant> constants = new Dictionary<string, Constant>();

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
            constants.Add("pi", new Constant("π", new Number(Math.PI)));
            constants.Add("e", new Constant("e", new Number(Math.E)));
        }
    }
}
