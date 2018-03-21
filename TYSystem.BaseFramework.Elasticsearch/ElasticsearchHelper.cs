using System;
using System.Collections.Generic;
using System.Text;
using Elasticsearch.Net;
using Nest;
using System.Linq;
using System.Linq.Expressions;


namespace TYSystem.BaseFramework.Elasticsearch
{
    public class ElasticsearchHelper
    {
        private static ElasticClient Client { get; set; }

        private static string IndexName { get; set; }

        static ElasticsearchHelper()
        {
            //Config config = TYSystem.BaseFramework.Configuration.Config.Bind<Config>("Elasticsearch.json");
            Config config = new Config()
            {
                ElasticsearchType = "Single",
                ConfigUrl = "http://192.168.126.136:9200/"
            };
            if (Client == null)
            {
                ConnectionSettings connectionSettings = new ConnectionSettings();
                if (config.ElasticsearchType == EnumElasticsearch.Single)
                {
                    connectionSettings = new ConnectionSettings(new Uri(config.ConfigUrl));
                }
                else if (config.ElasticsearchType == EnumElasticsearch.Multiple)
                {
                    StaticConnectionPool pool = new StaticConnectionPool(config.ConfigUrl.Split(",").Select(p => new Uri(p)).ToList());
                    connectionSettings = new ConnectionSettings(pool);
                }
                else
                {
                    StaticConnectionPool pool = new StaticConnectionPool(config.ConfigUrl.Split(",").Select(p => new Node(new Uri(p))).ToList());
                    connectionSettings = new ConnectionSettings(pool);
                }
                Client = new ElasticClient(connectionSettings);
            }
        }

        /// <summary>
        /// 创建索引(数据库)
        /// </summary>
        private static void CreateIndex<T>() where T : ElasticsearchBase
        {
            if (string.IsNullOrWhiteSpace(IndexName))
            {
                IndexName = "db_" + typeof(T).Name.ToLower();
            }

            if (!Client.IndexExists(IndexName).Exists)
            {
                IIndexState indexState = new IndexState()
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = 1,//副本数
                        NumberOfShards = 5//分片数
                    }
                };
                //创建并Mapping
                Client.CreateIndex(IndexName, p => p.InitializeUsing(indexState).Mappings(m => m.Map<T>(mp => mp.AutoMap())));
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Add<T>(T entity) where T : ElasticsearchBase
        {
            CreateIndex<T>();
            Client.Index<T>(entity, o => o.Index(IndexName).Type<T>());
            return true;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Update<T>(T entity) where T : ElasticsearchBase
        {
            CreateIndex<T>();

            DocumentPath<T> deletePath = new DocumentPath<T>(entity.IndexId);
            IUpdateRequest<T, T> request = new UpdateRequest<T, T>(deletePath)
            {
                Doc = entity
            };
            var response = Client.Update<T, T>(request);
            return true;
        }


        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static T GetByID<T>(long Id) where T : ElasticsearchBase
        {
            CreateIndex<T>();
            var response = Client.Get(new DocumentPath<T>(Id), pd => pd.Index(IndexName).Type<T>());
            return response.Source;
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<T> GetByIDs<T>(List<long> Ids) where T : ElasticsearchBase
        {
            CreateIndex<T>();
            var response = Client.MultiGet(m => m.Index(IndexName).GetMany<T>(Ids));
            return response.Hits.Select(p => (T)p.Source).ToList();
        }


        #region 查询业务具体封装

        /// <summary>
        /// 前50条记录
        /// 参照 https://www.cnblogs.com/huhangfei/p/5726650.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ISearchResponse<T> Query<T>(Func<SearchDescriptor<T>, ISearchRequest> func) where T : ElasticsearchBase
        {
            return Client.Search<T>(s => s.Index(IndexName).From(0).Size(50));
        }

        #endregion
    }
}
