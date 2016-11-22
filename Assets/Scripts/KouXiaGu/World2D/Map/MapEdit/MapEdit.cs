using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 地图编辑拓展;
    /// </summary>
    public static class MapEdit
    {

        /// <summary>
        /// 获取到这个点保存的信息;
        /// </summary>
        public static WorldNode GetWorldNode(this IMap<IntVector2, WorldNode> map, IntVector2 position)
        {
            return map[position];
        }

        /// <summary>
        /// 设置这个点保存的信息;
        /// </summary>
        public static void SetWorldNode(this IMap<IntVector2, WorldNode> map, IntVector2 position, WorldNode node)
        {
            map[position] = node;
        }


    }

}
