using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.RegularExpressions;
using TYSystem.BaseFramework.Common;

namespace TYSystem.BaseFramework.Helper
{
    public class BasePager
    {
        private string _PageParm = "Page";

        public BasePager(int RecordCount, int TotalPage, int PageIndex, int PageSize, int PageAround, string Rule)
        {
            this.RecordCount = RecordCount;
            this.TotalPage = TotalPage;
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
            if (PageAround == 0)
                this.PageAround = 2;
            else
                this.PageAround = PageAround;
            this.Rule = Rule;
        }


        public string GetPagerHtml()
        {

            //< a class="pull-left item" href="javascript:;"><</a>
            //<a class="pull-left item active" href="javascript:;">1</a>
            //<a class="pull-left item" href="javascript:;">...</a>
            //<a class="pull-left item" href="javascript:;">2</a>
            //<a class="pull-left item" href="javascript:;">3</a>
            //<a class="pull-left item" href="javascript:;">4</a>
            //<a class="pull-left item" href="javascript:;">...</a>
            //<a class="pull-left item" href="javascript:;">5</a>
            //<a class="pull-left item" href="javascript:;">></a>


            //优先TotalPage，如果没有传入TotalPage，则通过计算得出总页数
            if (this.RecordCount > 0 && this.TotalPage <= 0)
            {
                this.TotalPage = this.RecordCount % this.PageSize == 0 ? this.RecordCount / this.PageSize : this.RecordCount / this.PageSize + 1;
            }

            if (this.RecordCount <= 0)
            {
                return "";
            }

            if (PageIndex <= 0) PageIndex = 1;
            if (PageIndex > TotalPage) PageIndex = 1;

            //int pageAround = 4;

            if (this.TotalPage > 0 || this.PageSize > 0)
            {
                StringBuilder sb = new StringBuilder();

                //上一页.OK
                if (this.PageIndex > 1)
                {
                    sb.AppendFormat("<a href='{0}' class='pull-left prev'>&lt;</a>", this.GetPageUrl(this.PageIndex - 1));
                }
                else
                {
                    //sb.AppendFormat("<a href='javascript:;' class='pull-left prev'>&lt;</a>");
                }

                //第一页.OK
                if (this.PageIndex == 1)
                {
                    sb.Append("<a class='pull-left item active'>1</a>");
                }
                else
                {
                    sb.AppendFormat("<a href='{0}' class='pull-left item'>1</a>", this.GetPageUrl(1));
                }

                //前省略号
                bool preFlag = false;
                if (this.PageIndex >= PageAround * 2 + 1)
                {
                    sb.Append("<span class='pull-left page_break'>...</span>");
                }
                else
                {
                    //前连续分页
                    for (int i = 2; i < PageAround * 2 + 2 && i < TotalPage; i++)
                    {
                        if (i == this.PageIndex)
                        {
                            //sb.AppendFormat("<span class='numbercurrent'>{0}</span>", i);
                            sb.AppendFormat("<a class='pull-left item active'>{0}</a>", i);
                        }
                        else
                        {
                            sb.AppendFormat("<a href='{1}' class='pull-left item'>{0}</a>", i, this.GetPageUrl(i));
                        }
                    }
                    preFlag = true;
                }

                //中间
                if (!preFlag && !(this.TotalPage - this.PageIndex < PageAround * 2 + 1))
                {
                    for (int i = this.PageIndex - PageAround; i < this.PageIndex + PageAround + 1 && i < TotalPage; i++)
                    {
                        if (i == this.PageIndex)
                        {
                            //sb.AppendFormat("<span class='numbercurrent'>{0}</span>", i);
                            sb.AppendFormat("<a class='pull-left item active'>{0}</a>", i);
                        }
                        else
                        {
                            sb.AppendFormat("<a href='{1}' class='pull-left item'>{0}</a>", i, this.GetPageUrl(i));
                        }
                    }
                }

                //后省略号
                if (!(!preFlag && this.TotalPage - this.PageIndex < PageAround * 2 + 1) && this.TotalPage > PageAround * 2 + 2)
                {
                    sb.Append("<span class='pull-left page_break'>...</span>");
                }
                //后连续分页
                if (!preFlag && this.TotalPage - this.PageIndex < PageAround * 2 + 1)
                {
                    for (int i = this.TotalPage - PageAround * 2; i < this.TotalPage; i++)
                    {
                        if (i == this.PageIndex)
                        {
                            // sb.AppendFormat("<span class='numbercurrent'>{0}</span>", i);
                            sb.AppendFormat("<a class='pull-left item active'>{0}</a>", i);
                        }
                        else
                        {
                            sb.AppendFormat("<a href='{1}' class='pull-left item'>{0}</a>", i, this.GetPageUrl(i));
                        }
                    }
                }

                //最后一页
                if (this.TotalPage > 1)
                {
                    if (this.TotalPage == this.PageIndex)
                    {
                        //sb.AppendFormat("<span class='numbercurrent'>{0}</span>", this.TotalPage);
                        sb.AppendFormat("<a class='pull-left item active'>{0}</a>", this.TotalPage);
                    }
                    else
                    {
                        sb.AppendFormat("<a href='{1}' class='pull-left item'>{0}</a>", this.TotalPage, this.GetPageUrl(this.TotalPage));
                    }
                }

                //下一页
                if (this.TotalPage > 1 && this.PageIndex < this.TotalPage)
                {

                    sb.AppendFormat("<a href='{0}' class='pull-left item next '>&gt;</a>", this.GetPageUrl(this.PageIndex + 1));

                }
                else
                {
                    //sb.AppendFormat("<span class='pagel f_simsun stateoff'>&gt;</span>");

                    //sb.AppendFormat("<a href='javascript:;' class='pull-left item'>&gt;</a>");
                }

                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        public string GetPagerHtmlv2()
        {

           

            //优先TotalPage，如果没有传入TotalPage，则通过计算得出总页数
            if (this.RecordCount > 0 && this.TotalPage <= 0)
            {
                this.TotalPage = this.RecordCount % this.PageSize == 0 ? this.RecordCount / this.PageSize : this.RecordCount / this.PageSize + 1;
            }

            if (this.RecordCount <= 0)
            {
                return "";
            }

            if (PageIndex <= 0) PageIndex = 1;
            if (PageIndex > TotalPage) PageIndex = 1;

            //int pageAround = 4;

            if (this.TotalPage > 0 || this.PageSize > 0)
            {
                StringBuilder sb = new StringBuilder();

                //上一页.OK
                if (this.PageIndex > 1)
                {
                    sb.AppendFormat("<a href='{0}' class='pagel'>&lt;</a>", this.GetPageUrl(this.PageIndex - 1));
                }
                else
                {

                    sb.AppendFormat("<a class='pagel f_simsun stateoff'>&lt;</a>");
                }

                //下一页
                if (this.TotalPage > 1 && this.PageIndex < this.TotalPage)
                {

                    sb.AppendFormat("<a href='{0}' class='pagel'>&gt;</a>", this.GetPageUrl(this.PageIndex + 1));

                }
                else
                {

                    sb.AppendFormat("<a class='pagel f_simsun stateoff'>&gt;</a>");
                }

                return sb.ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 评论页码数
        /// </summary>
        /// <returns></returns>
        public string GetPagerHtmlv3()
        {

          

            //优先TotalPage，如果没有传入TotalPage，则通过计算得出总页数
            if (this.RecordCount > 0 && this.TotalPage <= 0)
            {
                this.TotalPage = this.RecordCount % this.PageSize == 0 ? this.RecordCount / this.PageSize : this.RecordCount / this.PageSize + 1;
            }

            if (this.RecordCount <= 0)
            {
                return "";
            }

            if (PageIndex <= 0) PageIndex = 1;
            if (PageIndex > TotalPage) PageIndex = 1;

            //int pageAround = 4;

            if (this.TotalPage > 0 || this.PageSize > 0)
            {
                StringBuilder sb = new StringBuilder();

                //上一页.OK
                if (this.PageIndex > 1)
                {
                    sb.AppendFormat("<a href='{0}' class='pull-left item'>&lt;</a>", this.GetPageUrl(this.PageIndex - 1));
                }
                else
                {
                    //sb.AppendFormat("<span class='pagel f_simsun stateoff'>&lt;</span>");
                    sb.AppendFormat("<a class='pull-left item'>&lt;</a>");
                }

                //第一页.OK
                if (this.PageIndex == 1)
                {
                    //sb.Append("<span class='numbercurrent'>1</span>");
                    sb.Append("<a class='pull-left item'>1</a>");
                }
                else
                {
                    sb.AppendFormat("<a href='{0}' class='pull-left item'>1</a>", this.GetPageUrl(1));
                }

                //前省略号
                bool preFlag = false;
                if (this.PageIndex >= PageAround * 2 + 1)
                {
                    sb.Append("<a class='pull-left item' href='javascript:;'>...</a>");
                }
                else
                {
                    //前连续分页
                    for (int i = 2; i < PageAround * 2 + 2 && i < TotalPage; i++)
                    {
                        if (i == this.PageIndex)
                        {
                            //sb.AppendFormat("<span class='numbercurrent'>{0}</span>", i);
                            sb.AppendFormat("<a class='numbercurrent'>{0}</a>", i);
                        }
                        else
                        {
                            sb.AppendFormat("<a href='{1}' class='number'>{0}</a>", i, this.GetPageUrl(i));
                        }
                    }
                    preFlag = true;
                }

                //中间
                if (!preFlag && !(this.TotalPage - this.PageIndex < PageAround * 2 + 1))
                {
                    for (int i = this.PageIndex - PageAround; i < this.PageIndex + PageAround + 1 && i < TotalPage; i++)
                    {
                        if (i == this.PageIndex)
                        {
                            //sb.AppendFormat("<span class='numbercurrent'>{0}</span>", i);
                            sb.AppendFormat("<a class='numbercurrent'>{0}</a>", i);
                        }
                        else
                        {
                            sb.AppendFormat("<a href='{1}' class='number'>{0}</a>", i, this.GetPageUrl(i));
                        }
                    }
                }

                //后省略号
                if (!(!preFlag && this.TotalPage - this.PageIndex < PageAround * 2 + 1) && this.TotalPage > PageAround * 2 + 2)
                {
                    sb.Append("<span class='page_break'>...</span>");
                }
                //后连续分页
                if (!preFlag && this.TotalPage - this.PageIndex < PageAround * 2 + 1)
                {
                    for (int i = this.TotalPage - PageAround * 2; i < this.TotalPage; i++)
                    {
                        if (i == this.PageIndex)
                        {
                            // sb.AppendFormat("<span class='numbercurrent'>{0}</span>", i);
                            sb.AppendFormat("<a class='numbercurrent'>{0}</a>", i);
                        }
                        else
                        {
                            sb.AppendFormat("<a href='{1}' class='number'>{0}</a>", i, this.GetPageUrl(i));
                        }
                    }
                }

                //最后一页
                if (this.TotalPage > 1)
                {
                    if (this.TotalPage == this.PageIndex)
                    {
                        //sb.AppendFormat("<span class='numbercurrent'>{0}</span>", this.TotalPage);
                        sb.AppendFormat("<a class='numbercurrent'>{0}</a>", this.TotalPage);
                    }
                    else
                    {
                        sb.AppendFormat("<a href='{1}' class='number'>{0}</a>", this.TotalPage, this.GetPageUrl(this.TotalPage));
                    }
                }

                //下一页
                if (this.TotalPage > 1 && this.PageIndex < this.TotalPage)
                {

                    sb.AppendFormat("<a href='{0}' class='pagel'>&gt;</a>", this.GetPageUrl(this.PageIndex + 1));

                }
                else
                {
                    //sb.AppendFormat("<span class='pagel f_simsun stateoff'>&gt;</span>");
                    sb.AppendFormat("<a class='pagel f_simsun stateoff'>&gt;</a>");
                }

                return sb.ToString();
            }
            else
            {
                return "";
            }

        }
        public string PageParm
        {
            get { return _PageParm; }
            set { _PageParm = value; }
        }
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount
        {
            get;
            set;
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        public int PageAround
        {
            get;
            set;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get;
            set;
        }

        public string OtherParm
        {
            get;
            set;
        }


        public bool PreFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 网址规则
        /// </summary>
        public string Rule { get; set; }

        public IQueryCollection QueryString { get; set; }

        /// <summary>
        /// 获得页面Url
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public string GetPageUrl(int pageIndex)
        {
            if (QueryString == null)
            {

                this.QueryString =Compatible.HttpContext.Current.Request.Query;
            }

            string pageUrl = string.Empty;

            if (string.IsNullOrEmpty(this.Rule))
            {
                string pattern = @"-p\d+.html";
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);

                // 如果找到匹配,也就是URL中含有类似 ?page=3 或者 &page=4 这样的字符串,则对后面的数值进行替换
                if (reg.IsMatch(pageUrl))
                {
                    pageUrl = reg.Replace(pageUrl, "-p" + pageIndex.ToString() + ".html");
                }
                else
                {
                    pageUrl = pageUrl.Replace(".html", "-p" + pageIndex.ToString() + ".html");
                }
            }
            else
            {
                string pattern = @"{(.+?)}";
                pageUrl = Regex.Replace(this.Rule.Replace("{page}", pageIndex.ToString()), pattern, new MatchEvaluator((match) =>
                {
                    if (!string.IsNullOrEmpty(this.QueryString[match.Groups[1].ToString()]))
                    {
                        return this.QueryString[match.Groups[1].ToString()];
                    }
                    else
                    {
                        return "";
                    }
                }), RegexOptions.Compiled);
            }

            return pageUrl;
        }
    }
}
