using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TYSystem.BaseFramework.BaseResult
{
    /// <summary>
    /// 统一返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResultList<T>
    {
        public DataResultList()
        {
            this.ResultList = new List<T>();
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IList<T> ResultList { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
    }
}
