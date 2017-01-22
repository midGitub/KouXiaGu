using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形创建控制;在 LateUpdate 把这个周期需要创建的地形块进行渲染;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainCreater : SceneSington<TerrainCreater>
    {

        static TerrainCreater()
        {
            AllowCreation = false;
            Activation = false;
        }

        TerrainCreater() { }

        /// <summary>
        /// 地形地图;
        /// </summary>
        static MapData Data
        {
            get { return MapDataManager.ActiveData; }
        }



        /// <summary>
        /// 正在请求渲染 的地图块;
        /// </summary>
        public readonly static HashSet<RectCoord> onRenderingChunks = new HashSet<RectCoord>();

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        static readonly CustomDictionary<RectCoord, TerrainChunk> activatedChunks = new CustomDictionary<RectCoord, TerrainChunk>();

        /// <summary>
        /// 休眠的地形块;
        /// </summary>
        static readonly Queue<TerrainChunk> restingChunks = new Queue<TerrainChunk>();

        /// <summary>
        /// 是否允许创建地形到场景?
        /// </summary>
        public static bool AllowCreation { get; set; }

        /// <summary>
        /// 当前组件是否为激活状态?
        /// </summary>
        public static bool Activation { get; private set; }


        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        public static IReadOnlyDictionary<RectCoord, TerrainChunk> ActivatedChunks
        {
            get { return activatedChunks; }
        }

        /// <summary>
        /// 正在请求渲染 的地图块;
        /// </summary>
        public static HashSet<RectCoord> ReadOnlyOnRenderingChunks
        {
            get { return onRenderingChunks; }
        }

        /// <summary>
        /// 重置的\休眠的地形块数目;
        /// </summary>
        public static int RestingChunkCount
        {
            get { return restingChunks.Count; }
        }


        /// <summary>
        /// 创建地形块到场景,若已经存在,则更新其贴图;
        /// </summary>
        static TerrainChunk CreateOrUpdate(RectCoord coord, TerrainTexPack tex)
        {
            TerrainChunk chunk;

            if (activatedChunks.TryGetValue(coord, out chunk))
            {
                chunk.Set(tex);
            }
            else
            {
                chunk = GetTerrainChunk();
                chunk.Set(coord, tex);
                activatedChunks.Add(coord, chunk);
            }
            return chunk;
        }

        /// <summary>
        /// 从池内获取到或者实例化一个;
        /// </summary>
        static TerrainChunk GetTerrainChunk()
        {
            TerrainChunk terrainChunk;
            if (restingChunks.Count > 0)
            {
                terrainChunk = DequeueTerrainChunk();
            }
            else
            {
                terrainChunk = new TerrainChunk();
            }
            return terrainChunk;
        }

        /// <summary>
        /// 从对象池获取到地形块;
        /// </summary>
        static TerrainChunk DequeueTerrainChunk()
        {
            var terrainChunk = restingChunks.Dequeue();
            terrainChunk.SetActive(true);
            return terrainChunk;
        }

        /// <summary>
        /// 释放这个地形块的使用权,将地形块放回池内,备下次使用,若不存在则返回false;
        /// </summary>
        static bool Release(RectCoord coord)
        {
            TerrainChunk terrainChunk;
            if (activatedChunks.TryGetValue(coord, out terrainChunk))
            {
                terrainChunk.Reset();

                terrainChunk.SetActive(false);
                restingChunks.Enqueue(terrainChunk);
                activatedChunks.Remove(coord);
                return true;
            }
            return false;
        }





        /// <summary>
        /// 激活功能;
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Activation = true;
        }


        /// <summary>
        /// 将这个地形块加入到创建队列,若创建,则不重复创建;
        /// </summary>
        public static void CreateChunk(RectCoord coord)
        {
            if (AllowCreation &&
                !activatedChunks.ContainsKey(coord) &&
                onRenderingChunks.Add(coord))
            {
                TerrainBaker.Requested.AddLast(new RenderRequest(coord));
            }
        }

        /// <summary>
        /// 将这个地形块加入到创建队列,若已经创建了,则重新创建;
        /// </summary>
        public static void UpdateChunk(RectCoord coord)
        {
            if (AllowCreation &&
                activatedChunks.ContainsKey(coord) &&
                onRenderingChunks.Add(coord))
            {
                TerrainBaker.Requested.AddLast(new RenderRequest(coord));
            }
        }

        /// <summary>
        /// 销毁这个地形块,若不存在则返回false;
        /// </summary>
        public static bool DestroyChunk(RectCoord coord)
        {
            if (Release(coord))
            {
                return true;
            }

            if (onRenderingChunks.Remove(coord))
            {
                TerrainBaker.Requested.Remove(item => item.ChunkCoord == coord);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 销毁所有该场景的引用;
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            activatedChunks.Clear();
            onRenderingChunks.Clear();
            restingChunks.Clear();

            Activation = false;
        }


        class RenderRequest : IBakeRequest
        {

            public RenderRequest(RectCoord chunkCoord)
            {
                this.ChunkCoord = chunkCoord;
            }

            public RectCoord ChunkCoord { get; private set; }

            public MapData Data
            {
                get { return TerrainCreater.Data; }
            }

            public void OnComplete(TerrainTexPack tex)
            {
                if (!Activation)
                {
                    tex.Destroy();
                }
                else
                {
                    onRenderingChunks.Remove(ChunkCoord);
                    CreateOrUpdate(ChunkCoord, tex);
                }
            }

            public void OnError(Exception ex)
            {
                Debug.LogError(ex);
            }

        }

    }

}
