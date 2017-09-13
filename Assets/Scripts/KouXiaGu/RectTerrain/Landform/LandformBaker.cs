using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.Concurrent;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形烘培请求;
    /// </summary>
    public class LandformBakeRequest : Request
    {
        public LandformBakeRequest(LandformBaker baker)
        {
            this.baker = baker;
        }

        readonly LandformBaker baker;

        protected override void Operate()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 地形烘培器;
    /// </summary>
    [Serializable]
    public sealed class LandformBaker : MonoBehaviour
    {
        LandformBaker()
        {
        }

        [SerializeField]
        LandformDispatcher dispatcher;

        [SerializeField]
        LandformBakeCamera bakeCamera;

        [SerializeField]
        LandformBakeDrawingBoardRenderer drawingBoardPrefab = null;

        IWorld world;
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
