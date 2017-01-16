using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 在开始时重置;
    /// </summary>
    public interface IResetWhenDisplay
    {
        /// <summary>
        /// 重置到初始设置;
        /// </summary>
        void OnReset();
    }

}
