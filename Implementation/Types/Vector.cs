using ExprCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Vector : TokenType
    {
        public readonly TokenType[] vecData;

        protected Vector(TokenType[] data)
        {
            vecData = data;
            IsConstant = true;

            for (int i = 0; i < data.Length; i++)
            {
                if (!data[i].IsConstant)
                {
                    IsConstant = false;
                    break;
                }
            }
        }

        public override string ToString()
        {
            return "(Unimplemented Vector)";
        }

        public override bool Equals(object obj)
        {
            var vector = obj as Vector;
            if(vector != null)
            {
                for (int i = 0; i < vecData.Length; i++)
                {
                    if (!vecData[i].Equals(vector.vecData[i]))
                        return false;
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 373119288;
            for (int i = 0; i < vecData.Length; i++)
            {
                hashCode = hashCode * -1521134295 + vecData[i].GetHashCode();
            }

            return hashCode;
        }

        public bool IsNumberElements()
        {
            for (int i = 0; i < vecData.Length; i++)
            {
                if (vecData[i] is Fraction == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static void CheckNumberic(Vector v1)
        {
            if (!v1.IsNumberElements())
                throw new ExprCoreException("벡터가 상수가 아닙니다.");
        }

        public static void CheckNumberic(Vector v1, Vector v2)
        {
            if (!(v1.IsNumberElements() && v2.IsNumberElements()))
                throw new ExprCoreException("벡터가 상수가 아닙니다.");
        }
    }
}
