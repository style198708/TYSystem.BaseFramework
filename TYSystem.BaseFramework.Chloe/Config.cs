using System;
using System.Collections.Generic;
using System.Text;

namespace TYSystem.BaseFramework.Chloe
{
    public class Config
    {
        //public const string SqlServerConnectionString = "server=DESKTOP-6149F5J;uid=sa;pwd=cst-88888;database=SqlSugar4XTest";
        //public const string MySqlConnectionString = "";

        public List<TableConfiguration> TableConfigurations { get; set; }

       
    }

    public class TableConfiguration
    {
        public string Name { get; set; }
        public  string SqlType { get; set; }
        public bool WithLock { get; set; }
        public string Author { get; set; }
        public string CacheDependency { get; set; }

        public string ConnectString { get; set; }
    }


}
