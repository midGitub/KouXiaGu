using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 响应设置的,在最后需要发生保存或撤销动作的;
    /// </summary>
    public interface IResponsive
    {

        /// <summary>
        /// 进行变更;
        /// </summary>
        void OnApply();

        /// <summary>
        /// 重置到初始设置;
        /// </summary>
        void OnReset();

    }

}
