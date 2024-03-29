﻿using ExprCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCalc
{
    class DumpDataContext
    {
        public ObservableCollection<ExprElement> Items { get; set; } = new ObservableCollection<ExprElement>();

        public DumpDataContext()
        {
            Items.Add(new ExprElement("det(mat3_3(3,3,2,2,0,-1,1,-3,4))"));
        }
    }

    class DumpDataContextVariable
    {
        public ObservableCollection<VariableElement> Items { get; set; } = new ObservableCollection<VariableElement>();

        public DumpDataContextVariable()
        {
            Items.Add(new VariableElement(new ExprCore.Types.Variable("x"), ExpressionParser.ParseExpression("-2*a+sqrt(12+3)")));
        }
    }
}
