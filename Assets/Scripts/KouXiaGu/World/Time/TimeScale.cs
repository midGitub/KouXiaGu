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

        static readonly float[] DEFAULT_TIME_SCALES_ARRAY = new float[]
        {
            1,
            1.5f,
            2,
        };

        static readonly int DEFAULT_TIME_SCALE_INDEX = 0;
        const float PAUSE_TIME_SCALE = 0f;


        TimeScale()
        {
        }

        public TimeScale(float[] timeScales, int timeScaleIndex) : this()
        {
            SetTimeScale(timeScales, timeScaleIndex);
        }


        /// <summary>
        /// 可用的时间流逝速度;
        /// </summary>
        [SerializeField]
        float[] timeScalesArray;

        /// <summary>
        /// 现在使用的时间流逝速度;
        /// </summary>
        [SerializeField]
        int timeScaleIndex;


        /// <summary>
        /// 可用的时间流逝速度;
        /// </summary>
        public float[] TimeScalesArray
        {
            get { return timeScalesArray; }
            private set { timeScalesArray = value; }
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
            get { return timeScalesArray[timeScaleIndex]; }
        }

        /// <summary>
        /// 当前执行的时间缩放;
        /// </summary>
        public float Scale
        {
            get { return Time.timeScale; }
            private set { Time.timeScale = value; }
        }

        /// <summary>
        /// 是否被暂停了?
        /// </summary>
        public bool IsPause
        {
            get { return Scale == PAUSE_TIME_SCALE; }
        }


        /// <summary>
        /// 恢复到默认;
        /// </summary>
        public void Reset()
        {
            SetTimeScale(DEFAULT_TIME_SCALES_ARRAY, DEFAULT_TIME_SCALE_INDEX);
        }

        /// <summary>
        /// 重新设置到时间缩放级别;
        /// </summary>
        public void OnValidate()
        {
            SetTimeScale(timeScaleIndex);
        }


        /// <summary>
        /// 设置时间流逝速度;
        /// </summary>
        /// <param name="timeScales">新的流逝级别</param>
        /// <param name="timeScaleIndex">流逝级别的数组下标</param>
        public void SetTimeScale(float[] timeScales, int timeScaleIndex)
        {
            if (timeScales == null)
                throw new ArgumentNullException();

            this.TimeScalesArray = (float[])timeScales.Clone();
            SetTimeScale(timeScaleIndex);
        }

        /// <summary>
        /// 设置时间流逝速度;
        /// </summary>
        /// <param name="timeScaleIndex">流逝级别的数组下标</param>
        public void SetTimeScale(int timeScaleIndex)
        {
            this.TimeScaleIndex = timeScaleIndex;
            this.Scale = CurrentTimeScale;
        }

        /// <summary>
        /// 设置到上一个缩放级别;
        /// 若已经到头了则返回false;
        /// </summary>
        public bool Previous()
        {
            if (this.timeScaleIndex > 0)
            {
                SetTimeScale(this.timeScaleIndex - 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置到下一个缩放级别;
        /// 若已经到头了则返回false;
        /// </summary>
        public bool Next()
        {
            if (this.timeScaleIndex < timeScalesArray.Length)
            {
                SetTimeScale(this.timeScaleIndex + 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 暂停;
        /// </summary>
        public void Pause()
        {
            Scale = PAUSE_TIME_SCALE;
        }

    }

}
