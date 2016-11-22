using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 地形类型;
    /// </summary>
    [Obsolete]
    public enum TerrainTypes
    {
        /// <summary>
        /// 陆地,允许行走到的区域;
        /// </summary>
        Land,
        /// <summary>
        /// 山区,不允许行走到的区域;
        /// </summary>
        Mountain,
        /// <summary>
        /// 河流,江;不允许直接行走到的区域;
        /// </summary>
        Rivers,
        /// <summary>
        /// 海域;不允许直接行走到的区域;
        /// </summary>
        Sea,
    }

}
