using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Calc.Interfaces;
using Calc.Models;
using Dapper;

namespace Calculator
{
    public class HistoryDatabase : IHistory
    {
        public ObservableCollection<Expression> Values { get; }
        private string nameDB;

        public HistoryDatabase(string name)
        {
            nameDB = name;
            Values = new ObservableCollection<Expression>();
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                var expressions = connection.Query<Expression>("select * from Expressions");
                if (expressions.Any())
                {
                    foreach (var expression in expressions)
                    {
                        Values.Add(expression);
                    }
                }
            }
        }

        public void Add(Expression expression)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText =
                    @"Insert into Expressions(Action, MathExpression, Result, ErrorMessage, HasError, Steps) 
                    Values(@Action, @MathExpression, @Result, @ErrorMessage, @HasError, @Steps)";
                command.Parameters.AddWithValue("@Action", expression.Action);
                command.Parameters.AddWithValue("@MathExpression", expression.MathExpression);
                command.Parameters.AddWithValue("@Result", expression.Result);
                command.Parameters.AddWithValue("@ErrorMessage", expression.ErrorMessage);
                command.Parameters.AddWithValue("@HasError", expression.HasError);
                command.Parameters.AddWithValue("@Steps", expression.Steps);

                command.ExecuteNonQuery();
                Values.Add(expression);
            }
        }

        public void Delete(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Expressions Where Id = {index + 1}");
                Values.RemoveAt(index);
            }
        }

        public void Clear()
        {
            using (var connection = new SQLiteConnection($"Data Source={nameDB};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Expressions");
                Values.Clear();
            }
        }
    }
}
