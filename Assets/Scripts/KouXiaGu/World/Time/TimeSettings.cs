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
        /// 小时长度;多少次 FixedUpdate 为一小时;
        /// </summary>
        [Range(1, 100)]
        public float HourLength = 30;

        /// <summary>
        /// 时间缩放比例,默认为1;
        /// </summary>
        [Range(0.1f, 5)]
        public float TimeScale = 1;
    }
}
