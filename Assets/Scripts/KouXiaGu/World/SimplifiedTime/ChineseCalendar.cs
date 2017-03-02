using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace KouXiaGu.SimplifiedTime
{

    /// <summary>
    /// 游戏使用的日历;
    /// </summary>
    public class ChineseCalendar : Calendar
    {

        /// <summary>
        /// 月份天数;
        /// </summary>
        static readonly byte[] _dayOfMonth = new byte[]
            {
                30, // 1
                29,
                30, // 3
                30,
                29, // 5
                30,
                30, // 7
                29,
                30, // 9
                30,
                29, // 11
                30,
            };

        /// <summary>
        /// 这一个月的天数;
        /// </summary>
        public override byte GetDaysInMonth(short year, byte month)
        {
            int leapMonth = GetLeapMonth(year);

            if (leapMonth != 0 && month >= leapMonth)
            {
                month--;
            }

            return _dayOfMonth[--month];
        }

        /// <summary>
        /// 这一年的月数;
        /// </summary>
        public override byte GetMonthsInYear(short year)
        {
            /// <summary>
            /// 闰年月数;
            /// </summary>
            const byte LEAP_YEAR_MONTH_COUNT = 13;

            /// <summary>
            /// 非闰年月数;
            /// </summary>
            const byte NOT_LEAP_YEAR_MONTH_COUNT = 12;

            return IsLeapYear(year) ? LEAP_YEAR_MONTH_COUNT : NOT_LEAP_YEAR_MONTH_COUNT;
        }


        /// <summary>
        /// 闰月分配表,需要不能被三整除的容量;
        /// 4  的顺序: 0 3 2 1 ...
        /// </summary>
        static readonly byte[] _leapMonthDistribution = new byte[]
            {
                11,
                8,
                5,
                3,
            };

        /// <summary>
        /// 获取到这一年闰几月,若闰7月则返回8,八月返回9,若不存在则返回 0;
        /// </summary>
        public override byte GetLeapMonth(short year)
        {
            if (IsLeapYear(year))
            {
                int seed = year % _leapMonthDistribution.Length;
                seed = Math.Abs(seed);
                return _leapMonthDistribution[seed];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 这个月是否为闰月?
        /// </summary>
        public override bool IsLeapMonth(short year, byte month)
        {
            byte leapMonth = GetLeapMonth(year);
            return leapMonth != 0 && leapMonth == month;
        }

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        public override bool IsLeapYear(short year)
        {
            /// <summary>
            /// 每几年置闰年;
            /// </summary>
            const int LEAP_YEAR_INTERAVAL = 3;

            return year % LEAP_YEAR_INTERAVAL == 0;
        }

    }

}
