using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.SimplifiedTime
{


    /// <summary>
    /// 日期抽象类;简化的 System.Calendar
    /// 年 : short
    /// 月,日 : byte
    /// 一段时间 : short
    /// </summary>
    public abstract class Calendar
    {

        public static Calendar Default
        {
            get { return new ChineseCalendar(); }
        }


        /// <summary>
        /// 获取到这一年的天数;
        /// </summary>
        public virtual short GetDaysInYear(short year)
        {
            short day = 0;
            for (byte month = GetMonthsInYear(year); month > 0; month--)
            {
                day += GetDaysInMonth(year, month);
            }
            return day;
        }

        /// <summary>
        /// 这一年的月数;
        /// </summary>
        public abstract byte GetMonthsInYear(short year);

        /// <summary>
        /// 这一个月的天数;
        /// </summary>
        public abstract byte GetDaysInMonth(short year, byte month);

        /// <summary>
        /// 获取到这一年闰几月,若不存在则返回 0;
        /// </summary>
        public abstract byte GetLeapMonth(short year);

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        public abstract bool IsLeapYear(short year);

        /// <summary>
        /// 这个月是否为闰月?
        /// </summary>
        public abstract bool IsLeapMonth(short year, byte month);

    }

}
