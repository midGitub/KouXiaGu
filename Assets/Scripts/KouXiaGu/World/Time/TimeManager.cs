using System;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 时间记录;
    /// </summary>
    [DisallowMultipleComponent]
    public class TimeManager : MonoBehaviour, IObservable<DateTime>
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
        /// 当前间隔时间缓存变量;
        /// </summary>
        byte time;

        /// <summary>
        /// 时间间隔长度;
        /// </summary>
        [SerializeField, Range(0, 200)]
        byte timeLenght = 50;

        /// <summary>
        /// 观察者;
        /// </summary>
        Tracker<DateTime> tracker;

        /// <summary>
        /// 时间是否在更新中?
        /// </summary>
        public bool IsRunning
        {
            get { return enabled; }
            set { enabled = value; }
        }


        void Awake()
        {
            time = 0;
            tracker = new ListTracker<DateTime>();
        }

        /// <summary>
        /// 每次更新增加一小时;
        /// </summary>
        void FixedUpdate()
        {
            time++;
            if (time > timeLenght)
            {
                time = 0;
                currentTime.AddHour();
                TrackTime();
            }
        }

        /// <summary>
        /// 当时间的 小时数 发生变化时推送;
        /// </summary>
        public IDisposable Subscribe(IObserver<DateTime> observer)
        {
            return this.tracker.Subscribe(observer);
        }

        /// <summary>
        /// 立即推送当前时间到观察者;
        /// </summary>
        public void TrackTime()
        {
            tracker.Track(currentTime);
        }

        /// <summary>
        /// 设置为新的时间,并且推送到所有观察者;
        /// </summary>
        public void SetCurrentTime(DateTime time)
        {
            this.currentTime = time;
            TrackTime();
        }


        /// <summary>
        /// 时间信息缓存;
        /// </summary>
        WorldTimeInfo info;

        /// <summary>
        /// 根据信息初始化类;
        /// </summary>
        /// <param name="info"></param>
        public void Initialize(WorldTimeInfo info)
        {
            this.info = info;
            currentTime = info.CurrentTime;
        }

        /// <summary>
        /// 获取到当前的时间信息;
        /// </summary>
        public WorldTimeInfo GetInfo()
        {
            info.CurrentTime = currentTime;
            return info;
        }

    }

}
