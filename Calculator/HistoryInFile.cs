using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Calc.Interfaces;
using Calc.Models;
using Newtonsoft.Json;

namespace Calculator
{
    public class HistoryInFile : IHistory
    {
        public ObservableCollection<Expression> Values { get; }

        private string file;

        public HistoryInFile(string file)
        {
            this.file = file;
            if (File.Exists(file) == false)
            {
                Values = new ObservableCollection<Expression>();
                return;
            }

            string fileText = File.ReadAllText(file);
            Values = JsonConvert.DeserializeObject<ObservableCollection<Expression>>(fileText);

            Values = Values ?? new ObservableCollection<Expression>();
        }

        public void Add(Expression expression)
        {
            Values.Insert(0, expression);
            UpdateValueInFile();
        }

        public void Delete(int index)
        {
            Values.RemoveAt(index);
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
