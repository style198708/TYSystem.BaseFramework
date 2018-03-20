using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TYSystem.BaseFramework.SqlSugar
{
    public class SqlServerHelp
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static SqlSugarClient ConfigInit()
        {
            return new SqlSugarClient(new ConnectionConfig() { ConnectionString = Config.SqlServerConnectionString, DbType = DbType.SqlServer, IsAutoCloseConnection = true });
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="type"></param>
        public static SqlSugarClient ConfigInit(DbType type)
        {
            ConnectionConfig config = new ConnectionConfig();
            config.DbType = type;
            config.IsAutoCloseConnection = true;
            switch (type)
            {
                case DbType.SqlServer: config.ConnectionString = Config.SqlServerConnectionString; break;
                case DbType.MySql: config.ConnectionString = Config.MySqlConnectionString; break;
            }
            return new SqlSugarClient(config);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static SqlSugarClient ConfigInit(ConnectionConfig config)
        {
            return new SqlSugarClient(config);
        }

    }
    public  static class SqlSugarExtentions
    {
        /// <summary>
        /// 实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this SqlSugarClient client) where T:class,new()
        {
            return client.Queryable<T>().First();
        }
    }
}
