using ExprCore.Exceptions;
using ExprCore.Operators;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Functions
{
    class VectorFunctions
    {
        public static Fraction Length(List<TokenType> parameters)
        {
            Vector v = parameters[0] as Vector;
            Vector.CheckNumberic(v);

            if (v is Vec2 vec2)
                return Fraction.Sqrt(FractionOperators.Add(FractionOperators.Multiply(vec2.X, vec2.X), FractionOperators.Multiply(vec2.Y, vec2.Y)));
            if (v is Vec3 vec3)
                return Fraction.Sqrt(FractionOperators.Add(FractionOperators.Add(FractionOperators.Multiply(vec3.X, vec3.X), FractionOperators.Multiply(vec3.Y, vec3.Y)), FractionOperators.Multiply(vec3.Z, vec3.Z)));
            return null;
        }

        public static Vector Normalize(List<TokenType> parameters)
        {
            Vector v = parameters[0] as Vector;
            Vector.CheckNumberic(v);

            Fraction len = Length(parameters);
            if (v is Vec2 vec2)
                return new Vec2(FractionOperators.Divide(vec2.X, len), FractionOperators.Divide(vec2.Y, len));
            if (v is Vec3 vec3)
                return new Vec3(FractionOperators.Divide(vec3.X, len), FractionOperators.Divide(vec3.Y, len), FractionOperators.Divide(vec3.Z, len));
            return null;
        }
    }
}
