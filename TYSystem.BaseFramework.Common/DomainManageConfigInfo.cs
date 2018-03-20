using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TYSystem.BaseFramework.Serializer;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using TYSystem.BaseFramework.Configuration;

namespace TYSystem.BaseFramework.Common
{
    /// <summary>
    /// 域名地址管理
    /// </summary>
    public class DomainManageConfigInfo
    {
        public static WDBuyNET config;

        static DomainManageConfigInfo()
        {
            if (config == null)
            {
                config = Config.Bind<WDBuyNET>("SettingHandler.json");
            }
        }

        /// <summary>
        /// 主站地址
        /// </summary>
        public static string PortalUrl
        {
            get
            {
                return config.WDBuyNETMainSite;
            }
        }

        /// <summary>
        /// 单品地址
        /// </summary>
        public static string ProductUrl
        {
            get
            {
                return config.WDBuyNETProductSite;
            }
        }

        /// <summary>
        /// 公募基金
        /// </summary>
        public static string PublicFundUrl
        {
            get
            {
                return config.WDBuyNETPublicFundSite;
            }
        }

        /// <summary>
        /// 后台域名
        /// </summary>
        public static string IboAdminUrl
        {
            get
            {
                return config.WDBuyNETIboAdminSite;
            }
        }


        /// <summary>
        /// 脚本/css域名
        /// </summary>
        public static string BaseServerUrl
        {
            get
            {
                return config.WDBuyNETBaseSite;

            }
        }

        /// <summary>
        /// 图片服务地址
        /// </summary>
        public static string ImgServerUrl
        {
            get
            {
                return config.WDBuyNETImgsSite;
            }
        }

        /// <summary>
        /// 文件服务地址
        /// </summary>
        public static string FileServerUrl
        {
            get
            {
                return config.WDBuyNETFilesSite;
            }
        }

        //
        /// <summary>
        /// 认证中心域名地址
        /// </summary>
        public static string PassportUrl
        {
            get
            {
                return config.WDBuyNETPassportSite;

            }
        }

        /// <summary>
        /// 会员中心地址
        /// </summary>
        public static string MemberUrl
        {
            get
            {
                return config.WDBuyNETMemberSite;
            }
        }
        /// <summary>
        /// 支付中心地址
        /// </summary>
        public static string PayUrl
        {
            get
            {
                return config.WDBuyNETPaySite;
            }
        }
        public static string EventUrl
        {
            get
            {
                return config.WDBuyNETEventSite;
            }

        }
        /// <summary>
        /// 购物车地址
        /// </summary>
        public static string CartUrl
        {
            get
            {
                return config.WDBuyNETCartSite;
            }
        }

        /// <summary>
        /// 统计
        /// </summary>
        public static string AnalyticsUrl
        {
            get
            {
                return config.WDBuyNETAnalyticsSite;
            }
        }

        public static string MobileUrl
        {
            get
            {
                return config.WDBuyNETMobileSite;
            }
        }
        public static string WebAppUrl
        {
            get
            {
                return config.WDBuyNETWebAppSite;
            }
        }
    }

    public class WDBuyNET
    {
        public string WDBuyNETMainSite { get; set; }
        public string WDBuyNETProductSite { get; set; }
        public string WDBuyNETPublicFundSite { get; set; }
        public string WDBuyNETCartSite { get; set; }
        public string WDBuyNETPassportSite { get; set; }
        public string WDBuyNETBaseSite { get; set; }
        public string WDBuyNETMemberSite { get; set; }
        public string WDBuyNETImgsSite { get; set; }
        public string WDBuyNETFilesSite { get; set; }
        public string WDBuyNETPaySite { get; set; }
        public string WDBuyNETEventSite { get; set; }
        public string WDBuyNETIboAdminSite { get; set; }
        public string WDBuyNETAnalyticsSite { get; set; }
        public string WDBuyNETMobileSite { get; set; }
        public string WDBuyNETWebAppSite { get; set; }

    }
}
