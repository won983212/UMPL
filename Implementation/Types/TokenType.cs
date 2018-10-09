using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class TokenType
    {
        // 해당 타입에 상수만 포함되어있는가?
        public bool IsConstant { get; protected set; } = false;

        // 해당 타입에 대입할 수 있는가?
        public virtual bool IsAcceptable(Type type)
        {
            return type.IsAssignableFrom(GetType());
        }

        // 해당 타입을 주어진 변수에 대하여 풀었을 때의 값
        public virtual TokenType Evaluate(Dictionary<Variable, Number> var_values)
        {
            return this;
        }
    }
}
