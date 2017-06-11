using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{


    public class TimeSettings : SceneSington<TimeSettings>
    {

        /// <summary>
        /// 分钟长度;多少次 FixedUpdate 为一分钟;
        /// </summary>
        [Range(1, 200)]
        public int MinuteLength = 10;

        /// <summary>
        /// 时间缩放比例,默认为1;
        /// </summary>
        [Range(0.1f, 5)]
        public float TimeScale = 1;
    }
}
