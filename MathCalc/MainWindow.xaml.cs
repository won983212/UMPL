using ExprCore;
using ExprCore.Exceptions;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public TokenType InputExpr { get; set; }
        public TokenType Result { get; set; }

        public ExprElement(string text)
        {
            PrevInput = text;
            bool successIn = false;

            try
            {
                InputExpr = ExpressionParser.ParseExpression(text);
                successIn = true;
            }
            catch (ExprCoreException e)
            {
                InputExpr = new ErrorTextTokenType(e.Message);
                Result = new ErrorTextTokenType("입력값에 오류가 있습니다.");
            }

            if(successIn)
            {
                try
                {
                    Result = InputExpr.Evaluate(new Dictionary<Variable, Fraction>());
                }
                catch (ExprCoreException e)
                {
                    Result = new ErrorTextTokenType(e.Message);
                }
            }
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
            if(e.Key == Key.Enter)
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
