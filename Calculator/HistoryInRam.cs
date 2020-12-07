using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Calc.Interfaces;
using Calc.Models;

namespace Calculator
{
    class HistoryInRam : IHistory
    {
        public ObservableCollection<Expression> Values { get; }

        public HistoryInRam()
        {
            Values = new ObservableCollection<Expression>();
        }

        public void Add(Expression expression)
        {
            Values.Add(expression);
        }

        public void Delete(int index)
        {
            Values.RemoveAt(index);
        }

        public void Clear()
        {
            Values.Clear();
        }
    }
}
