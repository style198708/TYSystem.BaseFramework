using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TYSystem.BaseFramework.MongoDB
{
    public class MongoDBHelper
    {
        public MongoDBHelper()
        {
            Client = new MongoClient(Configuration.Config.Settings.Mongodb);
        }
        private MongoClient Client { get; set; }

        private IMongoDatabase DataBase { get => Client.GetDatabase("MengTLog"); }

        public IMongoCollection<T> DbSet<T>() where T : IMongoModel => DataBase.GetCollection<T>("MengTLog.Logger");

    }
    public static class MongoExtend
    {
        public static void Add<T>(this IMongoCollection<T> collenction, T Model) where T : IMongoModel
                  => collenction.InsertOne(Model);

        public static void AddList<T>(this IMongoCollection<T> collenction, List<T> Model) where T : IMongoModel
                 => collenction.InsertMany(Model);

        /// <summary>
        /// 查找第一个
        /// </summary>
        public static T FirstOrDefault<T>(this IMongoCollection<T> collenction, Expression<Func<T, bool>> expression) where T : IMongoModel
        {
            if (expression == null) { throw new ArgumentNullException("参数无效"); }
            return collenction.Find(expression).FirstOrDefault();
        }

        /// <summary>
        /// 查找符合数据列表
        /// </summary>
        public static List<T> FindToList<T>(this IMongoCollection<T> collenction, Expression<Func<T, bool>> expression) where T : IMongoModel
        {
            if (expression == null) { throw new ArgumentNullException("参数无效"); }
            return collenction.Find(expression).ToList();
        }

        /// <summary>
        /// 删除全部匹配数据
        /// </summary>
        public static void Delete<T>(this IMongoCollection<T> collenction, Expression<Func<T, bool>> expression) where T : IMongoModel
        {
            if (expression == null) { throw new ArgumentNullException("参数无效"); }
            collenction.DeleteManyAsync(expression);
        }

        /// <summary>
        /// 删除一个
        /// </summary>
        public static void DeleteOne<T>(this IMongoCollection<T> collenction, Expression<Func<T, bool>> expression) where T : IMongoModel
        {
            if (expression == null) { throw new ArgumentNullException("参数无效"); }
            collenction.DeleteOneAsync(expression);
        }
    }
}
