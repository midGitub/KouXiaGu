using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 不限制范围;
    /// </summary>
    public sealed class UnlimitedRange<T> : IRange<T>
    {
        UnlimitedRange() { }

        static readonly UnlimitedRange<T> instance = new UnlimitedRange<T>();

        /// <summary>
        /// 默认的实例;
        /// </summary>
        public static UnlimitedRange<T> Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 永远返回false;
        /// </summary>
        public bool IsOutRange(T point)
        {
            return false;
        }

    }

}
