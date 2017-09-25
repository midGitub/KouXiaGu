using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    [Serializable]
    public class LandformChunkPool : GameObjectPool<LandformChunkRenderer>
    {
        public override void ResetWhenEnterPool(LandformChunkRenderer item)
        {
            base.ResetWhenEnterPool(item);
            item.Reset();
        }
    }
}
