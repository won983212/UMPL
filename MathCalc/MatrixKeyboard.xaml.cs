using ExprCore;
using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MathCalc
{
    /// <summary>
    /// MatrixKeyboard.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MatrixKeyboard : Window
    {
        private TextBox textbox;

        public MatrixKeyboard(TextBox textbox)
        {
            this.textbox = textbox;
            InitializeComponent();
            ResizeMatrix(3, 3);
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            int rows = 1, columns = 1;

            if (!(int.TryParse(RowsTextbox.Text, out rows) && int.TryParse(ColumnsTextbox.Text, out columns)))
            {
                MessageBox.Show("행과 열은 정수로 입력해야합니다.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ResizeMatrix(rows, columns);
        }

        private void ResizeMatrix(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
                MessageBox.Show("행과 열의 크기는 0보다 커야합니다.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Error);

            TextGrid.Rows = rows;
            TextGrid.Columns = columns;
            TextGrid.Children.Clear();

            for (int i = 0; i < rows * columns; i++)
            {
                TextBox element = new TextBox();
                element.Style = (Style)FindResource("InTextboxStyle");
                element.TextChanged += Matrix_TextChanged;
                element.Name = "Box" + (i / rows) + "_" + (i % rows);
                element.PreviewKeyDown += Element_PreviewKeyDown;
                TextGrid.Children.Add(element);
            }
        }

        private void Element_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            string[] rc = box.Name.Substring(3).Split('_');
            int r = int.Parse(rc[0]);
            int c = int.Parse(rc[1]);

            if (e.Key == Key.Up && r > 0) r--;
            else if (e.Key == Key.Down && r + 1 < TextGrid.Rows) r++;
            else if (e.Key == Key.Left && c > 0 && box.CaretIndex == 0) c--;
            else if (e.Key == Key.Right && c + 1 < TextGrid.Columns && box.CaretIndex == box.Text.Length) c++;

            TextBox nextBox = TextGrid.Children[TextGrid.Rows * r + c] as TextBox;
            if (nextBox != null) nextBox.Focus();
        }

        private void Matrix_TextChanged(object sender, TextChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder("mat");
            sb.Append(TextGrid.Rows);
            sb.Append("_");
            sb.Append(TextGrid.Columns);
            sb.Append('(');

            for (int i = 0; i < TextGrid.Children.Count; i++)
            {
                TextBox box = TextGrid.Children[i] as TextBox;
                if (box.Text.Length == 0)
                    return;
                else
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(box.Text);
                }
            }

            sb.Append(')');
            Preview.Text = sb.ToString();
        }

        private void Preview_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Preview.Text;
            if (text.Length >= 8 && text.Substring(0, 3).ToLower() == "mat")
            {
                int startBracket = text.IndexOf('(');
                if (startBracket <= 5) return;

                int endBracket = text.LastIndexOf(')');
                if (endBracket <= 6) return;

                string[] sizeString = text.Substring(3, startBracket - 3).Split('_');
                if (sizeString.Length != 2) return;

                int r = -1, c = -1;
                if (!(int.TryParse(sizeString[0], out r) && int.TryParse(sizeString[1], out c))) return;

                int i = 0;
                string[] elements = text.Substring(startBracket + 1, endBracket - startBracket - 1).Split(',');
                if (elements.Length != r * c) return;

                if (r != TextGrid.Rows || c != TextGrid.Columns) ResizeMatrix(r, c);
                foreach (object obj in TextGrid.Children)
                {
                    TextBox box = obj as TextBox;
                    box.Text = elements[i++];
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (object obj in TextGrid.Children)
            {
                TextBox box = obj as TextBox;
                box.Text = "";
            }
        }

        private void AppendButton_Click(object sender, RoutedEventArgs e)
        {
            if (textbox != null)
            {
                textbox.Text = textbox.Text.Insert(textbox.CaretIndex, Preview.Text);
            }
        }
    }
}
