using System;
using System.Collections.Generic;
using System.Text;

namespace TYSystem.BaseFramework.Elasticsearch
{


    public class EnumElasticsearch
    {
        public const string Single = "Single";
        public const string Multiple = "Multiple";
        public const string MultipleNode = "MultipleNode";
    }

    public class Config
    {

        public string ElasticsearchType { get; set; }

        public string ConfigUrl { get; set; }


    }

    /// <summary>
    /// 单地址
    /// </summary>
    public class ElasticsearchSingConfig
    {
        public string Url { get; set; }

    }

    /// <summary>
    /// 多地址
    /// </summary>
    public class ElasticsearchMultConfig
    {
        public string[] Urls { get; set; }

    }


    /// <summary>
    /// 多节点
    /// </summary>
    public class ElasticsearchMultNodeConfig
    {
        public string[] Nodes { get; set; }
    }


}
