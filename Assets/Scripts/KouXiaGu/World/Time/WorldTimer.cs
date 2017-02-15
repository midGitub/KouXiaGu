using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{


    [DisallowMultipleComponent]
    public class WorldTimer : MonoBehaviour
    {
        WorldTimer()
        {
        }


        [SerializeField, Range(1, 200)]
        short lengthOfDay = 120;

        [SerializeField]
        WorldDateTime time;

        short dayTime;


        public bool IsRuning
        {
            get { return enabled; }
            set { enabled = value; }
        }
        

        /// <summary>
        /// 一天的长度;
        /// </summary>
        public short LenghtOfDay
        {
            get { return lengthOfDay; }
            set { lengthOfDay = value; }
        }

        /// <summary>
        /// 当前的时间;
        /// </summary>
        public WorldDateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        void Awake()
        {
            dayTime = 0;
        }

        void FixedUpdate()
        {
            dayTime++;
            if (dayTime > lengthOfDay)
            {
                dayTime = 0;
                Time = Time.AddDay();
            }
        }

    }

}
