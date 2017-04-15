using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    public interface ISegmented
    {
        /// <summary>
        /// 若需要等待返回true,不需要等待返回false;
        /// </summary>
        bool KeepWait();
    }

    /// <summary>
    /// 秒数过后一个停顿;
    /// </summary>
    public class SegmentedTime : ISegmented
    {
        public SegmentedTime(float seconds)
        {
            this.seconds = seconds;

        }

        float seconds;
        float before;

        public void Start()
        {
            before = Time.realtimeSinceStartup;
        }

        public bool KeepWait()
        {
            return Time.realtimeSinceStartup - before < seconds;
        }

        public void Reset()
        {
            before = 0;
        }
    }

    /// <summary>
    /// 当计数超过最大计数值时等待几秒;
    /// </summary>
    public class SegmentedCounter : ISegmented
    {
        public SegmentedCounter(int maximum)
        {
            Maximum = maximum;
            Count = 0;
        }

        public int Maximum { get; set; }
        public int Count { get; private set; }

        public bool KeepWait()
        {
            if (Count > Maximum)
            {
                Count = 0;
                return true;
            }

            Count++;
            return false;
        }
    }

    /// <summary>
    /// 永远返回 false,;
    /// </summary>
    public class SegmentedBlock : ISegmented
    {
        public bool KeepWait()
        {
            return false;
        }
    }


}
