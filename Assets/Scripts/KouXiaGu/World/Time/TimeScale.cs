using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏时间缩放;
    /// </summary>
    [Serializable]
    public class TimeScale
    {

        TimeScale()
        {
        }

        public TimeScale(float[] timeScales, int timeScaleIndex)
        {
            SetSpeedLevel(timeScales, timeScaleIndex);
        }


        /// <summary>
        /// 可用的时间流逝速度;
        /// </summary>
        [SerializeField]
        float[] timeScales;

        /// <summary>
        /// 现在使用的时间流逝速度;
        /// </summary>
        [SerializeField]
        int timeScaleIndex;


        /// <summary>
        /// 可用的时间流逝速度;
        /// </summary>
        public float[] TimeScales
        {
            get { return timeScales; }
            private set { timeScales = value; }
        }

        /// <summary>
        /// 现在使用的时间流逝速度;
        /// </summary>
        public int TimeScaleIndex
        {
            get { return timeScaleIndex; }
            private set { timeScaleIndex = value; }
        }

        /// <summary>
        /// 当前指向的时间缩放;
        /// </summary>
        public float CurrentTimeScale
        {
            get { return timeScales[timeScaleIndex]; }
        }

        /// <summary>
        /// 当前执行的时间缩放;
        /// </summary>
        public float Scale
        {
            get { return Time.timeScale; }
            private set { Time.timeScale = value; }
        }


        public void OnValidate()
        {
            this.Scale = CurrentTimeScale;
        }

        /// <summary>
        /// 设置时间流逝速度;
        /// </summary>
        /// <param name="timeScales">新的流逝级别</param>
        /// <param name="timeScaleIndex">流逝级别的数组下标</param>
        public void SetSpeedLevel(float[] timeScales, int timeScaleIndex)
        {
            if (timeScales == null)
                throw new ArgumentNullException();

            this.TimeScales = (float[])timeScales.Clone();
            SetSpeedLevel(timeScaleIndex);
        }

        /// <summary>
        /// 设置时间流逝速度;
        /// </summary>
        /// <param name="timeScaleIndex">流逝级别的数组下标</param>
        public void SetSpeedLevel(int timeScaleIndex)
        {
            if (timeScaleIndex < 0 || timeScaleIndex >= this.timeScales.Length)
                throw new IndexOutOfRangeException();

            this.TimeScaleIndex = timeScaleIndex;
            OnValidate();
        }

    }

}
