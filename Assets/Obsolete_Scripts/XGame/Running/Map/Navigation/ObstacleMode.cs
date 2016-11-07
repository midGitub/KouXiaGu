using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame.Running.Map.Guidances
{


    /// <summary>
    /// 障碍物状态;
    /// </summary>
    [Flags]
    public enum ObstacleMode
    {

        /// <summary>
        /// 静态的障碍物;仅允许自己移除;
        /// </summary>
        Static = 1,

        ///// <summary>
        ///// 动态障碍物;
        ///// </summary>
        //Dynamic,

        /// <summary>
        /// 不重要的障碍物,允许覆盖\隐藏;
        /// </summary>
        Unimportant = 2,

        /// <summary>
        /// 占用这个障碍物点,但是不对寻路形成障碍;
        /// </summary>
        Occupied = 4,

    }

}
