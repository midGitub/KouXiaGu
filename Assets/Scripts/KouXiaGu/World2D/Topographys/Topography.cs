using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 地貌细节;
    /// </summary>
    public struct Topography
    {

        /// <summary>
        /// 地形唯一的表示;
        /// </summary>
        public int id;

        /// <summary>
        /// 是否允许行走到?;
        /// </summary>
        public bool CanWalk;

        /// <summary>
        /// 行动的代价值;
        /// </summary>
        public int ActionCost;

        /// <summary>
        /// 提供的资源;
        /// </summary>
        public Resource Provide;

    }

}
