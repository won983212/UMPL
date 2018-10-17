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
    public partial class MainWindow : Window
    {
        private ObservableCollection<ExprElement> cards = new ObservableCollection<ExprElement>();

        public MainWindow()
        {
            InitializeComponent();
            CardListbox.ItemsSource = cards;
            InputTextBox.Focus();
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

        private void OnTitleBarDrag(object sender, MouseButtonEventArgs e)
        {
            MainWnd.DragMove();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            MainWnd.Close();
        }

        private void OnConfigOpen(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = FindResource("ConfigMenu") as ContextMenu;
            menu.PlacementTarget = sender as Button;
            menu.IsOpen = true;
        }

        private void Item_Delete(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if(bt.DataContext is ExprElement element)
            {
                element.MarkAsRemove(cards);
            }
        }

        private void Item_ReExecute(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.DataContext is ExprElement element)
            {
                cards.Add(new ExprElement(element.PrevInput));
                ScrollViewer.ScrollToEnd();
            }
        }

        private void Item_LiveExecuteMode(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.DataContext is ExprElement element)
            {
                if(element.InputType == "Error")
                {
                    MessageBox.Show("입력에 오류있는 식을 대상으로 실시간 제출기능을 사용할 수 없습니다.", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    element.LiveExecute = !element.LiveExecute;
                    if (element.LiveExecute)
                    {
                        element.BorderBrush = (Brush)FindResource("VariableColor");
                    }
                    else
                    {
                        if(element.OutputType == "Error")
                            element.BorderBrush = (Brush)FindResource("ErrorColor");
                        else
                            element.BorderBrush = (Brush)FindResource("ThemeColor");
                    }
                }
            }
        }

        private void Allclear_Cards(object sender, RoutedEventArgs e)
        {
            cards.Clear();
        }
    }
}
