using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TYSystem.BaseFramework.Common
{
    /// <summary>
    /// 静态公共变量
    /// </summary>
    public class StaticConst
    {
        /// <summary>
        /// 默认最小时间
        /// </summary>
        public static DateTime DATEBEGIN = Convert.ToDateTime("1900-01-01 00:00:00");
        /// <summary>
        /// 默认最大时间
        /// </summary>
        public static DateTime DATEMAX = Convert.ToDateTime("2099-01-01 00:00:00");
        /// <summary>
        /// 购物车默认最大数据
        /// </summary>
        public static int DEFAULT_CART_COUNT = 999;
        /// <summary>
        /// 后台用户默认密码
        /// </summary>
        public const string DEFAULT_ADMIN_PWD = "cst888";
        /// <summary>
        /// 保证金
        /// </summary>
        public const decimal SUPPLIERBAIL = 2000;
        /// <summary>
        /// 加盟费
        /// </summary>
        public const decimal AGENTBAIL = 5000;
        /// <summary>
        /// 提现手续费
        /// </summary>
        public const decimal WITHDRAWALS_FEE = 20;
        /// <summary>
        /// 最多可同时发放店铺优惠券数量
        /// </summary>
        public const int MAX_COUPON_COUNT = 5;

        /// <summary>
        /// 系统生成用户ID
        /// 如自动发邮件等工作
        /// </summary>
        public static int SystemCreateUser = 1;

        public static float FLOAT_DEFAULT_VALUE = -99999;   
        public static decimal DECIMAL_DEFAULT_VALUE = -99999;


        #region Redis 缓存库
        public static int RedisDBNum0 = 0;
        public static int RedisDBNum1 = 1;
        public static int RedisDBNum2 = 2;
        #endregion

        #region 缓存Key
       
        #endregion

    }
}
