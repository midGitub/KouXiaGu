using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.Concurrent;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形烘培器,在Unity线程进行烘培任务;
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

        [SerializeField]
        LandformChunkRenderer rectLandformChunk;

        IWorld world;
        List<RectCoord> displayPoints;

        /// <summary>
        /// 地形请求处置器;
        /// </summary>
        public IRequestDispatcher RequestDispatcher
        {
            get { return dispatcher; }
        }

        public LandformBakeCamera BakeCamera
        {
            get { return bakeCamera; }
        }

        void Awake()
        {
            displayPoints = new List<RectCoord>();
        }

        public LandformChunkRenderer CreateChunk(RectCoord chunkPos)
        {
            Vector3 pos = chunkPos.ToLandformChunkPixel();
            return Instantiate(rectLandformChunk, pos, Quaternion.identity);
        }

        public void UpdateChunk(RectCoord chunkPos, LandformChunkRenderer landformChunk)
        {
            Debug.Log("Update:" + chunkPos.ToString());
        }

        public void DestroyChunk(LandformChunkRenderer landformChunk)
        {
            Destroy(landformChunk.gameObject);
        }

        ///// <summary>
        ///// 烘培对应地形块;
        ///// </summary>
        //void Bake(RectCoord chunkPos , LandformChunkRenderer landformChunk)
        //{
        //    displayPoints.Clear();
        //    displayPoints.AddRange(LandformInfo.GetChildren(chunkPos));

        //    throw new NotImplementedException();
        //}
    }
}
