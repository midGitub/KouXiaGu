using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    /// <summary>
    /// 游戏时间缩放;
    /// </summary>
    [Serializable]
    public class GameTimeScale
    {
        GameTimeScale()
        {
        }

        /// <summary>
        /// 可用的时间流逝速度;
        /// </summary>
        [SerializeField]
        float[] timeScales;

        /// <summary>
        /// 现在使用的时间流逝速度;
        /// </summary>
        [SerializeField]
        int timeScaleIndex;


        /// <summary>
        /// 可用的时间流逝速度;
        /// </summary>
        public float[] TimeScales
        {
            get { return timeScales; }
            private set { timeScales = value; }
        }

        /// <summary>
        /// 现在使用的时间流逝速度;
        /// </summary>
        public int TimeScaleIndex
        {
            get { return timeScaleIndex; }
            private set { timeScaleIndex = value; }
        }

        /// <summary>
        /// 当前时间缩放;
        /// </summary>
        public float CurrentTimeScale
        {
            get { return timeScales[timeScaleIndex]; }
        }


        /// <summary>
        /// 设置时间流逝速度;
        /// </summary>
        /// <param name="timeScales">新的流逝级别</param>
        /// <param name="timeScaleIndex">流逝级别的数组下标</param>
        public void SetSpeedLevel(float[] timeScales, int timeScaleIndex)
        {
            if (timeScales == null)
                throw new ArgumentNullException();

            this.TimeScales = (float[])timeScales.Clone();
            SetSpeedLevel(timeScaleIndex);
        }

        /// <summary>
        /// 设置时间流逝速度;
        /// </summary>
        /// <param name="timeScaleIndex">流逝级别的数组下标</param>
        public void SetSpeedLevel(int timeScaleIndex)
        {
            if (timeScaleIndex < 0 || timeScaleIndex >= this.timeScales.Length)
                throw new IndexOutOfRangeException();

            this.TimeScaleIndex = timeScaleIndex;
        }

    }


    /// <summary>
    /// 十二生肖;
    /// </summary>
    public enum ChineseZodiac
    {
        Mouse,
        Ox,
        Tiger,
        Rabbit,
        Dragon,
        Snake,
        Horse,
        Sheep,
        Monkey,
        Rooster,
        Dog,
        Pig
    }


    /// <summary>
    /// 游戏世界时间;
    /// </summary>
    [Serializable]
    public struct WorldTime : IEquatable<WorldTime>, IComparable<WorldTime>
    {

        public WorldTime(int year, int month, int day) : this()
        {
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
        public WorldTime AddDay()
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
        WorldTime AddMonth()
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
        WorldTime AddYear()
        {
            Year++;
            return this;
        }

        public bool IsLeapMonth()
        {
            return IsLeapMonth(Year, Month);
        }


        public int CompareTo(WorldTime other)
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
            if (!(obj is WorldTime))
                return false;
            return this.Equals((WorldTime)obj);
        }

        public bool Equals(WorldTime other)
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
