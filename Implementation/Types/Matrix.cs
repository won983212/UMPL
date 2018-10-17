using ExprCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExprCore.Types
{
    class Matrix : TokenType
    {
        public TokenType[,] data;
        public int rows;
        public int columns;

        private Matrix(int row, int column)
        {
            data = new TokenType[row, column];
            rows = row;
            columns = column;
            IsConstant = true;
        }

        private Matrix(Matrix src)
        {
            data = new TokenType[src.rows, src.columns];
            Array.Copy(src.data, data, src.data.Length);

            rows = src.rows;
            columns = src.columns;
            IsConstant = src.IsConstant;
        }

        public Matrix(int row, int column, List<TokenType> sequenceData) : this(row, column)
        {
            if (rows == 0 || columns == 0)
                throw new ExprCoreException("행 또는 열의 개수가 0이 될 수 없습니다. ");
            if (sequenceData.Count != rows * columns)
                throw new ExprCoreException("Matrix의 데이터의 개수가 행*열개가 아닙니다: " + sequenceData.Count + " != " + rows * columns);

            int k = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    data[i, j] = sequenceData[k++];
                    if (!data[i, j].IsConstant)
                    {
                        IsConstant = false;
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Mat");
            sb.Append(rows);
            sb.Append('_');
            sb.Append(columns);
            sb.Append('(');

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i > 0 || j > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(data[i, j].ToString());
                }
            }

            sb.Append(')');
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            var matrix = obj as Matrix;
            if (matrix != null)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (!data[i, j].Equals(matrix.data[i, j]))
                            return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1768953197;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    hashCode = hashCode * 34572 + data[i, j].GetHashCode();
                }
            }

            return hashCode;
        }

        public override TokenType Evaluate(Dictionary<Variable, Fraction> var_values)
        {
            List<TokenType> datas = new List<TokenType>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    datas.Add(data[i, j].Evaluate(var_values));
                }
            }
            return new Matrix(rows, columns, datas);
        }

        // Operators
        private static Matrix Sum(TokenType left, TokenType right, bool negative)
        {
            Matrix l = left as Matrix;
            Matrix r = right as Matrix;
            Matrix ret = new Matrix(l.rows, l.columns);

            if (l.rows != r.rows || l.columns != r.columns)
                throw new ExprCoreException("행과 열이 다른 행렬은 더할 수 없습니다.");

            for (int i = 0; i < l.rows; i++)
            {
                for (int j = 0; j < l.columns; j++)
                {
                    TokenType lParam = l.data[i, j];
                    TokenType rParam = r.data[i, j];
                    if (lParam is Fraction && rParam is Fraction)
                        ret.data[i, j] = negative ? Fraction.Subtract(lParam, rParam) : Fraction.Add(lParam, rParam);
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
            Matrix ret = new Matrix(l.rows, r.columns);

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
                            sum = Fraction.Add(sum, Fraction.Multiply(lParam, rParam));
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
            Matrix ret = new Matrix(r.rows, r.columns);

            for (int i = 0; i < r.rows; i++)
            {
                for (int j = 0; j < r.columns; j++)
                {
                    if (r.data[i, j] is Fraction)
                        ret.data[i, j] = Fraction.Multiply(l, r.data[i, j]);
                    else
                        throw new ExprCoreException("상수인 행렬만 스칼라 곱연산을 할 수 있습니다.");
                }
            }

            return ret;
        }

        // Functions
        private static void CheckNumbericMatrix(Matrix m)
        {
            if (!m.IsConstant)
                throw new ExprCoreException("상수 Matrix에 대해서만 가능한 연산입니다.");
        }

        private static void CheckSquareMatrix(Matrix m)
        {
            if (m.rows != m.columns)
                throw new ExprCoreException("정방행렬이 아닙니다.");
        }

        private static void ScalaAdd(Matrix m, int src, int dst, Fraction s)
        {
            for (int i = 0; i < m.columns; i++)
                m.data[dst, i] = Fraction.Add(m.data[dst, i], Fraction.Multiply(s, m.data[src, i]));
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
                m.data[r, i] = Fraction.Multiply(m.data[r, i], s);
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
                        if (val.GetValue() != 0) ScalaAdd(ret, r, j, Fraction.Negative(Fraction.Divide(val, src)));
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
            CheckNumbericMatrix(m);
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
                        if (val.GetValue() != 0) ScalaAdd(ret, r, j, Fraction.Negative(Fraction.Divide(val, src)));
                    }
                }

                r--; c--;
            }

            return ret;
        }

        public static Matrix GaussTop(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            CheckNumbericMatrix(m);
            return GaussTop(m, true);
        }

        public static Fraction Det(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            CheckNumbericMatrix(m);
            CheckSquareMatrix(m);
            m = GaussBot(m, true, true);

            Fraction det = 1;
            for (int i = 0; i < m.rows; i++)
                det = Fraction.Multiply(det, m.data[i, i]);

            return det;
        }

        public static Matrix Transpose(List<TokenType> parameters)
        {
            Matrix m = parameters[0] as Matrix;
            Matrix ret = new Matrix(m.rows, m.columns);
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
            CheckNumbericMatrix(m);

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
            CheckNumbericMatrix(m);
            CheckSquareMatrix(m);
            
            Matrix mat = new Matrix(m.rows, m.columns * 2);
            for(int r = 0; r < m.rows; r++)
            {
                for(int c = 0; c < m.columns; c++)
                    mat.data[r, c] = m.data[r, c];
                for (int c = m.columns; c < m.columns * 2; c++)
                    if(c == r + m.columns)
                        mat.data[r, c] = new Fraction(1);
                    else
                        mat.data[r, c] = new Fraction(0);
            }

            GaussBot(mat, false, false);

            Fraction det = 1;
            for (int i = 0; i < mat.rows; i++)
                det = Fraction.Multiply(det, mat.data[i, i]);

            if (det.GetValue() == 0)
                throw new ExprCoreException("행렬식이 0인 행렬은 inverse를 구할 수 없습니다.");

            for (int i = mat.rows - 1; i >= 0; i--)
            {
                for(int j = i - 1; j >= 0; j--)
                {
                    ScalaAdd(mat, i, j, Fraction.Negative(Fraction.Divide(mat.data[j, i], mat.data[i, i])));
                }
                ScalaRow(mat, i, Fraction.Divide(new Fraction(1), mat.data[i, i]));
            }

            Matrix ret = new Matrix(m.rows, m.columns);
            for (int r = 0; r < m.rows; r++)
                for (int c = 0; c < m.columns; c++)
                    ret.data[r, c] = mat.data[r, m.rows + c];

            return ret;
        }
    }
}
