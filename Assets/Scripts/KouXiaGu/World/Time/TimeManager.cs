using System;
using KouXiaGu.Rx;
using UnityEngine;

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
        /// 当前间隔时间缓存变量;
        /// </summary>
        int hourInterval;

        /// <summary>
        /// 时间间隔长度;
        /// </summary>
        [SerializeField, Range(0, 200)]
        int hourIntervalLenght = 50;

        /// <summary>
        /// 时间间隔长度;
        /// </summary>
        public int HourIntervalLenght
        {
            get { return hourIntervalLenght; }
            private set { hourIntervalLenght = value; }
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
            hourInterval++;
            if (hourInterval > hourIntervalLenght)
            {
                hourInterval = 0;
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
            hourIntervalLenght = info.HourIntervalLenght;
        }

        /// <summary>
        /// 获取到当前的时间信息;
        /// </summary>
        public WorldTimeInfo GetInfo()
        {
            tempInfo.CurrentTime = currentTime;
            tempInfo.HourIntervalLenght = hourIntervalLenght;
            return tempInfo;
        }

    }

}
