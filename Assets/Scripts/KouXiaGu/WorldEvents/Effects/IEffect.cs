using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 产生效果的;
    /// </summary>
    public interface IEffect
    {

        /// <summary>
        /// 是否生效中?
        /// </summary>
        bool Effective { get; }


        /// <summary>
        /// 启用;
        /// </summary>
        void Enable();

        /// <summary>
        /// 停用;
        /// </summary>
        void Disable();

    }

}
