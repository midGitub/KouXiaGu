using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 地图编辑拓展;
    /// </summary>
    public static class MapEdit
    {

        /// <summary>
        /// 获取到这个点保存的信息;
        /// </summary>
        public static WorldNode GetWorldNode(this IHexMap<ShortVector2, WorldNode> map, ShortVector2 position)
        {
            return map[position];
        }

        /// <summary>
        /// 设置这个点保存的信息;
        /// </summary>
        public static void SetWorldNode(this IHexMap<ShortVector2, WorldNode> map, ShortVector2 position, WorldNode node)
        {
            map[position] = node;
        }


    }

}
