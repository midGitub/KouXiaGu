﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public interface IBakeRequest
    {
        RectCoord ChunkCoord { get; }
        ChunkTexture Textures { get; }
        BakeTargets Targets { get; }
        bool IsCanceled { get; }

        void AddBakeQueue();
        void StartBake();
        void BakeCompleted();
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
        BakeLandform bakeLandform = null;
        [SerializeField]
        BakeRoad bakeRoad = null;
        [SerializeField]
        Stopwatch runtimeStopwatch = null;
        IWorldData worldData;
        Coroutine bakeCoroutine;
        Queue<IBakeRequest> requestQueue;

        public int RequestCount
        {
            get { return requestQueue == null ? 0 : requestQueue.Count; }
        }

        public bool IsEmpty
        {
            get { return RequestCount == 0; }
        }

        void Awake()
        {
            bakeCamera.Initialize();
            bakeLandform.Initialize();
            bakeRoad.Initialise();
            requestQueue = new Queue<IBakeRequest>();
            bakeCoroutine = new Coroutine(BakeCoroutine());
        }

        [SerializeField]
        int waitUpdate = 80;
        int currentUpdateTime;

        void Update()
        {
            if (currentUpdateTime < waitUpdate)
            {
                currentUpdateTime++;
                return;
            }
            else
            {
                currentUpdateTime = 0;
            }

            runtimeStopwatch.Restart();
            while (requestQueue.Count != 0 
                && !runtimeStopwatch.Await() 
                && bakeCoroutine.MoveNext())
                continue;
        }

        void Reset()
        {
            bakeCamera.Settings.UpdataTextureSize();
        }

        IEnumerator BakeCoroutine()
        {
            while (true)
            {
                IBakeRequest bakeRequest = requestQueue.Peek();

                if (bakeRequest.IsCanceled)
                    goto Complete;

                bakeRequest.StartBake();
                BakeTargets targets = bakeRequest.Targets;
                CubicHexCoord chunkCenter = bakeRequest.ChunkCoord.GetChunkHexCenter();

                if ((targets & BakeTargets.Landform) > 0)
                {
                    yield return bakeLandform.BakeCoroutine(bakeCamera, worldData, chunkCenter);
                    var diffuseMap = bakeCamera.GetDiffuseTexture(bakeLandform.DiffuseRT);
                    var heightMap = bakeCamera.GetHeightTexture(bakeLandform.HeightRT);
                    bakeRequest.Textures.SetDiffuseMap(diffuseMap);
                    bakeRequest.Textures.SetHeightMap(heightMap);
                }

                if ((targets & BakeTargets.Road) > 0)
                {
                    yield return bakeRoad.BakeCoroutine(bakeCamera, worldData, chunkCenter);
                    var roadDiffuseMap = bakeCamera.GetDiffuseTexture(bakeRoad.DiffuseRT, TextureFormat.ARGB32);
                    var roadHeightMap = bakeCamera.GetHeightTexture(bakeRoad.HeightRT);
                    bakeRequest.Textures.SetRoadDiffuseMap(roadDiffuseMap);
                    bakeRequest.Textures.SetRoadHeightMap(roadHeightMap);
                }

                bakeRequest.BakeCompleted();
                Complete:
                requestQueue.Dequeue();
                yield return null;
            }
        }

        public void AddRequest(IBakeRequest request)
        {
            request.AddBakeQueue();
            requestQueue.Enqueue(request);
        }

        [ContextMenu("Log")]
        void Log()
        {
            Debug.Log(typeof(LandformBaker).Name + ";RequestCount:" + RequestCount);
        }

    }

}
