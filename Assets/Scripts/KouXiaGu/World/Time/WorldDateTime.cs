using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{


    /// <summary>
    /// 游戏世界时间;
    /// </summary>
    [Serializable]
    public struct WorldDateTime : IEquatable<WorldDateTime>, IComparable<WorldDateTime>
    {

        public WorldDateTime(int year, int month, int day)
        {
            if (year <= 0 || month <= 0 || day <= 0)
                throw new ArgumentOutOfRangeException();

            this._year = year;
            this._month = month;
            this._day = day;
        }


        [SerializeField]
        int _year;
        [SerializeField]
        int _month;
        [SerializeField]
        int _day;


        public int Year
        {
            get { return _year; }
            private set { _year = value; }
        }

        public int Month
        {
            get { return _month; }
            private set { _month = value; }
        }

        public int Day
        {
            get { return _day; }
            private set { _day = value; }
        }


        /// <summary>
        /// 增加一天,若超出这个月则增加一月;
        /// </summary>
        public WorldDateTime AddDay()
        {
            Day++;
            if (Day > GetDayOfMonth(Year, Month))
            {
                Day = 1;
                AddMonth();
            }
            return this;
        }

        /// <summary>
        /// 增加一月,若超过这年则增加一年;
        /// </summary>
        WorldDateTime AddMonth()
        {
            Month++;
            if (Month > GetMonthOfYear(Year))
            {
                Month = 1;
                AddYear();
            }
            return this;
        }

        /// <summary>
        /// 增加一年;
        /// </summary>
        WorldDateTime AddYear()
        {
            Year++;
            return this;
        }

        public bool IsLeapMonth()
        {
            return IsLeapMonth(Year, Month);
        }

        public void Reset()
        {
            _year = 1;
            _month = 1;
            _day = 1;
        }


        public int CompareTo(WorldDateTime other)
        {
            if (other.Year > Year)
                return 1;
            else if (other.Year < Year)
                return -1;
            else
            {
                if (other.Month > Month)
                    return 1;
                else if (other.Month < Month)
                    return -1;
                else
                {
                    if (other.Day > Day)
                        return 1;
                    else if (other.Day < Day)
                        return -1;
                }
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WorldDateTime))
                return false;
            return this.Equals((WorldDateTime)obj);
        }

        public bool Equals(WorldDateTime other)
        {
            return 
                Year == other.Year &&
                Month == other.Month &&
                Day == other.Day;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "[Year:" + _year + ",Month:" + _month + ",Day:" + _day + "]";
        }


        /// <summary>
        /// 获取到这一年的总天数;
        /// </summary>
        public static int GetDayOfYear(int y)
        {
            int day = 0;
            for (int m = GetMonthOfYear(y); m > 0; m--)
            {
                day += GetDayOfMonth(y, m);
            }
            return day;
        }

        /// <summary>
        /// 获取到这个月的天数;
        /// </summary>
        public static int GetDayOfMonth(int year, int month)
        {
            return 30;
        }

        /// <summary>
        /// 获取到这一年的月数;
        /// </summary>
        public static int GetMonthOfYear(int year)
        {
            const int LEAP_YEAR_MONTH_COUNT = 13;
            const int MONTH_COUNT = 12;

            return IsLeapYear(year) ? LEAP_YEAR_MONTH_COUNT : MONTH_COUNT;
        }

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        public static bool IsLeapYear(int year)
        {
            /// <summary>
            /// 每几年置闰年;
            /// </summary>
            const int LEAP_YEAR_INTERAVAL = 3;

            return year % LEAP_YEAR_INTERAVAL == 0;
        }

        /// <summary>
        /// 这个月是否为闰月?
        /// </summary>
        public static bool IsLeapMonth(int year, int month)
        {
            int leapMonth = GetLeapMonth(year);
            return leapMonth != 0 && leapMonth == month;
        }

        /// <summary>
        /// 获取到这一年的闰月,若不存在则返回0;
        /// </summary>
        public static int GetLeapMonth(int year)
        {
            return IsLeapYear(year) ? 7 : 0;
        }

    }

}
