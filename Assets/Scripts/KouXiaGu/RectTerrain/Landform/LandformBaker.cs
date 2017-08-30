using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    [Serializable]
    public class LandformBaker
    {

        [SerializeField]
        LandformBakeDrawingBoardRenderer drawingBoardPrefab = null;

        IWorld world;
        LandformBakeCamera bakeCamera;
        List<RectCoord> displayPoints;

        public void Initialize(IWorld world, LandformBakeCamera bakeCamera)
        {
            this.world = world;
            this.bakeCamera = bakeCamera;
            displayPoints = new List<RectCoord>();
        }

        /// <summary>
        /// 烘培对应地形块;
        /// </summary>
        void Bake(RectCoord chunkPos , LandformChunkRenderer landformChunk)
        {
            displayPoints.Clear();
            displayPoints.AddRange(LandformInfo.GetChildren(chunkPos));

            throw new NotImplementedException();
        }
    }
}
