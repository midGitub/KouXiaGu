//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace XGame.GameRunning
//{

//    /// <summary>
//    /// 游戏时间控制;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public sealed class TimeManager : Manager<TimeManager>, IGameSave
//    {

//        /// <summary>
//        /// 一天存在的秒数;
//        /// </summary>
//        private const int _DaySecond = 24 * _HourSecond;

//        /// <summary>
//        /// 一小时存在的秒数;
//        /// </summary>
//        private const int _HourSecond = 60 * _MinuteSecond;

//        /// <summary>
//        /// 一分钟存在的秒数;
//        /// </summary>
//        private const int _MinuteSecond = 60;

//        /// <summary>
//        /// 初始化游戏调用次序;
//        /// </summary>
//        [SerializeField]
//        private CallOrder moduleType = CallOrder.Static;

//        /// <summary>
//        /// 更新秒间隔;
//        /// </summary>
//        [SerializeField]
//        private float waitForSecondsCheck = 1;

//        /// <summary>
//        /// 每个月首调用;
//        /// </summary>
//        public Action<DateTime> OnMonthEvent;

//        /// <summary>
//        /// 每年年首调用;
//        /// </summary>
//        public Action<DateTime> OnYearEvent;

//        /// <summary>
//        /// 游戏开始时的时间;
//        /// </summary>
//        private float m_StartTime;

//        /// <summary>
//        /// 游戏已经经过的时间;
//        /// </summary>
//        private float m_ElapsedTime;

//        /// <summary>
//        /// 游戏背景开始时时间;
//        /// </summary>
//        private DateTime m_startDateTime;

//        /// <summary>
//        /// 时间倍数;(仅在编辑状态确认!)
//        /// </summary>
//        private float m_TimeMultiple;

//        protected override TimeManager This
//        {
//            get { return this; }
//        }

//        /// <summary>
//        /// 时间的缩放。这可以用于减慢运动效果。
//        /// </summary>
//        public float TimeScale
//        {
//            get { return Time.timeScale; }
//            set { Time.timeScale = value; }
//        }

//        /// <summary>
//        /// 初始化次序;
//        /// </summary>
//        CallOrder ICallOrder.CallOrder
//        {
//            get { return moduleType; }
//        }

//        /// <summary>
//        /// 获取到总游戏时间,乘上时间倍数的时间;(秒)
//        /// </summary>
//        public int ElapsedSecond
//        {
//            get { return (int)((Time.time - m_StartTime + m_ElapsedTime) * m_TimeMultiple); }
//        }

//        /// <summary>
//        /// 获取到游戏进行的总时间,真实时间,单位 秒;
//        /// </summary>
//        public float ElapsedTime
//        {
//            get { return Time.time - m_StartTime + m_ElapsedTime; }
//        }

//        void IGameO.GameStart()
//        {
//            m_StartTime = Time.time;
//            m_TimeMultiple = TimeController.GetInstance.TimeMultiple;
//            StartTimeEvent();
//        }

//        IEnumerator IGameSave.OnSave(GameSaveInfo info, GameSaveData data)
//        {
//            info.ElapsedTime = ElapsedTime;
//            info.StartTime_Ticks = m_startDateTime.Ticks;
//            yield break;
//        }

//        IEnumerator IGameSave.OnLoad(GameSaveInfo info, GameSaveData data)
//        {
//            m_ElapsedTime = info.ElapsedTime;
//            m_startDateTime = new DateTime(info.StartTime_Ticks);
//            yield break;
//        }

//        IEnumerator IGameO.OnStart()
//        {
//            m_ElapsedTime = 0;
//            m_startDateTime = TimeController.GetInstance.GetStartTime();
//            yield break;
//        }

//        /// <summary>
//        /// 获取到总游戏内的时间;
//        /// </summary>
//        /// <returns></returns>
//        public DateTime GetGameTime()
//        {
//            return m_startDateTime.AddSeconds(ElapsedSecond);
//        }

//        /// <summary>
//        /// 获取到游戏时间的 时钟表示部分;
//        /// </summary>
//        /// <param name="hour"></param>
//        /// <param name="minute"></param>
//        /// <param name="second"></param>
//        public void GetGameTime(out int hour, out int minute, out int second)
//        {
//            var elapsedSecond = ElapsedSecond;
//            hour = elapsedSecond % _DaySecond / _HourSecond;
//            minute = elapsedSecond % _HourSecond / _MinuteSecond;
//            second = elapsedSecond % _MinuteSecond;
//        }

//        /// <summary>
//        /// 获取到时间的文字表示;
//        /// </summary>
//        /// <returns></returns>
//        public string GetGameTimeString()
//        {
//            int hour;
//            int minute;
//            int second;
//            GetGameTime(out hour, out minute, out second);
//            return hour + " : " + minute + " : " + second;
//        }

//        /// <summary>
//        /// 开始时间事件更新协程;
//        /// </summary>
//        private void StartTimeEvent()
//        {
//            var waitForSeconds = new WaitForSeconds(waitForSecondsCheck);
//            StartCoroutine(TimeEvent_Coroutine(waitForSeconds));
//        }

//        /// <summary>
//        /// 事件更新协程;
//        /// </summary>
//        /// <returns></returns>
//        IEnumerator TimeEvent_Coroutine(WaitForSeconds waitForSeconds)
//        {
//            DateTime lastTime = GetGameTime();
//            DateTime dateTime;

//            while (true)
//            {
//                dateTime = GetGameTime();

//                if (dateTime.Month != lastTime.Month /*&& dateTime.Hour == 0 && dateTime.Minute ==0 && dateTime.Second == 0*/)
//                {
//                    if (OnMonthEvent != null)
//                        OnMonthEvent(dateTime);

//                    if (dateTime.Year != lastTime.Year)
//                    {
//                        if (OnYearEvent != null)
//                            OnYearEvent(dateTime);
//                    }
//                }

//                //Debug.Log(dateTime.ToShortDateString() + "  " + dateTime.ToShortTimeString());

//                yield return waitForSeconds;
//                lastTime = dateTime;
//            }
//        }


//        ///// <summary>
//        ///// 获取到这一年的总秒数;
//        ///// </summary>
//        ///// <param name="year">年份;</param>
//        ///// <returns></returns>
//        //public static int GetYearSecond(int year)
//        //{
//        //    int day = 0;
//        //    if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
//        //        day = 365;
//        //    else
//        //        day = 366;
//        //    return day * _DaySecond;
//        //}

//        ///// <summary>
//        ///// 获取到这个月份的总秒数;
//        ///// </summary>
//        ///// <param name="year">年份;</param>
//        ///// <param name="month">要获取到的月份;</param>
//        ///// <returns>这个月份的总秒数;</returns>
//        //public static int GetMonthSecond(int year, int month)
//        //{
//        //    int day = 0;
//        //    switch (month)
//        //    {
//        //        case 1:
//        //        case 3:
//        //        case 5:
//        //        case 7:
//        //        case 8:
//        //        case 10:
//        //        case 12:
//        //            day = 31;
//        //            break;
//        //        case 4:
//        //        case 6:
//        //        case 9:
//        //        case 11:
//        //            day = 30;
//        //            break;
//        //        case 2:
//        //            if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
//        //                day = 28;
//        //            else
//        //                day = 29;
//        //            break;
//        //        default:
//        //            throw new ArgumentOutOfRangeException("月份超出范围!");
//        //    }
//        //    return day * _DaySecond;
//        //}

//        ///// <summary>
//        ///// 获取到这一年的剩余天数;若为负数则为超出的天数;
//        ///// </summary>
//        ///// <param name="year">年份;</param>
//        ///// <param name="month">月份;</param>
//        ///// <param name="day">该月份的第几天;</param>
//        ///// <param name="hour"></param>
//        ///// <param name="minute"></param>
//        ///// <param name="second"></param>
//        ///// <returns></returns>
//        //public static int GetYearSecondLeft(int year, int month, int day, int hour, int minute, int second)
//        //{
//        //    int monthSecond = day * _DaySecond + hour * _HourSecond + minute * _HourSecond + second;

//        //    for (month--; month > 0; month--)
//        //    {
//        //        monthSecond += GetMonthSecond(year, month);
//        //    }
//        //    return GetYearSecond(year) - monthSecond;
//        //}

//    }

//}
