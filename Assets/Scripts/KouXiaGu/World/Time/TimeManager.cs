using System;
using System.IO;
using KouXiaGu.Rx;
using UnityEngine;
using XLua;

namespace KouXiaGu.World
{

    /// <summary>
    /// 世界时间初始化信息;
    /// </summary>
    public struct WorldTimeInfo
    {
        /// <summary>
        /// 开始时间;
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 当前时间;
        /// </summary>
        public DateTime CurrentTime { get; set; }

        /// <summary>
        /// 时间间隔长度;
        /// </summary>
        public int HourIntervalLenght { get; set; }

    }

    /// <summary>
    /// 时间记录;
    /// </summary>
    [DisallowMultipleComponent]
    public class TimeManager : MonoBehaviour
    {
        TimeManager()
        {
        }

        /// <summary>
        /// 时间信息;
        /// </summary>
        DateTime currentTime;

        /// <summary>
        /// 完整的时间信息;
        /// </summary>
        public DateTime CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }

        /// <summary>
        /// 当时间的 小时数 发生变化时推送;
        /// </summary>
        public Tracker<DateTime> TimeTracker { get; private set; }

        /// <summary>
        /// 当前分钟缓存变量;
        /// </summary>
        int currenMinute;

        /// <summary>
        /// 时间间隔长度;
        /// </summary>
        [SerializeField, Range(0, 200)]
        int hourInterval = 50;

        /// <summary>
        /// 时间间隔长度;
        /// </summary>
        public int HourInterval
        {
            get { return hourInterval; }
            private set { hourInterval = value; }
        }

        /// <summary>
        /// 时间信息缓存;
        /// </summary>
        WorldTimeInfo tempInfo;

        /// <summary>
        /// 时间信息缓存;
        /// </summary>
        public WorldTimeInfo TempInfo
        { 
            get { return tempInfo; }
            private set { tempInfo = value; }
        }

        /// <summary>
        /// 设定时间是否更新;
        /// </summary>
        public bool IsRunning
        {
            get { return enabled; }
            set { enabled = value; }
        }

        void Awake()
        {
            IsRunning = false;
            TimeTracker = new ListTracker<DateTime>();
        }

        /// <summary>
        /// 每次更新增加一小时;
        /// </summary>
        void FixedUpdate()
        {
            currenMinute++;
            if (currenMinute > hourInterval)
            {
                currenMinute = 0;
                currentTime.AddHour();
                TrackTime();
            }
        }

        /// <summary>
        /// 立即推送当前时间到观察者;
        /// </summary>
        void TrackTime()
        {
            TimeTracker.Track(currentTime);
        }

        /// <summary>
        /// 根据信息初始化类;
        /// </summary>
        /// <param name="info"></param>
        public void Initialize(WorldTimeInfo info)
        {
            this.tempInfo = info;
            currentTime = info.CurrentTime;
            hourInterval = info.HourIntervalLenght;
        }

        /// <summary>
        /// 获取到当前的时间信息;
        /// </summary>
        public WorldTimeInfo GetInfo()
        {
            tempInfo.CurrentTime = currentTime;
            tempInfo.HourIntervalLenght = hourInterval;
            return tempInfo;
        }


        [CSharpCallLua]
        public delegate ICalendar CalendarCreater();

        /// <summary>
        /// 从Lua文件获取到日历信息;
        /// </summary>
        public void LoadCalendarFromLua()
        {
            LuaEnv luaenv = LuaManager.Luaenv;
            CalendarCreater creater = luaenv.Global.GetInPath<CalendarCreater>("Calendar.New");
            ICalendar calendar = creater();
            DateTime.CurrentCalendar = calendar;
        }

        void Start()
        {
            LoadCalendarFromLua();
        }

    }


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
