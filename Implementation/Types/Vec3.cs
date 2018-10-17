using ExprCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Vec3 : Vector
    {
        public TokenType X => vecData[0];
        public TokenType Y => vecData[1];
        public TokenType Z => vecData[2];

        public Vec3(List<TokenType> elements) : base(new TokenType[] { elements[0], elements[1], elements[2] })
        {
            if (elements.Count != 3)
                throw new ExprCoreException("Vec3는 인자가 3개 필요합니다.");
        }

        public Vec3(TokenType x, TokenType y, TokenType z) : base(new TokenType[] { x, y, z })
        { }

        public override string ToString()
        {
            return "Vec3(" + X + ", " + Y + ", " + Z + ")";
        }

        public override TokenType Evaluate(Dictionary<Variable, TokenType> var_values)
        {
            return new Vec3(X.Evaluate(var_values), Y.Evaluate(var_values), Z.Evaluate(var_values));
        }
    }
}
