using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 时间更新器;
    /// </summary>
    [DisallowMultipleComponent]
    public class TimeUpdater : MonoBehaviour
    {
        static bool isCreated = false;

        internal static TimeUpdater Create(TimeManager manager)
        {
            if (isCreated)
                throw new ArgumentException();

            var gameObject = new GameObject("TimeUpdater", typeof(TimeUpdater));
            var item = gameObject.GetComponent<TimeUpdater>();
            item.manager = manager;
            return item;
        }

        TimeUpdater()
        {
        }

        TimeManager manager;
        int currenMinute;

        public bool IsRunning
        {
            get { return transform != null && enabled; }
        }

        public DateTime CurrentTime
        {
            get { return manager.CurrentTime; }
        }

        public int HourInterval
        {
            get { return manager.HourInterval; }
        }

        void FixedUpdate()
        {
            currenMinute++;
            if (currenMinute > HourInterval)
            {
                currenMinute = 0;
                CurrentTime.AddHour();
                manager.TrackTime();
            }
        }

        void OnDestroy()
        {
            isCreated = false;
        }

    }

}
