using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public interface IBakeRequest : IAsyncOperation
    {
        RectCoord ChunkCoord { get; }
        ChunkTexture Textures { get; }

        void OnCompleted();
        void OnFaulted(Exception ex);
    }

    /// <summary>
    /// 地形烘培;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LandformBaker : MonoBehaviour
    {

        public static LandformBaker Initialize(IWorldData worldData)
        {
            var item = SceneObject.GetObject<LandformBaker>();
            item.worldData = worldData;
            return item;
        }

        /// <summary>
        /// 透明的黑色颜色;
        /// </summary>
        public static readonly Color BlackTransparent = new Color(0, 0, 0, 0);

        /// <summary>
        /// 地平线颜色;
        /// </summary>
        public static readonly Color Horizon = new Color(0.5f, 0.5f, 0.5f, 1);

        LandformBaker()
        {
        }

        [SerializeField]
        BakeCamera bakeCamera = null;
        [SerializeField]
        BakeLandform landform = null;
        [SerializeField]
        Stopwatch runtimeStopwatch = null;
        IWorldData worldData;
        Coroutine bakeCoroutine;
        Queue<IBakeRequest> requestQueue;

        internal BakeLandform Landform
        {
            get { return landform; }
        }

        void Awake()
        {
            bakeCamera.Initialize();
            requestQueue = new Queue<IBakeRequest>();
            bakeCoroutine = new Coroutine(BakeCoroutine());
        }

        void Update()
        {
            runtimeStopwatch.Restart();
            while (!runtimeStopwatch.Await() && requestQueue.Count != 0 && bakeCoroutine.MoveNext())
                continue;
        }

        public void Reset()
        {
            bakeCamera.Settings.UpdataTextureSize();
        }

        IEnumerator BakeCoroutine()
        {
            while (true)
            {
                IBakeRequest bakeRequest = requestQueue.Dequeue();

                if (bakeRequest.IsCompleted)
                {
                    bakeRequest.OnFaulted(new Exception("已经完成;"));
                    continue;
                }

                CubicHexCoord chunkCenter = bakeRequest.ChunkCoord.GetChunkHexCenter();
                yield return landform.BakeCoroutine(bakeCamera, worldData, chunkCenter);

                var diffuseMap = bakeCamera.GetDiffuseTexture(landform.DiffuseRT);
                var heightMap = bakeCamera.GetHeightTexture(landform.HeightRT);

                bakeRequest.Textures.SetDiffuseMap(diffuseMap);
                bakeRequest.Textures.SetHeightMap(heightMap);
                bakeRequest.OnCompleted();
            }
        }

        public void AddRequest(IBakeRequest request)
        {
            requestQueue.Enqueue(request);
        }

    }

}
