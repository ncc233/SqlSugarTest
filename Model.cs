﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace SqlSugarTest
{
    [SugarTable(IsDisabledDelete =true)]
    public class Model
    {
        [SugarColumn(IsPrimaryKey =true)]
        public int Id { get; set; }
        [SugarColumn(OldColumnName = "MyName", ColumnName ="NccName")]
        public string MyName { get; set; }
    }
}
