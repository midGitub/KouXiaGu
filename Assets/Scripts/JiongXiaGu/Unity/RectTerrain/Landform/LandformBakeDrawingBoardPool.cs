using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    [Serializable]
    public class LandformBakeDrawingBoardPool : GameObjectPool<LandformBakeDrawingBoardRenderer>
    {
        public override void ResetWhenEnterPool(LandformBakeDrawingBoardRenderer item)
        {
        }

        public override void ResetWhenOutPool(LandformBakeDrawingBoardRenderer item)
        {
        }
    }
}
