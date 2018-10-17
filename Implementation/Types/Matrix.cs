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

        public Matrix(Matrix src)
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

        public override TokenType Evaluate(Dictionary<Variable, TokenType> var_values)
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

        public static Matrix CreateUnsafeMatrix(int rows, int columns)
        {
            return new Matrix(rows, columns);
        }

        public static void CheckNumbericMatrix(Matrix m)
        {
            if (!m.IsConstant)
                throw new ExprCoreException("상수 Matrix가 아닙니다.");
        }

        public static void CheckSquareMatrix(Matrix m)
        {
            if (m.rows != m.columns)
                throw new ExprCoreException("정방행렬이 아닙니다.");
        }
    }
}
