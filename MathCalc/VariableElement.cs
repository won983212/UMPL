using ExprCore.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCalc
{
    class VariableElement : RemovableElement<VariableElement>
    {
        private TokenType _varvalue = null;
        public TokenType VariableValue
        {
            get { return _varvalue; }
            set
            {
                _varvalue = value;
                OnPropertyChanged("VariableValue");
            }
        }
        public string VariableName
        {
            get { return Variable.var_name; }
        }
        public Variable Variable { get; private set; }

        public VariableElement(Variable var, TokenType value)
        {
            Variable = var;
            VariableValue = value;
        }
    }
}
