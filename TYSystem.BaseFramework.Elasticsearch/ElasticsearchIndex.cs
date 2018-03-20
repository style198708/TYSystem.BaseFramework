using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Elasticsearch.Net;
using Nest;

namespace TYSystem.BaseFramework.Elasticsearch
{
    public class ElasticsearchIndex
    {
        [Keyword(Name = "Name")]
        public string Name { get; set; }
        [Text(Name = "Age")]
        public int Age { get; set; }

    }
}
