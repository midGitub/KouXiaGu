using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.TimeSystem
{

    /// <summary>
    /// 区别于现实的农历;
    /// </summary>
    public class ChineseCalendar : ICalendar
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

        public int GetDaysInMonth(int year, int month)
        {
            int leapMonth = GetLeapMonth(year);
            if (leapMonth != 0 && month >= leapMonth)
            {
                month--;
            }
            return _dayOfMonth[month];
        }

        public int GetMonthsInYear(int year)
        {
            const int leapYear = 13;
            const int notLeapYear = 12;
            return IsLeapYear(year) ? leapYear : notLeapYear;
        }

        /// <summary>
        /// 闰月分配表,需要不能被三整除的容量;
        /// 4  的顺序: 0 3 2 1 ...
        /// </summary>
        static readonly int[] _leapMonthDistribution = new int[]
            {
                11,
                8,
                5,
                3,
            };

        /// <summary>
        /// 获取到这一年闰几月,若闰7月则返回8,八月返回9,若不存在则返回 0;
        /// </summary>
        public int GetLeapMonth(int year)
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
        public bool IsLeapMonth(int year, int month)
        {
            int leapMonth = GetLeapMonth(year);
            return leapMonth != 0 && leapMonth == month;
        }

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        public bool IsLeapYear(int year)
        {
            /// <summary>
            /// 每几年置闰年;
            /// </summary>
            const int leapYearInteraval = 3;
            return year % leapYearInteraval == 0;
        }

        public int GetMonth(int year, int month, out bool isLeapMonth)
        {
            int leapMonth = GetLeapMonth(year);
            if (leapMonth != 0 && month >= leapMonth)
            {
                isLeapMonth = leapMonth == month;
                month--;
                return month;
            }
            isLeapMonth = false;
            return month;
        }

        public int GetChineseZodiac(int year)
        {
            return year % 12;
        }
    }
}
