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
}
