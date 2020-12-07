using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Interfaces
{
    public interface IMemory
    {
        ObservableCollection<double> Values { get; }
        void Add(double value);
        void Delete(int index);
        void Increase(double value, int index);
        void Decrease(double value, int index);
        void Clear();
    }
}
