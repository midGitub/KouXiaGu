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


        static string CalendarScriptPath
        {
            get { return Path.Combine(Application.streamingAssetsPath, "Scripts/Calendar.lua"); }
        }


        [CSharpCallLua]
        public interface ICalc
        {
            int Add(int a, int b);
            int Dpp(int a, int b);
            int Mult { get; set; }
            int Number { get; set; }
        }

        [CSharpCallLua]
        public delegate ICalc CalcNew(int mult, int number);

        public void LoadCalendar()
        {
            LuaEnv luaenv = LuaManager.Luaenv;
            CalcNew calc_new = luaenv.Global.GetInPath<CalcNew>("Calc.New");
            ICalc calc1 = calc_new(10, 2);
            Debug.Log("Dpp:" + calc1.Dpp(10, 0));

            ICalc calc2 = calc_new(10, 2);
            Debug.Log("Dpp:" + calc1.Dpp(100, 0));

            Debug.Log("Add:" + calc2.Add(10, 10));
        }

        void Start()
        {
            LoadCalendar();
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
        byte GetDaysInMonth(short year, byte month);

        /// <summary>
        /// 这一年存在的月数; 1 ~ max;
        /// </summary>
        byte GetMonthsInYear(short year);

        /// <summary>
        /// 这个月是否为闰月?
        /// </summary>
        bool IsLeapMonth(short year, byte month);

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        bool IsLeapYear(short year);

        /// <summary>
        /// 获取到枚举类型的月份表示;
        /// </summary>
        Months GetMonth(short year, byte month);
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
        /// 获取到这一年的天数; 1 ~ max;
        /// </summary>
        public short GetDaysInYear(short year)
        {
            short day = 0;
            for (byte month = GetMonthsInYear(year); month > 0; month--)
            {
                day += GetDaysInMonth(year, month);
            }
            return day;
        }

        /// <summary>
        /// 这一个月的天数; 0 ~ max;
        /// </summary>
        public byte GetDaysInMonth(short year, byte month)
        {
            byte leapMonth = GetLeapMonth(year);

            if (leapMonth != 0 && month >= leapMonth)
            {
                month--;
            }

            return _dayOfMonth[month];
        }

        /// <summary>
        /// 这一年的月数; 1 ~ max;
        /// </summary>
        public byte GetMonthsInYear(short year)
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
        public byte GetLeapMonth(short year)
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
        public bool IsLeapMonth(short year, byte month)
        {
            byte leapMonth = GetLeapMonth(year);
            return leapMonth != 0 && leapMonth == month;
        }

        /// <summary>
        /// 这年是否为闰年?
        /// </summary>
        public bool IsLeapYear(short year)
        {
            /// <summary>
            /// 每几年置闰年;
            /// </summary>
            const int LEAP_YEAR_INTERAVAL = 3;

            return year % LEAP_YEAR_INTERAVAL == 0;
        }


        static readonly Months[] MonthsArray = new Months[]
            {
                Months.January,
                Months.February,
                Months.March,
                Months.April,
                Months.May,
                Months.June,
                Months.July,
                Months.August,
                Months.September,
                Months.October,
                Months.November,
                Months.December,
            };

        /// <summary>
        /// 获取到枚举类型的月份表示;
        /// </summary>
        public Months GetMonth(short year, byte month)
        {
            byte leapMonth = GetLeapMonth(year);

            if (leapMonth != 0 && month >= leapMonth)
            {
                month--;
            }

            return MonthsArray[month];
        }

    }

}
