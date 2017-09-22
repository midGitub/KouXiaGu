using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Grids
{

    /// <summary>
    /// 范围;
    /// </summary>
    public interface IRange<TPoint>
    {
        /// <summary>
        /// 若超出范围则返回false;
        /// </summary>
        bool IsOutRange(TPoint point);
    }

}
