using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// 当计数超过最大计数值时重置计数;
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
