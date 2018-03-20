using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TYSystem.BaseFramework.BaseParam
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageParam
    {
        private int _pageIndex = 1;
        public PageParam()
        {
            this.PageSize = 15;
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value <= 0 ? 1 : value; }
        }
        /// <summary>
        /// 当前页大小
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 多少页
        /// </summary>
        public int? TotalPage { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int? SortMode { get; set; }

    }
}
