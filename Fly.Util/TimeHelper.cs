using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Util
{
    public static class TimeHelper
    {
        public static string ToTimeText(this DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;

            if (span.TotalDays > 7)
            {
                return dt.ToString("yyyy/MM/dd HH:mm");
            }

            if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }

            if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }

            if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }

            if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }

            return "刚刚";

        }
    }

}
