using ExprCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Vector : TokenType
    {
        protected TokenType[] vecData;

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
                if (vecData[i] is Number == false)
                {
                    return false;
                }
            }
            return true;
        }

        protected static void CheckNumber(Vector v1)
        {
            if (!v1.IsNumberElements())
                throw new ExprCoreException("벡터가 상수가 아닙니다.");
        }

        protected static void CheckNumber(Vector v1, Vector v2)
        {
            if (!(v1.IsNumberElements() && v2.IsNumberElements()))
                throw new ExprCoreException("벡터가 상수가 아닙니다.");
        }

        public static Number Length(List<TokenType> parameters)
        {
            Vector v = parameters[0] as Vector;
            CheckNumber(v);

            if(v is Vec2 vec2)
                return Number.Sqrt(Number.Add(Number.Multiply(vec2.X, vec2.X), Number.Multiply(vec2.Y, vec2.Y)));
            if(v is Vec3 vec3)
                return Number.Sqrt(Number.Add(Number.Add(Number.Multiply(vec3.X, vec3.X), Number.Multiply(vec3.Y, vec3.Y)), Number.Multiply(vec3.Z, vec3.Z)));
            return null;
        }

        public static Vector Normalize(List<TokenType> parameters)
        {
            Vector v = parameters[0] as Vector;
            Number len = Length(parameters);
            if (v is Vec2 vec2)
                return new Vec2(Number.Divide(vec2.X, len), Number.Divide(vec2.Y, len));
            if (v is Vec3 vec3)
                return new Vec3(Number.Divide(vec3.X, len), Number.Divide(vec3.Y, len), Number.Divide(vec3.Z, len));
            return null;
        }
    }
}
