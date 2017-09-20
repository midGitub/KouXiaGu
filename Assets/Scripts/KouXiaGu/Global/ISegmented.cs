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
        /// 需要等待返回true,不需要等待返回false;
        /// </summary>
        bool Await();
    }

    /// <summary>
    /// 在闲置秒数过后进行等待;
    /// </summary>
    [Serializable]
    public class Stopwatch : ISegmented
    {
        Stopwatch()
        {
        }

        public Stopwatch(float idleSeconds)
        {
            this.idleSeconds = idleSeconds;
        }

        /// <summary>
        /// 用于的执行时间;
        /// </summary>
        [SerializeField]
        float idleSeconds;
        float before;

        public void Restart()
        {
            before = Time.realtimeSinceStartup;
        }

        public bool Await()
        {
            return Time.realtimeSinceStartup - before > idleSeconds;
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

        public bool Await()
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

        public bool Await()
        {
            return false;
        }
    }


}
