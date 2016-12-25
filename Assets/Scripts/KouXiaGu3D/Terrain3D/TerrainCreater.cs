using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形创建到场景;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainCreater : MonoBehaviour
    {


        #region 静态

        static TerrainCreater creater;

        public static void Load()
        {
            if (creater != null)
                throw new Exception("已经进行游戏?");

            creater = GameObject.FindObjectOfType<TerrainCreater>();
            creater.enabled = true;
        }

        public static void Unload()
        {
            creater.enabled = false;
            creater = null;

            onSceneChunk.Clear();
            TerrainChunk.DestroyAll();
            Renderer.Clear();
        }

        /// <summary>
        /// 地形地图;
        /// </summary>
        static IDictionary<CubicHexCoord, TerrainNode> terrainMap
        {
            get { return TerrainController.CurrentMap.Map; }
        }

        /// <summary>
        /// 请求创建到场景的块;
        /// </summary>
        static readonly HashSet<RectCoord> onSceneChunk = new HashSet<RectCoord>();

        /// <summary>
        /// 创建地形到场景,若已经在场景则返回false;
        /// </summary>
        static bool Create(RectCoord chunkCoord)
        {
            if (IsCreated(chunkCoord))
                return false;

            onSceneChunk.Add(chunkCoord);
            Renderer.BakingRequests.AddLast(new BakeRequest(terrainMap, chunkCoord));
            return true;
        }

        /// <summary>
        /// 从场景移除地形,若不存在这个地形则返回false;
        /// </summary>
        static bool Destroy(RectCoord chunkCoord)
        {
            if (!IsCreated(chunkCoord))
                return false;

            if(!TerrainChunk.Destroy(chunkCoord))
                Renderer.BakingRequests.Remove(item => item.ChunkCoord == chunkCoord);

            onSceneChunk.Remove(chunkCoord);
            RemoveOnScene(chunkCoord);
            return true;
        }

        /// <summary>
        /// 从场景移除地形;
        /// </summary>
        static void RemoveOnScene(RectCoord chunkCoord)
        {
            TerrainChunk.Destroy(chunkCoord);
        }

        /// <summary>
        /// 是否已经创建到场景?
        /// </summary>
        static bool IsCreated(RectCoord chunkCoord)
        {
            return onSceneChunk.Contains(chunkCoord);
        }

        /// <summary>
        /// 创建地形到场景;
        /// </summary>
        static void Create(RectCoord chunkCoord, Texture2D diffuse, Texture2D height, Texture2D normal)
        {
            if (!onSceneChunk.Contains(chunkCoord))
            {
                GameObject.Destroy(diffuse);
                GameObject.Destroy(height);
                return;
            }

            TerrainChunk.Create(chunkCoord, diffuse, height, normal);
        }

        #endregion


        TerrainCreater() { }

        /// <summary>
        /// x 显示半径,在这个半径内的地形块会创建并显示;
        /// y 最大半径,超出这个半径内的将会销毁;
        /// </summary>
        [SerializeField]
        RectCoord radius;

        /// <summary>
        /// 中心点;
        /// </summary>
        Vector3 position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        void Update()
        {
            RectCoord center = TerrainChunk.ChunkGrid.GetCoord(position);
            HashSet<RectCoord> displayCoords = new HashSet<RectCoord>(GetDisplayCoords(center));

            foreach (var item in displayCoords)
            {
                Create(item);
            }

            foreach (var item in onSceneChunk.ToArray())
            {
                if(!displayCoords.Contains(item))
                    Destroy(item);
            }

        }

        /// <summary>
        /// 获取到需要显示到场景的坐标;
        /// </summary>
        IEnumerable<RectCoord> GetDisplayCoords(RectCoord center)
        {
            return RectCoord.Range(center, radius.x, radius.y);
        }

        //IEnumerable<RectCoord> GetDestoryCoords(RectCoord center)
        //{
        //    return RectCoord.Range(center, radius.y);
        //}

        [ContextMenu("渲染所有")]
        void BakingAll()
        {
            foreach (var item in RectCoord.Range(RectCoord.Self, 4))
            {
                try
                {
                    Create(item);
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }

    }

}
