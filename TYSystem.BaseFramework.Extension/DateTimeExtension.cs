using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TYSystem.BaseFramework.Extension
{
    public static class DateTimeExtension
    {
        #region 格式化日期
        /// <summary>
        /// 返回标准时间到秒 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="fDateTime">时间</param>
        /// <returns></returns>
        public static String FomatDateTimeToSecond(this Object fDateTime)
        {
            if (fDateTime.IsNullOrEmpty()) return "";
            DateTime dtOut = new DateTime();
            DateTime.TryParse(fDateTime.ToString(), out dtOut);
            return dtOut.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 返回标准时间到分 yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="fDateTime">时间</param>
        /// <returns></returns>
        public static String FomatDateTimeToMinute(this Object fDateTime)
        {
            if (fDateTime.IsNullOrEmpty()) return "";
            DateTime dtOut = new DateTime();
            DateTime.TryParse(fDateTime.ToString(), out dtOut);
            return dtOut.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 返回标准时间到月 yyyy-MM-dd
        /// </summary>
        /// <param name="fDateTime">时间</param>
        /// <returns></returns>
        public static String FomatDateTimeToMonth(this Object fDateTime)
        {
            if (fDateTime.IsNullOrEmpty()) return "";
            DateTime dtOut = new DateTime();
            DateTime.TryParse(fDateTime.ToString(), out dtOut);
            return dtOut.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 返回小时形式
        /// </summary>
        /// <param name="fDateTime"></param>
        /// <returns></returns>
        public static String FomatDateTimeToHour(this Object fDateTime)
        {
            if (fDateTime.IsNullOrEmpty()) return "";
            DateTime dtOut = new DateTime();
            DateTime.TryParse(fDateTime.ToString(), out dtOut);
            return dtOut.ToString("HH:mm");
        }
        #endregion

        #region 统计时间间隔
        /// <summary>
        /// 把发表的时间改为几个月，几周前，几天前，几小时前，几分钟前，或几秒前
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNow(this DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else
            {
                if (span.TotalDays > 30)
                {

                    return "1个月前";
                }
                else
                {
                    if (span.TotalDays > 14)
                    {

                        return "2周前";
                    }
                    else
                    {
                        if (span.TotalDays > 7)
                        {
                            return "1周前";
                        }
                        else
                        {
                            if (span.TotalDays > 1)
                            {
                                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                            }
                            else
                            {
                                if (span.TotalHours > 1)
                                {
                                    return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                                }
                                else
                                {
                                    if (span.TotalMinutes > 1)
                                    {
                                        return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                    }
                                    else
                                    {
                                        if (span.TotalSeconds >= 1)
                                        {
                                            return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                        }
                                        else
                                        {
                                            return "1秒前";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 时间格式转型（刚刚，10分钟前，30分钟前，xx小时前，xx天前，M月dd日）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string DateStringFromNow2(this DateTime time)
        {
            if (time == null)
                return "";
            string datestr = string.Empty;
            TimeSpan span = DateTime.Now - time;
            int Millis = span.Minutes;
            int hours = span.Hours;
            int day = span.Days;

            if (day >= 7)
            {
                return time.ToString("MM月dd日");
            }
            else if (day >= 1 && day < 7)
            {
                return day + "天前";
            }
            else
            {
                if (hours < 1)
                {
                    if (Millis < 30)
                    {
                        if (Millis < 10)
                        {
                            return "刚刚";
                        }
                        else
                        {
                            return "10分钟前";
                        }
                    }
                    else
                    {
                        return "30分钟前";
                    }
                }
                else
                {

                    return hours + "小时前";
                }
            }
        }

        #endregion

    }
}
