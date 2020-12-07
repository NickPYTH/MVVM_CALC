using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Calc.Interfaces;
using Newtonsoft.Json;

namespace Calculator
{
    public class MemoryInFile : IMemory
    {
        public ObservableCollection<double> Values { get; }
        private string file;

        public MemoryInFile(string file)
        {
            this.file = file;
            Values = new ObservableCollection<double>();
            if (File.Exists(file) == false)
                return;

            string fileText = File.ReadAllText(file);
            Values = JsonConvert.DeserializeObject<ObservableCollection<double>>(fileText);
            Values = Values ?? new ObservableCollection<double>();
        }

        public void Add(double value)
        {
            Values.Insert(0, value);
            UpdateValueInFile();
        }

        public void Delete(int index)
        {
            Values.RemoveAt(index);
            UpdateValueInFile();
        }

        public void Increase(double value, int index)
        {
            Values[index] += value;
            UpdateValueInFile();
        }

        public void Decrease(double value, int index)
        {
            Values[index] -= value;
            UpdateValueInFile();
        }

        public void Clear()
        {
            Values.Clear();
            UpdateValueInFile();
        }

        private void UpdateValueInFile()
        {
            var textToOutput = JsonConvert.SerializeObject(Values);
            File.WriteAllText(file, textToOutput);
        }
    }
}
