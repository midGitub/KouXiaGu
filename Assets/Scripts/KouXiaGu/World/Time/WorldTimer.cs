using System;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 时间记录;
    /// </summary>
    public class WorldTimer : MonoBehaviour, IObservable<DateTime>
    {

        WorldTimer()
        {
        }


        public bool IsRunning
        {
            get { return enabled; }
            set { enabled = value; }
        }


        /// <summary>
        /// 时间信息;
        /// </summary>
        DateTime currentTime;

        /// <summary>
        /// 完整的时间信息;
        /// </summary>
        public DateTime CurrentDateTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }

        /// <summary>
        /// 时间信息;
        /// </summary>
        public SimplifiedDateTime CurrentSimplifiedDateTime
        {
            get { return new SimplifiedDateTime(currentTime); }
        }


        byte time;

        [SerializeField, Range(0, 200)]
        byte timeLenght = 50;


        void Awake()
        {
            time = 0;
            trigger = new Trigger<DateTime>();
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
                Triggering();
            }
        }


        /// <summary>
        /// 时间触发器;
        /// </summary>
        Trigger<DateTime> trigger;

        /// <summary>
        /// 当时间发生变化时推送;
        /// </summary>
        public IDisposable Subscribe(IObserver<DateTime> observer)
        {
            return this.trigger.Subscribe(observer);
        }

        /// <summary>
        /// 立即推送当前时间到观察者;
        /// </summary>
        public void Triggering()
        {
            trigger.Triggering(currentTime);
        }

        /// <summary>
        /// 设置为新的时间,并且推送到所有观察者;
        /// </summary>
        public void SetCurrentDateTime(DateTime currentDateTime)
        {
            currentTime = currentDateTime;
            Triggering();
        }

    }

}
