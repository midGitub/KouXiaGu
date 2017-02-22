using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 对城镇启用的效果;
    /// </summary>
    public interface ITownEffect
    {

        /// <summary>
        /// 启用;
        /// </summary>
        void Enable(Town country);

        /// <summary>
        /// 停用;
        /// </summary>
        void Disable(Town country);
    }

}
