using ExprCore;
using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace MathCalc
{
    public class ExprElement : RemovableElement<ExprElement>
    {
        public bool LiveExecute { get; set; } = false;
        public string PrevInput { get; private set; }
        public string InputType { get; private set; }
        public string OutputType { get; private set; }
        public string TimeToExecute { get; private set; }
        public string ExecutionTime { get; private set; }
        public TokenType InputExpr { get; private set; }

        private TokenType result;
        public TokenType Result
        {
            get
            {
                return result;
            }
            private set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }

        private Brush _borderBrush;
        public Brush BorderBrush
        {
            get
            {
                return _borderBrush;
            }
            set
            {
                _borderBrush = value;
                OnPropertyChanged("BorderBrush");
            }
        }

        public ExprElement(string text)
        {
            PrevInput = text;
            TimeToExecute = DateTime.Now.ToString("tt hh:mm:ss");
            BorderBrush = (Brush) App.Current.FindResource("ThemeColor");
            bool successIn = false;

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                InputExpr = ExpressionParser.ParseExpression(text);
                InputType = GetPrettyType(InputExpr);
                successIn = true;
            }
            catch (ExprCoreException e)
            {
                InputExpr = new ErrorTextTokenType(e.Message);
                Result = new ErrorTextTokenType("입력값에 오류가 있습니다.");
                BorderBrush = (Brush)App.Current.FindResource("ErrorColor");
                InputType = "Error";
                OutputType = "Error";
            }

            if (successIn)
            {
                UpdateResult();
            }

            stopwatch.Stop();

            long time = stopwatch.ElapsedMilliseconds;
            ExecutionTime = time == 0 ? "매우 짧음" : (time + "ms");
        }

        public void UpdateResult()
        {
            try
            {
                Result = VariableManager.EvaluateWithVariable(InputExpr);
                OutputType = GetPrettyType(Result);
            }
            catch (ExprCoreException e)
            {
                Result = new ErrorTextTokenType(e.Message);
                BorderBrush = (Brush)App.Current.FindResource("ErrorColor");
                OutputType = "Error";
            }
        }

        private static string GetPrettyType(TokenType type)
        {
            if (type is Fraction)
                return "수";
            if (type is ExprCore.Types.Expression)
                return "식";
            if (type is ExprCore.Types.Matrix)
                return "행렬";
            if (type is Constant)
                return "상수";
            if (type is Function)
                return "함수";
            if (type is Operator)
                return "연산자";
            if (type is Variable)
                return "변수";
            if (type is Vec2)
                return "2차원 벡터";
            if (type is Vec3)
                return "3차원 벡터";
            if (type is UnaryOperatorWrapper)
                return "단항연산";
            return "알 수 없는 토큰";
        }
    }
}
