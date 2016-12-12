using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 提供寻路方法
    /// </summary>
    [DisallowMultipleComponent]
    public class NavController : UnitySingleton<NavController>
    {

        Obstruction obstruction;
        IHexMap<RectCoord, WorldNode> worldMap;

        void Start()
        {
            obstruction = Obstruction.GetInstance;
            worldMap = WorldMapData.GetInstance.Map;
        }

        /// <summary>
        /// 获取到导航路径;
        /// </summary>
        public NavPath NavigateTo(RectCoord starting, RectCoord destination, RectCoord maximumRange, INavAction mover)
        {
            AStar<WorldNode, INavAction> astarNav = new AStar<WorldNode, INavAction>(obstruction, worldMap);
            LinkedList<RectCoord> path = astarNav.Start(starting, destination, maximumRange, mover);
            astarNav.Clear();
            return new NavPath(path, WorldMapData.GetInstance.Map, TopographiessData.GetInstance);
        }



    }

}
