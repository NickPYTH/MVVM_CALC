using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Calc.Interfaces;

namespace Calculator
{
    public class MemoryInRam : IMemory
    {
        public ObservableCollection<double> Values { get; }

        public MemoryInRam()
        {
            Values = new ObservableCollection<double>();
        }

        public void Add(double value)
        {
            Values.Add(value);
        }

        public void Delete(int index)
        {
            Values.RemoveAt(index);
        }

        public void Increase(double value, int index)
        {
            Values[index] += value;
        }

        public void Decrease(double value, int index)
        {
            Values[index] -= value;
        }

        public void Clear()
        {
            Values.Clear();
        }
    }
}
