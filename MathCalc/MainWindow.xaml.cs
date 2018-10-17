using ExprCore;
using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathCalc
{
    class ExprElement
    {
        public string PrevInput { get; private set; }
        public string InputType { get; private set; }
        public string OutputType { get; private set; }
        public string TimeToExecute { get; private set; }
        public string ExecutionTime { get; private set; }
        public TokenType InputExpr { get; set; }
        public TokenType Result { get; set; }

        public ExprElement(string text)
        {
            PrevInput = text;
            TimeToExecute = DateTime.Now.ToString("tt hh:mm:ss");
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
                InputType = "Error";
                OutputType = "Error";
            }

            if (successIn)
            {
                try
                {
                    Result = InputExpr.Evaluate(new Dictionary<Variable, Fraction>());
                    OutputType = GetPrettyType(Result);
                }
                catch (ExprCoreException e)
                {
                    Result = new ErrorTextTokenType(e.Message);
                    OutputType = "Error";
                }
            }

            stopwatch.Stop();

            long time = stopwatch.ElapsedMilliseconds;
            ExecutionTime = time == 0 ? "매우 짧음" : (time + "ms");
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
            return "알 수 없는 토큰";
        }
    }

    public partial class MainWindow : Window
    {
        private ObservableCollection<ExprElement> cards = new ObservableCollection<ExprElement>();

        public MainWindow()
        {
            InitializeComponent();
            CardListbox.ItemsSource = cards;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cards.Add(new ExprElement(InputTextBox.Text));
                InputTextBox.Text = "";
                ScrollViewer.ScrollToEnd();
            }
        }

        private void CardListbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                ExprElement element = item.DataContext as ExprElement;
                InputTextBox.Text = element.PrevInput;
                InputTextBox.CaretIndex = element.PrevInput.Length;
                InputTextBox.ScrollToEnd();
            }
        }
    }
}
