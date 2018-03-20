using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace TYSystem.BaseFramework.Elasticsearch
{
    /// <summary>
    /// 
    /// </summary>
    public class ElasticsearchHelper
    {
        private static ElasticClient Client { get; set; }

        static ElasticsearchHelper()
        {
            var uri = new Uri("http://mynode.somewhere.com/");

            //var connectionConfiguration = new ConnectionConfiguration()
            //        .DisableAutomaticProxyDetection()
            //        .EnableHttpCompression()
            //        .DisableDirectStreaming()
            //        .PrettyJson()
            //        .RequestTimeout(TimeSpan.FromMinutes(2));
            //var lowLevelClient = new ElasticLowLevelClient(connectionConfiguration);


            //var connectionSettings = new ConnectionSettings(new Uri("http://192.168.126.132:9200"))
            //      .DefaultMappingFor<ElasticsearchIndex>(i => i
            //        .IndexName("my-projects")
            //        .TypeName("project")
            //       ).EnableDebugMode()
            //    .PrettyJson()
            //    .RequestTimeout(TimeSpan.FromMinutes(2));
            var connSettings = new ConnectionSettings(new Uri("http://92.168.126.132:9200/"));
             

                Client = new ElasticClient(connSettings);

            //var settings = new ConnectionSettings(uri, defaultIndex = "my-application");

        }


        /// <summary>
        /// 创建索引
        /// </summary>
        /// <returns></returns>
        public static bool CreateIndex()
        {
            ElasticsearchIndex index = new ElasticsearchIndex() { Name = "线吉林若天有叵地一", Age = 18 };
            Client.CreateIndex("default", i => i.Mappings(m => m.Map<ElasticsearchIndex>(ms => ms.AutoMap())));
            return true;
        }

        /// <summary>
        /// 查询索引
        /// </summary>
        /// <returns></returns>
        public List<string> QueryIndex()
        {
            return null;
        }
    }
}
