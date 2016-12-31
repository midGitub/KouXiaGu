using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 寻路范围;
    /// </summary>
    public interface INavigationRange<TPoint>
    {
        /// <summary>
        /// 若超出范围则返回false;
        /// </summary>
        bool IsOutRange(TPoint point);
    }

}
