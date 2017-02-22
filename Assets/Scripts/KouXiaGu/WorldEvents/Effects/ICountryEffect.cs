using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 对于国家的效果;
    /// </summary>
    public interface ICountryEffect
    {

        /// <summary>
        /// 启用;
        /// </summary>
        void Enable(Country country);

        /// <summary>
        /// 停用;
        /// </summary>
        void Disable(Country country);

    }

}
