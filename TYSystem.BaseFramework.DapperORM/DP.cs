using TYSystem.BaseFramework.Dapper.Tool;
using TYSystem.BaseFramework.DapperExtensions;
using TYSystem.BaseFramework.DapperExtensions.Lambda;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TYSystem.BaseFramework.Dapper
{
    public class DP
    {
        public static List<TableConfiguration> config { get; set; }

        /// <summary>
        /// 初始化配置
        /// </summary>
        static DP()
        {
            if (config == null)
            {

                config = Configuration.Config.Bind<List<TableConfiguration>>("TableConfig.json");
;                //config = (List<TableConfiguration>)SerializerXml.LoadSettings(@"E:\01技术研究\ORM\TYSystem.Dapper\Dapper.Test\bin\Debug\TableConfig.xml", typeof(List<TableConfiguration>));
            }
        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static LambdaQueryHelper<T> Create<T>() where T : class
        {
            Type type = typeof(T);

            TableMapping map = type.GetCustomAttributes(typeof(TableMapping), false)[0] as TableMapping;
            TableConfiguration table = config.Where(p => p.Name == map.ConfigName).FirstOrDefault();

            DataBaseType BaseType;
            IDbConnection connection;
            if (table != null)
            {
                switch (table.SqlType)
                {
                    case "MsSql": BaseType = DataBaseType.SqlServer; connection = GetMsSqlConn(table.ConnectString); break;
                    case "MySql": BaseType = DataBaseType.MySql; connection = GetMySqlConn(table.ConnectString); break;
                    default: BaseType = DataBaseType.SqlServer; connection = GetMsSqlConn(table.ConnectString); break;
                }
                return DapperExtension.Instance(BaseType).LambdaQuery<T>(connection, null, null);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取MySql
        /// </summary>
        /// <param name="constring"></param>
        /// <returns></returns>
        public static IDbConnection GetMySqlConn(string constring)
        {
            return new MySqlConnection(constring);
        }
        public static IDbConnection GetMsSqlConn(string constring)
        {
            return new SqlConnection(constring);
        }

    }
}
