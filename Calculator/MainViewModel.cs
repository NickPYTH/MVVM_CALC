using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Calc.Commands;
using Calc.Interfaces;
using Calc.Models;
using Calculator.Annotations;

namespace Calculator
{
    public class MainViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        public IMemory Memory { get; set; }
        public IHistory OperationLog { get; set; }
        public ICalculate Calculator { get; set; }

        public MainViewModel()
        {
            Calculator = new Calculate();
            //OperationLog = new HistoryDatabase("database.db");
            //Memory = new MemoryDatabase("database.db");

            OperationLog = new HistoryInFile("Operations.json");
            Memory = new MemoryInFile("Memory.json");

            _validationDictionary = new Dictionary<string, string>();
            TextExp = "";
        }

        private ICommand _clearText;

        public ICommand ClearText
        {
            get => new RelayCommand(() =>
            {
                TextExp = "";
            }, () => true);
        }

        private ICommand _decreaseValueToItemMemory;

        public ICommand DecreaseValueToItemMemory
        {
            get => _decreaseValueToItemMemory ?? new RelayCommand<TextBox>(x =>
            {
                x.Text = (Convert.ToDouble(x.Text) - Calculator.Parse(new Expression(TextExp, Calculator)).Result).ToString();
            }, x => Calculator.Parse(new Expression(TextExp, Calculator)).HasError != true);
        }
        private ICommand _addValueToItemMemory;

        public ICommand AddValueToItemMemory
        {
            get => _addValueToItemMemory ?? new RelayCommand<TextBox>(x =>
            {
                x.Text = (Convert.ToDouble(x.Text) + Calculator.Parse(new Expression(TextExp, Calculator)).Result)
                    .ToString();
            }, x => Calculator.Parse(new Expression(TextExp, Calculator)).HasError != true);
        }

        private ICommand _deleteItemMemory;

        public ICommand DeleteItemMemory
        {
            get => _deleteItemMemory ?? new RelayCommand<TextBox>(x =>
            {
                Memory.Delete((int)x.Tag);
            }, x => true);
        }

        private ICommand _memoryTake;

        public ICommand MemoryTake
        {
            get => _memoryTake ?? new RelayCommand(() =>
            {
                TextExp = Memory.Values.First().ToString();
            }, () => Memory is null == false && Memory.Values.Count > 0);
        }

        private ICommand _memorySave;

        public ICommand MemorySave
        {
            get => _memorySave ?? new RelayCommand(() =>
            {
                Memory.Add(Calculator.Parse(new Expression(TextExp, Calculator)).Result);
            }, () => Calculator.Parse(new Expression(TextExp, Calculator)).HasError != true);
        }

        private ICommand _memoryClear;

        public ICommand MemoryClear
        {
            get => _memoryClear ?? new RelayCommand(() =>
            {
                Memory.Clear();
            }, () => Memory.Values.Count > 0);
        }

        private ICommand _computeValue;

        public ICommand ComputeValue
        {
            get => _computeValue ?? new RelayCommand(() =>
            {
                var res = Calculator.Parse(new Expression(TextExp, Calculator));
                TextExp = res.Result.ToString();
                OperationLog.Add(res);
            }, () => Calculator.Parse(new Expression(TextExp, Calculator)).HasError != true);
        }

        private ICommand _addNumericAndSymbols;

        public ICommand AddNumericAndSymbols
        {
            get => _addNumericAndSymbols ?? new RelayCommand<string>(x =>
            {
                TextExp += x;
            }, x => true);
        }
        private string _textExp;

        public string TextExp
        {
            get => _textExp;
            set
            {
                _textExp = value;
                if (string.IsNullOrWhiteSpace(value))
                    _validationDictionary[nameof(TextExp)] = "NoText";
                else if (new Expression(value, Calculator).HasError)
                    _validationDictionary[nameof(TextExp)] = "Error expression";
                else
                    _validationDictionary[nameof(TextExp)] = null;
                OnPropertyChanged(nameof(TextExp));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Dictionary<string, string> _validationDictionary;
        public string Error 
        { 
            get => _validationDictionary.Any(x => string.IsNullOrWhiteSpace(x.Value))
                ? string.Join(Environment.NewLine, _validationDictionary.Where(x => string.IsNullOrWhiteSpace(x.Value) == false).GetEnumerator().Current)
                : null;
        }

        public string this[string columnName] => _validationDictionary.ContainsKey(columnName) ? _validationDictionary[columnName] : null;
    }
}
