using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCalc
{
    class VariableElement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TokenType VariableValue { get; private set; }
        public string VariableName { get; private set; }

        public VariableElement(string varName, TokenType value)
        {
            VariableName = varName;
            VariableValue = value;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
