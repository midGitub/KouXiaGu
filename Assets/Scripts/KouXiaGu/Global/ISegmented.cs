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
        /// 重新开始计算;
        /// </summary>
        void Restart();

        /// <summary>
        /// 若需要等待返回true,不需要等待返回false;
        /// </summary>
        bool KeepWait();
    }

    /// <summary>
    /// 秒数过后进行停顿;
    /// </summary>
    public class SegmentedTime : ISegmented
    {
        public SegmentedTime(float seconds)
        {
            this.seconds = seconds;

        }

        float seconds;
        float before;

        public void Restart()
        {
            before = Time.realtimeSinceStartup;
        }

        public bool KeepWait()
        {
            return Time.realtimeSinceStartup - before > seconds;
        }
    }

    /// <summary>
    /// 当计数超过最大计数值时停顿;
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

        public void Restart()
        {
            Count = 0;
        }

        public bool KeepWait()
        {
            Count++;
            return Maximum < Count;
        }
    }

    /// <summary>
    /// 永远返回 false,;
    /// </summary>
    public class SegmentedFalse : ISegmented
    {
        public void Restart()
        {
            return;
        }

        public bool KeepWait()
        {
            return false;
        }
    }


}
