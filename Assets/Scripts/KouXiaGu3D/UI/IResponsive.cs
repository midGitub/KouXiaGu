using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.UI
{

    public interface IResponsive
    {

        /// <summary>
        /// 重置到初始设置;
        /// </summary>
        void OnReset();

        /// <summary>
        /// 进行变更;
        /// </summary>
        void OnApply();

    }

}
