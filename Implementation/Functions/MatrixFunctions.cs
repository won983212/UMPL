using ExprCore.Exceptions;
using ExprCore.Operators;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Functions
{
    class MatrixFunctions
    {
        private static void ScalaAdd(Matrix m, int src, int dst, Fraction s)
        {
            for (int i = 0; i < m.columns; i++)
                m.data[dst, i] = FractionOperators.Add(m.data[dst, i], FractionOperators.Multiply(s, m.data[src, i]));
        }

        private static void Switching(Matrix m, int r1, int r2)
        {
            for (int i = 0; i < m.columns; i++)
            {
                TokenType temp = m.data[r1, i];
                m.data[r1, i] = m.data[r2, i];
                m.data[r2, i] = temp;
            }
        }

        private static void ScalaRow(Matrix m, int r, Fraction s)
        {
            for (int i = 0; i < m.columns; i++)
                m.data[r, i] = FractionOperators.Multiply(m.data[r, i], s);
        }

        public static Matrix Gauss(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            m = GaussBot(m, true, false);
            return GaussTop(m, true);
        }

        // copy = 행렬을 copy후 계산할것인가?
        // allowMulNeg = Switching이 홀수번 이루어지면 자동으로 첫 행에 * -1
        public static Matrix GaussBot(Matrix m, bool copy, bool allowMulNeg)
        {
            bool found = false, neg = false;
            int r = 0, c = 0;
            Matrix ret = copy ? new Matrix(m) : m;
            while (r < m.rows && c < m.columns)
            {
                for (int i = r; i < m.rows; i++)
                {
                    if (((Fraction)m.data[i, c]).GetValue() != 0)
                    {
                        if (i != r) Switching(ret, i, r);
                        found = true;
                        neg = !neg;
                        break;
                    }
                }

                if (!found)
                {
                    c++;
                    continue;
                }
                else
                {
                    Fraction src = ret.data[r, c] as Fraction;
                    for (int j = r + 1; j < m.rows; j++)
                    {
                        Fraction val = ret.data[j, c] as Fraction;
                        if (val.GetValue() != 0) ScalaAdd(ret, r, j, FractionOperators.Negative(FractionOperators.Divide(val, src)));
                    }
                }

                r++; c++;
            }

            if (neg && allowMulNeg)
                ScalaRow(ret, 0, -1);

            return ret;
        }

        public static Matrix GaussBot(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            Matrix.CheckNumbericMatrix(m);
            return GaussBot(m, true, false);
        }

        public static Matrix GaussTop(Matrix m, bool copy)
        {
            int r = m.rows - 1, c = m.columns - 1;
            Matrix ret = copy ? new Matrix(m) : m;
            while (r >= 0 && c >= 0)
            {
                int swcnt = 0;
                for (int i = r; i >= 0; i--)
                {
                    if (((Fraction)ret.data[i, c]).GetValue() != 0)
                    {
                        if (i != r) Switching(ret, i, r);
                        swcnt++;
                        break;
                    }
                }

                if (swcnt == 0)
                {
                    c--;
                    continue;
                }
                else
                {
                    Fraction src = ret.data[r, c] as Fraction;
                    for (int j = r - 1; j >= 0; j--)
                    {
                        Fraction val = ret.data[j, c] as Fraction;
                        if (val.GetValue() != 0) ScalaAdd(ret, r, j, FractionOperators.Negative(FractionOperators.Divide(val, src)));
                    }
                }

                r--; c--;
            }

            return ret;
        }

        public static Matrix GaussTop(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            Matrix.CheckNumbericMatrix(m);
            return GaussTop(m, true);
        }

        public static Fraction Det(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            Matrix.CheckNumbericMatrix(m);
            Matrix.CheckSquareMatrix(m);
            m = GaussBot(m, true, true);

            Fraction det = 1;
            for (int i = 0; i < m.rows; i++)
                det = FractionOperators.Multiply(det, m.data[i, i]);

            return det;
        }

        public static Matrix Transpose(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            Matrix ret = Matrix.CreateUnsafeMatrix(m.rows, m.columns);
            for (int i = 0; i < m.rows; i++)
            {
                for (int j = 0; j < m.columns; j++)
                {
                    ret.data[i, j] = m.data[j, i];
                }
            }

            return ret;
        }

        public static Fraction Rank(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            Matrix.CheckNumbericMatrix(m);

            int rank = 0;
            for (int r = 0; r < m.rows; r++)
            {
                int c;
                for (c = 0; c < m.columns; c++)
                {
                    if (((Fraction)m.data[r, c]).GetValue() != 0)
                        break;
                }
                if (c != m.columns)
                    rank++;
            }

            return rank;
        }

        public static Matrix Inverse(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            Matrix.CheckNumbericMatrix(m);
            Matrix.CheckSquareMatrix(m);

            Matrix mat = Matrix.CreateUnsafeMatrix(m.rows, m.columns * 2);
            for (int r = 0; r < m.rows; r++)
            {
                for (int c = 0; c < m.columns; c++)
                    mat.data[r, c] = m.data[r, c];
                for (int c = m.columns; c < m.columns * 2; c++)
                    if (c == r + m.columns)
                        mat.data[r, c] = new Fraction(1);
                    else
                        mat.data[r, c] = new Fraction(0);
            }

            GaussBot(mat, false, false);

            Fraction det = 1;
            for (int i = 0; i < mat.rows; i++)
                det = FractionOperators.Multiply(det, mat.data[i, i]);

            if (det.GetValue() == 0)
                throw new ExprCoreException("행렬식이 0인 행렬은 inverse를 구할 수 없습니다.");

            for (int i = mat.rows - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    ScalaAdd(mat, i, j, FractionOperators.Negative(FractionOperators.Divide(mat.data[j, i], mat.data[i, i])));
                }
                ScalaRow(mat, i, FractionOperators.Divide(new Fraction(1), mat.data[i, i]));
            }

            Matrix ret = Matrix.CreateUnsafeMatrix(m.rows, m.columns);
            for (int r = 0; r < m.rows; r++)
                for (int c = 0; c < m.columns; c++)
                    ret.data[r, c] = mat.data[r, m.rows + c];

            return ret;
        }
    }
}
