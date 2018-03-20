using System;
using System.Collections.Generic;
using System.Text;

namespace TYSystem.BaseFramework.BaseResult
{
    public class JsonPageDataResult<T> : JsonDataBase
    {
        public DataResultList<T> data { get; set; }
    }
}
