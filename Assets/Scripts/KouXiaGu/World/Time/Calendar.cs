using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XLua;

namespace KouXiaGu.World
{

    /// <summary>
    /// 日历接口;
    /// </summary>
    [CSharpCallLua]
    public interface ICalendar
    {
        /// <summary>
        /// 这一个月存在的天数; 0 ~ max;
        /// </summary>
        int GetDaysInMonth(int year, int month);

        /// <summary>
        /// 这一年存在的月数; 1 ~ max;
        /// </summary>
        int GetMonthsInYear(int year);

        /// <summary>
        /// 这个月是否为闰月?
        /// </summary>
        bool IsLeapMonth(int year, int month);

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        bool IsLeapYear(int year);

        /// <summary>
        /// 获取到枚举类型的月份表示;
        /// </summary>
        MonthType GetMonthType(int year, int month, out bool isLeapMonth);
    }


    /// <summary>
    /// 测试使用的日历;
    /// </summary>
    public class Calendar : ICalendar
    {
        public Calendar()
        {
        }

        public int GetDaysInMonth(int year, int month)
        {
            return 30;
        }

        public int GetMonthsInYear(int year)
        {
            return 12;
        }

        public bool IsLeapMonth(int year, int month)
        {
            return IsLeapYear(year) && month == GetLeapMonth(year);
        }

        int GetLeapMonth(int year)
        {
            return 6;
        }

        public bool IsLeapYear(int year)
        {
            return year % 3 == 0;
        }

        static readonly MonthType[] MonthsArray = new MonthType[]
            {
                MonthType.January,
                MonthType.February,
                MonthType.March,
                MonthType.April,
                MonthType.May,
                MonthType.June,
                MonthType.July,
                MonthType.August,
                MonthType.September,
                MonthType.October,
                MonthType.November,
                MonthType.December,
            };

        public MonthType GetMonthType(int year, int month, out bool isLeapMonth)
        {
            int leapMonth = GetLeapMonth(year);

            if (leapMonth != 0 && month >= leapMonth)
            {
                isLeapMonth = leapMonth == month;
                month--;
                return MonthsArray[month];
            }

            isLeapMonth = false;
            return MonthsArray[month];
        }

    }


    public class CalendarFormLuaScript
    {

        [CSharpCallLua]
        public delegate ICalendar CalendarReader();

        internal const string luaScriptName = "Calendar.New";

        /// <summary>
        /// 从Lua文件获取到日历信息;
        /// </summary>
        public static ICalendar Read()
        {
            const string errorString = "无法从Lua获取到日历信息;";

            LuaEnv luaenv = LuaManager.Luaenv;
            CalendarReader creater = luaenv.Global.GetInPath<CalendarReader>(luaScriptName);
            if (creater == null)
                throw new ArgumentException(errorString);

            ICalendar calendar = creater();
            if (calendar == null)
                throw new ArgumentException(errorString);

            return calendar;
        }
    }

    /// <summary>
    /// 游戏使用的日历;
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


        /// <summary>
        /// 这一个月的天数; 0 ~ max;
        /// </summary>
        public int GetDaysInMonth(int year, int month)
        {
            int leapMonth = GetLeapMonth(year);

            if (leapMonth != 0 && month >= leapMonth)
            {
                month--;
            }

            return _dayOfMonth[month];
        }

        /// <summary>
        /// 这一年的月数; 1 ~ max;
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
            const int LEAP_YEAR_INTERAVAL = 3;

            return year % LEAP_YEAR_INTERAVAL == 0;
        }


        static readonly MonthType[] MonthsArray = new MonthType[]
            {
                MonthType.January,
                MonthType.February,
                MonthType.March,
                MonthType.April,
                MonthType.May,
                MonthType.June,
                MonthType.July,
                MonthType.August,
                MonthType.September,
                MonthType.October,
                MonthType.November,
                MonthType.December,
            };

        /// <summary>
        /// 获取到枚举类型的月份表示;
        /// </summary>
        public MonthType GetMonthType(int year, int month, out bool isLeapMonth)
        {
            int leapMonth = GetLeapMonth(year);

            if (leapMonth != 0 && month >= leapMonth)
            {
                isLeapMonth = leapMonth == month;
                month--;
                return MonthsArray[month];
            }

            isLeapMonth = false;
            return MonthsArray[month];
        }

    }


}
