using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Map.Navigation;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 为游戏实现的导航地图;
    /// </summary>
    public class NaviMap : MonoBehaviour, INavMap<NavNode, Navigator>
    {
        private NaviMap() { }

        private HexMap<NavNode> navMap;


        #region INavigationMap<TMover>

        IEnumerable<KeyValuePair<IntVector2, NavNode>> INavMap<NavNode, Navigator>.GetAround(IntVector2 position)
        {
            return navMap.GetAround(position);
        }

        NavNode INavMap<NavNode, Navigator>.GetAt(IntVector2 position)
        {
            NavNode navNode;
            if (navMap.TryGetValue(position, out navNode))
                return navNode;
            else
                throw new KeyNotFoundException("尝试获取到不存在的节点!" + position.ToString());
        }

        #endregion

    }

}
