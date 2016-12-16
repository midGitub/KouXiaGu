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

        /// <summary>
        /// 地形地图;
        /// </summary>
        public static IMap<CubicHexCoord, TerrainNode> terrainMap
        {
            get { return TerrainController.ActivatedMap; }
        }

        /// <summary>
        /// 正在队列等待烘焙的地形块;
        /// </summary>
        static readonly HashSet<RectCoord> onQueueChunk = new HashSet<RectCoord>();

        /// <summary>
        /// 创建地形到场景,若已经在场景则返回false;
        /// </summary>
        public static bool Create(RectCoord chunkCoord)
        {
            if (IsCreated(chunkCoord))
                return false;

            onQueueChunk.Add(chunkCoord);
            BasicRenderer.Enqueue(new BakeRequest(terrainMap, chunkCoord));
            return true;
        }

        /// <summary>
        /// 从场景移除地形,若不存在这个地形则返回false;
        /// </summary>
        public static bool Destroy(RectCoord chunkCoord)
        {
            if (!IsCreated(chunkCoord))
                return false;

            onQueueChunk.Remove(chunkCoord);
            RemoveOnScene(chunkCoord);
            return true;
        }

        /// <summary>
        /// 从场景移除地形;
        /// </summary>
        static void RemoveOnScene(RectCoord chunkCoord)
        {
            TerrainData.Destroy(chunkCoord);
        }

        /// <summary>
        /// 是否已经创建到场景?
        /// </summary>
        static bool IsCreated(RectCoord chunkCoord)
        {
            return TerrainData.Contains(chunkCoord) || onQueueChunk.Contains(chunkCoord);
        }

        /// <summary>
        /// 是否为空的地形块;
        /// </summary>
        static bool IsEmtpyChunk(RectCoord chunkCoord)
        {
            IEnumerable<CubicHexCoord> cover = TerrainData.GetChunkCover(chunkCoord);
            try
            {
                cover.First();
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }


        /// <summary>
        /// 创建地形到场景;
        /// </summary>
        static void Create(RectCoord chunkCoord, Texture2D diffuse, Texture2D height)
        {
            if (!onQueueChunk.Contains(chunkCoord))
            {
                Destroy(diffuse);
                Destroy(height);
                return;
            }

            TerrainData.Create(chunkCoord, diffuse, height);
        }

        struct BakeRequest : IBakeRequest<BakingNode>
        {

            BakingNode[] bakingNodes;
            Vector3 cameraPosition;

            public RectCoord ChunkCoord { get; private set; }

            /// <summary>
            /// 范围内请求烘焙的节点;
            /// </summary>
            IEnumerable<BakingNode> IBakeRequest<BakingNode>.BakingNodes
            {
                get { return bakingNodes; }
            }

            /// <summary>
            /// 摄像机位置;
            /// </summary>
            Vector3 IBakeRequest<BakingNode>.CameraPosition
            {
                get { return cameraPosition; }
            }

            public BakeRequest(IMap<CubicHexCoord, TerrainNode> map, RectCoord chunkCoord) : this()
            {
                this.ChunkCoord = chunkCoord;
                this.cameraPosition = TerrainData.ChunkGrid.GetCenter(chunkCoord);
                this.bakingNodes = GetBakingNodes(map, chunkCoord).ToArray();

                if (bakingNodes.Length == 0)
                    throw new IndexOutOfRangeException("这个块坐标已经超出了地图的定义;");
            }

            /// <summary>
            /// 获取到这次需要烘焙的所有节点;
            /// </summary>
            IEnumerable<BakingNode> GetBakingNodes(IMap<CubicHexCoord, TerrainNode> map, RectCoord blockCoord)
            {
                IEnumerable<CubicHexCoord> cover = TerrainData.GetChunkCover(blockCoord);
                TerrainNode node;
                Vector3 pixPoint;

                foreach (var coord in cover)
                {
                    pixPoint = GridConvert.Grid.GetPixel(coord);
                    if (map.TryGetValue(coord, out node))
                    {
                        yield return new BakingNode(pixPoint, node);
                    }
                    else
                    {
                        yield return default(BakingNode);
                    }
                }
            }

            /// <summary>
            /// 基本的贴图烘焙完毕时调用;
            /// </summary>
            void IBakeRequest<BakingNode>.TextureComplete(Texture2D diffuse, Texture2D height)
            {
                TerrainCreater.Create(ChunkCoord, diffuse, height);
            }

        }

        #endregion


        #region 实例

        TerrainCreater() { }

        /// <summary>
        /// 最小显示半径,在这个半径内的地形块会创建并显示;
        /// </summary>
        [SerializeField]
        RectCoord minRadius;

        /// <summary>
        /// 最大缓存半径,超出这个半径的地形块贴图将会销毁;
        /// </summary>
        [SerializeField]
        RectCoord maxRadius;

        BreadthTraversal breadthTraversal;

        /// <summary>
        /// 中心点;
        /// </summary>
        Vector3 position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        void Awake()
        {
            breadthTraversal = new BreadthTraversal();
        }

        void Update()
        {
            RectCoord center = TerrainData.ChunkGrid.GetCoord(position);
            IEnumerable<RectCoord> pp = GetDisplayCoords(center);

            foreach (var item in pp)
            {
                Create(item);
            }
        }

        /// <summary>
        /// 获取到需要显示到场景的坐标;
        /// </summary>
        IEnumerable<RectCoord> GetDisplayCoords(RectCoord center)
        {
            RectCoord southwest = center - minRadius;
            RectCoord northeast = center + minRadius;

            Func<RectCoord, bool> IsOutMinRange = delegate (RectCoord coord)
            {
                return coord.x < southwest.x || coord.x > northeast.x ||
                        coord.y < southwest.y || coord.y > northeast.y;
            };

            return breadthTraversal.Traversal(center, IsOutMinRange);
        }

        #endregion

    }

}
