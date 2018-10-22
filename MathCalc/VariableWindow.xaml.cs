using ExprCore;
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
using System.Windows.Shapes;

namespace MathCalc
{
    /// <summary>
    /// VariableWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VariableWindow : Window
    {
        private ObservableCollection<VariableElement> Items = new ObservableCollection<VariableElement>();
        private Dictionary<Variable, VariableElement> RenderElementTable = new Dictionary<Variable, VariableElement>();
        private List<ExprElement> liveExprs;

        public VariableWindow(List<ExprElement> liveExprs)
        {
            InitializeComponent();

            this.liveExprs = liveExprs;
            foreach(var ent in VariableManager.GetVariables())
            {
                VariableElement element = new VariableElement(ent.Key, ent.Value);
                Items.Add(element);
                RenderElementTable.Add(ent.Key, element);
            }

            VariableManager.OnVariableAdded += VariableManager_OnVariableAdded;
            VariableManager.OnVariableChanged += VariableManager_OnVariableChanged;
            CardListbox.ItemsSource = Items;
        }

        ~VariableWindow()
        {
            VariableManager.OnVariableAdded -= VariableManager_OnVariableAdded;
            VariableManager.OnVariableChanged -= VariableManager_OnVariableChanged;
        }

        private void VariableManager_OnVariableChanged(object sender, VariableEventArgs e)
        {
            if (RenderElementTable.ContainsKey(e.var))
            {
                RenderElementTable[e.var].VariableValue = e.value;
            }
        }

        private void VariableManager_OnVariableAdded(object sender, VariableEventArgs e)
        {
            VariableElement element = new VariableElement(e.var, e.value);
            Items.Add(element);
            RenderElementTable.Add(e.var, element);
        }

        private void Item_Delete(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.DataContext is VariableElement element)
            {
                element.MarkAsRemove(Items);
                RenderElementTable.Remove(element.Variable);
                VariableManager.DeleteVariable(element.Variable);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = sender as Slider;
            Label label = s.FindName("ContentValue") as Label;
            if (label != null && s.DataContext is VariableElement element)
            {
                label.Content = Math.Round(s.Value * 10) / 10;
                element.VariableValue = new Fraction(s.Value);
                VariableManager.SetVariable(element.Variable, element.VariableValue);
                foreach(ExprElement ent in liveExprs)
                {
                    ent.UpdateResult();
                }
            }
        }

        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            Slider s = sender as Slider;
            TextBox min = s.FindName("MinVal") as TextBox;
            TextBox max = s.FindName("MaxVal") as TextBox;
            if (min != null && max != null && s.DataContext is VariableElement element)
            {
                s.Value = ((Fraction)element.VariableValue).GetValue();
                min.Text = ((int)s.Value - 10).ToString();
                max.Text = ((int)s.Value + 10).ToString();
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            Slider s = expander.FindName("Slider") as Slider;
            TextBox min = s.FindName("MinVal") as TextBox;
            TextBox max = s.FindName("MaxVal") as TextBox;
            if (min != null && max != null && s.DataContext is VariableElement element)
            {
                s.Value = ((Fraction)element.VariableValue).GetValue();
                min.Text = ((int)s.Value - 10).ToString();
                max.Text = ((int)s.Value + 10).ToString();
            }
        }

        private void Expander_Loaded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            if(expander.DataContext is VariableElement element)
            {
                if(element.VariableValue is Fraction)
                {
                    expander.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
