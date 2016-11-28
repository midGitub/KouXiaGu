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
    public class Obstruction : UnitySingleton<Obstruction>, IObstructive<WorldNode, INavAction>
    {

        TopographiessData topographiessData;

        void Awake()
        {
            topographiessData = TopographiessData.GetInstance;
        }


        public bool CanWalk(INavAction mover, WorldNode item)
        {
            TopographyInfo topographyInfo = topographiessData.GetInfoWithID(item.TopographyID);
            return topographyInfo.CanWalk;
        }

        public float GetCost(INavAction mover, ShortVector2 currentPoint, WorldNode currentNode, ShortVector2 destination)
        {
            float cost = 0;
            TopographyInfo topographyInfo = topographiessData.GetInfoWithID(currentNode.TopographyID);
            cost += topographyInfo.ActionCost;
            cost += (ShortVector2.Distance(currentPoint, destination) * 3);
            return cost;
        }

    }

}
