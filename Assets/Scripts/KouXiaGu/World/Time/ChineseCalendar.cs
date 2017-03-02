using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{


    public class ChineseCalendar
    {

        /// <summary>
        /// 月份天数; 354天
        /// </summary>
        static readonly int[] _dayOfMonth = new int[]
            {
                30, // 1
                29,
                30, // 3
                29,
                30, // 5
                29,
                30, // 7
                29,
                30, // 9
                29, 
                30, // 11
                29,
            };

        /// <summary>
        /// 每几年置闰年;
        /// </summary>
        const int LEAP_YEAR_INTERAVAL = 3;

        /// <summary>
        /// 闰月分配表,需要不能被三整除的容量(推荐14, 17);
        /// 14 的顺序: 0 3 6 9 12 1 4 7 10 13 2 5 8 11 ...
        /// 17 的顺序: 0 3 6 9 12 15 1 4 7 10 13 16 2 5 8 11 14 ...
        /// </summary>
        static readonly int[] _leapMonthDistribution = new int[]
            {
                6,  // 1
                4,
                10, // 3
                6,
                5, // 5
                3,
                8, // 7
                4,
                5, // 9
                2,
                7, // 11
                5,
                4, // 13
                9,
                6, // 14
            };


        /// <summary>
        /// 获取到这一年的天数;
        /// </summary>
        public int GetDayOfYear(int year)
        {
            int day = 0;
            for (int month = GetMonthsInYear(year); month > 0; month--)
            {
                day += GetDayOfMonth(year, month);
            }
            return day;
        }

        /// <summary>
        /// 获取到月份天数;
        /// </summary>
        public int GetDayOfMonth(int year, int month)
        {
            int leapMonth = GetLeapMonth(year);

            if (leapMonth > 0)
            {
                if (month > leapMonth)
                {
                    return GetDayOfMonth(month - 1);
                }
                else
                {
                    return GetDayOfMonth(month);
                }
            }
            else
            {
                return GetDayOfMonth(month);
            }
        }

        /// <summary>
        /// 获取到这一年闰几月,若不存在则返回 -1;
        /// </summary>
        public int GetLeapMonth(int year)
        {
            if (IsLeapYear(year))
            {
                int seed = year % _leapMonthDistribution.Length;
                return _leapMonthDistribution[seed];
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取到月份天数(不包括闰月);
        /// </summary>
        int GetDayOfMonth(int month)
        {
            if (month > 12 || month <= 0)
                throw new ArgumentOutOfRangeException();

            return _dayOfMonth[--month];
        }

        /// <summary>
        /// 获取到这一年的月数;
        /// </summary>
        public int GetMonthsInYear(int year)
        {
            /// <summary>
            /// 闰年月数;
            /// </summary>
            const int LEAP_YEAR_MONTH_COUNT = 13;

            /// <summary>
            /// 非闰年月数;
            /// </summary>
            const int NOT_LEAP_YEAR_MONTH_COUNT = 12;

            return IsLeapYear(year) ? LEAP_YEAR_MONTH_COUNT : NOT_LEAP_YEAR_MONTH_COUNT;
        }

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        public bool IsLeapYear(int year)
        {
            return year % LEAP_YEAR_INTERAVAL == 0;
        }

        /// <summary>
        /// 这个月是否为闰月?
        /// </summary>
        public bool IsLeapMonth(int year, int month)
        {
            int leapMonth = GetLeapMonth(year);
            return leapMonth != -1 && (leapMonth + 1) == month;
        }

    }

}
