using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 寻路障碍物;
    /// </summary>
    public class Obstruction : UnitySingleton<Obstruction>, IObstructive<WorldNode, NavAction>
    {

        public bool CanWalk(NavAction mover, WorldNode item)
        {
            throw new NotImplementedException();
        }

        public float GetCost(NavAction mover, ShortVector2 currentPoint, WorldNode currentNode, ShortVector2 destination)
        {
            throw new NotImplementedException();
        }

    }

}
