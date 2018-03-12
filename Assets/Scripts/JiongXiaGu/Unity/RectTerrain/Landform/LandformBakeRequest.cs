using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Unity.Maps;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{


    public class LandformBakeRequest
    {
        /// <summary>
        /// 地形坐标;
        /// </summary>
        public RectCoord ChunkCoord { get; private set; }

        /// <summary>
        /// 需要烘培的节点,和其内容;
        /// </summary>
        public List<LandformBakeNode> BakePoints { get; private set; }

        internal LandformBakeRequest(RectCoord chunkCoord, List<LandformBakeNode> bakePoints)
        {
            ChunkCoord = chunkCoord;
            BakePoints = bakePoints;
        }
    }

    public struct LandformBakeNode
    {
        public RectCoord Position { get; internal set; }
        public NodeLandformInfo Node { get; internal set; }
        public LandformTextures Res { get; internal set; }
    }
}
