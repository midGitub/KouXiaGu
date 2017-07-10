using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XLua;

namespace KouXiaGu.World.TimeSystem
{

    /// <summary>
    /// 日历;
    /// </summary>
    public class Calendar : ICalendar
    {

        internal static readonly IReadOnlyList<MonthType> MonthsArray = new MonthType[]
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
            }.AsReadOnlyList();

        internal static readonly IReadOnlyList<ChineseZodiac> ZodiacArray = new ChineseZodiac[]
            {
                ChineseZodiac.Mouse,
                ChineseZodiac.Ox,
                ChineseZodiac.Tiger,
                ChineseZodiac.Rabbit,
                ChineseZodiac.Dragon,
                ChineseZodiac.Snake,
                ChineseZodiac.Horse,
                ChineseZodiac.Sheep,
                ChineseZodiac.Monkey,
                ChineseZodiac.Rooster,
                ChineseZodiac.Dog,
                ChineseZodiac.Pig,
            }.AsReadOnlyList();

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
