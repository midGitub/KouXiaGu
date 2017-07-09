using System;
using XLua;

namespace KouXiaGu.World.TimeSystem
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
        /// 这一年存在的月数; 0 ~ max;
        /// </summary>
        int GetMonthsInYear(int year);

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        bool IsLeapYear(int year);

        /// <summary>
        /// 这个月是否为闰月?
        /// </summary>
        bool IsLeapMonth(int year, int month);



        /// <summary>
        /// 获取到实际表示月份;
        /// </summary>
        [Obsolete]
        int GetMonth(int year, int month, out bool isLeapMonth);
    }
}
