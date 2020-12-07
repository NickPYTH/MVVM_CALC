using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Calc.Interfaces;
using Dapper;

namespace Calculator
{
    public class MemoryDatabase : IMemory
    {
        public ObservableCollection<double> Values { get; }
        public ObservableCollection<RowDB> Rows { get; }
        private string nameDB;

        public MemoryDatabase(string name)
        {
            nameDB = name;
            Values = new ObservableCollection<double>();
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                var rows = connection.Query<RowDB>("select * from Memory");
                if (rows.Any())
                {
                    foreach (var row in rows)
                    {
                        Values.Add(row.Value);
                           // Rows.Add(row);
                    }
                }
            }
        }

        public void Add(double value)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText =
                    @"Insert into Memory(Value) 
                    Values(@Value)";
                command.Parameters.AddWithValue("@Value", value);

                command.ExecuteNonQuery();
                Values.Add(value);
            }
        }

        public void Delete(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                var count = connection.Query($"Delete from Memory Where Id = {index + 1}");
                Values.RemoveAt(index);
            }
        }

        public void Increase(double value, int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText = $"Update Memory SET Value={Values[index] + value} Where Id={index + 1}";
                command.ExecuteNonQuery();
                Values[index] += value;
            }
        }

        public void Decrease(double value, int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText = $"Update Memory SET Value={Values[index] - value} Where Id={index + 1}";
                command.ExecuteNonQuery();
                Values[index] -= value;
            }
        }

        public void Clear()
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Memory");
                Values.Clear();
            }
        }
    }

    public class RowDB
    {
        public int Id { get; set; }
        public double Value { get; set; }
    }
}
