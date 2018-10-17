using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Operators
{
    class MatrixOperators
    {
        private static Matrix Sum(TokenType left, TokenType right, bool negative)
        {
            Matrix l = left as Matrix;
            Matrix r = right as Matrix;
            Matrix ret = Matrix.CreateUnsafeMatrix(l.rows, l.columns);

            if (l.rows != r.rows || l.columns != r.columns)
                throw new ExprCoreException("행과 열이 다른 행렬은 더할 수 없습니다.");

            for (int i = 0; i < l.rows; i++)
            {
                for (int j = 0; j < l.columns; j++)
                {
                    TokenType lParam = l.data[i, j];
                    TokenType rParam = r.data[i, j];
                    if (lParam is Fraction && rParam is Fraction)
                        ret.data[i, j] = negative ? FractionOperators.Subtract(lParam, rParam) : FractionOperators.Add(lParam, rParam);
                    else
                        throw new ExprCoreException("상수인 행렬만 더할 수 있습니다.");
                }
            }

            return ret;
        }

        public static Matrix Add(TokenType left, TokenType right)
        {
            return Sum(left, right, false);
        }

        public static Matrix Subtract(TokenType left, TokenType right)
        {
            return Sum(left, right, true);
        }

        public static Matrix Multiply(TokenType left, TokenType right)
        {
            Matrix l = left as Matrix;
            Matrix r = right as Matrix;
            Matrix ret = Matrix.CreateUnsafeMatrix(l.rows, r.columns);

            if (l.columns != r.rows)
                throw new ExprCoreException("왼쪽 행렬의 열의 개수와 오른쪽 행렬의 행의 개수가 다릅니다.");

            for (int i = 0; i < l.rows; i++)
            {
                for (int j = 0; j < r.columns; j++)
                {
                    Fraction sum = 0;
                    for (int k = 0; k < l.columns; k++)
                    {
                        TokenType lParam = l.data[i, k];
                        TokenType rParam = r.data[k, j];
                        if (lParam is Fraction && rParam is Fraction)
                            sum = FractionOperators.Add(sum, FractionOperators.Multiply(lParam, rParam));
                        else
                            throw new ExprCoreException("상수인 행렬만 곱할 수 있습니다.");
                    }
                    ret.data[i, j] = sum;
                }
            }

            return ret;
        }

        public static Matrix Scala(TokenType left, TokenType right)
        {
            Fraction l = left as Fraction;
            Matrix r = right as Matrix;
            Matrix ret = Matrix.CreateUnsafeMatrix(r.rows, r.columns);

            for (int i = 0; i < r.rows; i++)
            {
                for (int j = 0; j < r.columns; j++)
                {
                    if (r.data[i, j] is Fraction)
                        ret.data[i, j] = FractionOperators.Multiply(l, r.data[i, j]);
                    else
                        throw new ExprCoreException("상수인 행렬만 스칼라 곱연산을 할 수 있습니다.");
                }
            }

            return ret;
        }

        public static Matrix Negative(TokenType operand)
        {
            Matrix mat = operand as Matrix;
            Matrix.CheckNumbericMatrix(mat);

            Matrix ret = Matrix.CreateUnsafeMatrix(mat.rows, mat.columns);
            for (int i = 0; i < ret.rows; i++)
            {
                for (int j = 0; j < ret.columns; j++)
                {
                    ret.data[i, j] = FractionOperators.Negative(mat.data[i, j]);
                }
            }

            return ret;
        }
    }
}
