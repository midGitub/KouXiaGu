using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 提供寻路路径;
    /// </summary>
    [DisallowMultipleComponent]
    public class NavController : UnitySingleton<NavController>
    {

        AStar<WorldNode, NavAction> astarNav;

        void Start()
        {
            Obstruction obstruction = Obstruction.GetInstance;
            IMap<ShortVector2, WorldNode> worldMap = WorldMapData.GetInstance.Map;
            astarNav = new AStar<WorldNode, NavAction>(obstruction, worldMap);
        }

        /// <summary>
        /// 返回到达这儿的路径;
        /// </summary>
        public NavPath FreeToGo(ShortVector2 starting, ShortVector2 destination, ShortVector2 maximumRange)
        {
            try
            {
                LinkedList<ShortVector2> path = astarNav.Start(starting, destination, maximumRange);
                astarNav.Clear();
                return new NavPath(path, WorldMapData.GetInstance.Map, TopographiessData.GetInstance);
            }
            finally
            {
                astarNav.Clear();
            }
        }



    }

}
