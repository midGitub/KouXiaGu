using System;
using KouXiaGu.Grids;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块实例;
    /// </summary>
    [Obsolete]
    public sealed class OLandformChunk
    {

        #region 地形块大小(静态)

        /// <summary>
        /// 地图节点所使用的六边形参数;
        /// </summary>
        static Hexagon MAP_HEXAGON
        {
            get { return TerrainConvert.hexagon; }
        }

        /// <summary>
        /// 地形块大小(需要大于或等于2),不能随意变更;
        /// </summary>
        public static readonly int CHUNK_SIZE = 3;

        /// <summary>
        /// 地形块宽度;
        /// </summary>
        public static readonly float ChunkWidth = (float)(MAP_HEXAGON.OuterDiameters * 1.5f * (CHUNK_SIZE - 1));
        public static readonly float CHUNK_WIDTH_HALF = ChunkWidth / 2;

        /// <summary>
        /// 地形块高度;
        /// </summary>
        public static readonly float ChunkHeight = (float)MAP_HEXAGON.InnerDiameters * CHUNK_SIZE;
        public static readonly float CHUNK_HEIGHT_HALF = ChunkHeight / 2;

        static readonly RectGrid CHUNK_GRID = new RectGrid(ChunkWidth, ChunkHeight);
        /// <summary>
        /// 矩形网格结构,用于地形块的排列;
        /// </summary>
        public static RectGrid ChunkGrid
        {
            get { return CHUNK_GRID; }
        }

        /// <summary>
        /// 地形块坐标 获取到其中心的六边形坐标;
        /// </summary>
        public static CubicHexCoord GetHexCenter(RectCoord coord)
        {
            Vector3 pixelCenter = ChunkGrid.GetCenter(coord);
            return TerrainConvert.Grid.GetCubic(pixelCenter);
        }

        /// <summary>
        /// 获取到地图节点所属的地形块;
        /// </summary>
        public static RectCoord[] GetBelongChunks(CubicHexCoord coord)
        {
            RectCoord[] chunks = new RectCoord[2];
            GetBelongChunks(coord, ref chunks);
            return chunks;
        }

        /// <summary>
        /// 获取到地图节点所属的地形块;
        /// 传入数组容量需要大于或者等于2,所属的地形块编号放置在 0 和 1 下标处;
        /// </summary>
        public static void GetBelongChunks(CubicHexCoord coord, ref RectCoord[] chunks)
        {
            Vector3 point = TerrainConvert.Grid.GetPixel(coord);
            GetBelongChunks(point, ref chunks);
        }

        static readonly Vector3 CheckBelongChunkPoint =
            new Vector3((float)MAP_HEXAGON.OuterRadius / 2, 0, (float)MAP_HEXAGON.InnerRadius / 2);

        /// <summary>
        /// 获取到地图节点所属的地形块;
        /// 传入数组容量需要大于或者等于2,所属的地形块编号放置在 0 和 1 下标处;
        /// </summary>
        static void GetBelongChunks(Vector3 pointCenter, ref RectCoord[] chunks)
        {
            try
            {
                Vector3 point1 = pointCenter + CheckBelongChunkPoint;
                chunks[0] = CHUNK_GRID.GetCoord(point1);

                Vector3 point2 = pointCenter - CheckBelongChunkPoint;
                chunks[1] = CHUNK_GRID.GetCoord(point2);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        #endregion

        /// <summary>
        /// 地形块挂载的脚本;
        /// </summary>
        static readonly Type[] TERRAIN_CHUNK_SCRIPTS = new Type[]
            {
                typeof(OLandformRenderer),    //渲染;
                typeof(OLandformMesh),        //地形网格;
                typeof(OLandformTrigger),     //地形碰撞器;
            };


#if UNITY_EDITOR
        [MenuItem("GameObject/Create Other/LandformChunk")]
#endif
        static void _CraeteTerrainChunk()
        {
            new GameObject("TerrainChunk", TERRAIN_CHUNK_SCRIPTS);
        }

        /// <summary>
        /// 实例一个地形块,并指定名称;
        /// </summary>
        static GameObject CraeteTerrainChunk()
        {
            GameObject gameObject = new GameObject("TerrainChunk", TERRAIN_CHUNK_SCRIPTS);
#if UNITY_EDITOR
            gameObject.transform.SetParent(ChunkParent, false);
#endif
            return gameObject;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 放置地形块的父节点;
        /// </summary>
        static Transform chunkParent;
        static Transform ChunkParent
        {
            get { return chunkParent ?? (chunkParent = new GameObject("LandformChunks").transform); }
        }
#endif

        /// <summary>
        /// 创建一个空的地图块到;
        /// </summary>
        public OLandformChunk()
        {
            ChunkObject = CraeteTerrainChunk();
            Renderer = ChunkObject.GetComponent<OLandformRenderer>();
            Trigger = ChunkObject.GetComponent<OLandformTrigger>();
        }


        public RectCoord ChunkCoord { get; private set; }
        public GameObject ChunkObject { get; private set; }
        public OLandformRenderer Renderer { get; private set; }
        public OLandformTrigger Trigger { get; private set; }

        /// <summary>
        /// 是否有效的?
        /// </summary>
        public bool IsEffective
        {
            get { return ChunkObject != null; }
        }


        /// <summary>
        /// gameObject.SetActive(value);
        /// </summary>
        public void SetActive(bool value)
        {
            ChunkObject.SetActive(value);
        }


        /// <summary>
        /// 设置地形块坐标;
        /// </summary>
        public void Set(RectCoord chunkCoord)
        {
            ChunkObject.transform.position = ChunkGrid.GetCenter(chunkCoord);
            ChunkCoord = chunkCoord;
        }

        /// <summary>
        /// 设置地形贴图信息;
        /// </summary>
        public void Set(TerrainTexPack tex)
        {
            Renderer.SetDiffuseMap(tex.diffuseMap);
            Renderer.SetHeightMap(tex.heightMap);
            Renderer.SetNormalMap(tex.normalMap);

            Trigger.ResetCollisionMesh();
        }


        public void Reset()
        {
            Renderer.DestroyTextures();
        }

        public void Destroy()
        {
            GameObject.Destroy(ChunkObject);

            ChunkObject = null;
            Renderer = null;
            Trigger = null;
        }

    }

}
