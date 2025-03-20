using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PropertyChanged;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarTest
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private Model selectData;
        public SqlSugarClient db;
        public ObservableCollection<Model> ShowData { get; set; } = new ObservableCollection<Model>();
        [ObservableProperty]
        private string idName = string.Empty;
        public MainWindowViewModel()
        {
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=127.0.0.1;database=Test;uid=root;pwd=root;port=3306;Persist Security Info=True;Charset=utf8;Allow User Variables=True",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            db.CodeFirst.InitTables<Model>();
            db.CodeFirst.As<Model>("model233").InitTables<Model>();
            //GenerateData();
        }

        private void GenerateData()
        {
            var cellCollection = File.ReadAllText("TextCellCollection.txt").Split(',');
            Enumerable.Range(1, 20).ToList().ForEach(i =>
            {
                db.Insertable(new Model() { Id = i, MyName = cellCollection[i] }).ExecuteCommand();
            });
        }
        [RelayCommand]
        private void QueryData()
        {
            ShowData.Clear();
            var qrs = db.Queryable<Model>().ToList();
            Enumerable.Range(0, qrs.Count()).ToList().ForEach(i =>
            {
                ShowData.Add(qrs[i]);
            });
        }
        [RelayCommand]
        private void DeleteData()
        {
            if (SelectData == null)
            {
                db.Deleteable<Model>().ExecuteCommand();
            }
            else
            {
                db.Deleteable<Model>().Where(it => it.MyName == SelectData.MyName).ExecuteCommand();
            }
            ShowData.Clear();
            QueryData();

        }
        [RelayCommand]
        private void UpdateData()
        {
            if (SelectData == null) return;
            db.Updateable<Model>().SetColumns(it => new Model() { MyName = IdName }).Where(it => it.Id == SelectData.Id).ExecuteCommand();
            ShowData.Clear();
            QueryData();
        }
        [RelayCommand]
        private void addData()
        {
            if (string.IsNullOrEmpty(IdName)) return;
            var max = db.Queryable<Model>().Max(it => it.Id);
            db.Insertable(new Model() { Id = max + 1, MyName = IdName }).ExecuteCommand();
            QueryData();
        }
    }
}
