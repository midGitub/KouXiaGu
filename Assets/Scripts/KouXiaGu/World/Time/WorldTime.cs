using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{

    public class WorldTimeInfo
    {
        public DateTime CurrentTime { get; set; }
    }


    /// <summary>
    /// 线程安全;
    /// </summary>
    public class WorldTime
    {
        public WorldTime(IWorldData data)
        {
            InitCalendar();
        }

        object asyncLock = new object();
        DateTime currentTime;

        /// <summary>
        /// 当前时间;
        /// </summary>
        public DateTime CurrentTime
        {
            get { return currentTime; }
        }

        public void SetCurrentTime(DateTime time)
        {
            lock (asyncLock)
            {
                currentTime = time;
            }
        }

        void InitCalendar()
        {
            try
            {
                ICalendar Calendar = CalendarFormLuaScript.Read();
                DateTime.CurrentCalendar = Calendar;
            }
            catch (Exception ex)
            {
                Debug.LogError("无法从Lua文件获取到日历信息:" + ex);
            }
        }
    }

    /// <summary>
    /// 时间更新器;
    /// </summary>
    public class WorldTimeUpdater : IUnityThreadBehaviour<Action>, IDisposable
    {
        public WorldTimeUpdater(WorldTime time)
        {
            if (time == null)
                throw new ArgumentNullException("time");

            timeManager = time;
            settings = TimeSettings.Instance;
        }

        WorldTime timeManager;
        TimeSettings settings;
        IDisposable updateCanceler;
        float currenMinute;

        public bool IsTimeUpdating
        {
            get { return updateCanceler != null; }
        }

        object IUnityThreadBehaviour<Action>.Sender
        {
            get { return "时间更新器"; }
        }

        Action IUnityThreadBehaviour<Action>.Action
        {
            get { return Next; }
        }

        void Next()
        {
            currenMinute += settings.TimeScale;
            if (currenMinute >= settings.HourLength)
            {
                currenMinute -= settings.HourLength;
                DateTime time = timeManager.CurrentTime;
                time = time.AddHour();
                timeManager.SetCurrentTime(time);
            }
        }

        public void Start()
        {
            if (updateCanceler == null)
            {
                updateCanceler = UnityThreadDispatcher.Instance.SubscribeFixedUpdate(this);
            }
        }

        public void Stop()
        {
            if (updateCanceler != null)
            {
                updateCanceler.Dispose();
                updateCanceler = null;
            }
        }

        void IDisposable.Dispose()
        {
            Stop();
        }
    }
}
