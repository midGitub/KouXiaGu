using System;
using System.Linq;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地图数据提供;
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
    public sealed class TerrainChunk : MonoBehaviour
    {
        TerrainChunk() { }

        static Shader TerrainShader
        {
            get { return TerrainData.TerrainShader; }
        }

        static Shader HeightShader
        {
            get { return TerrainData.HeightShader; }
        }


        #region 地形块大小(静态)

        /// <summary>
        /// 地图节点所使用的六边形参数;
        /// </summary>
        static Hexagon MAP_HEXAGON
        {
            get { return TerrainConvert.hexagon; }
        }
        
        /// <summary>
        /// 地形块大小(需要大于或等于2);
        /// </summary>
        public static readonly int CHUNK_SIZE = 3;

        /// <summary>
        /// 地形块宽度;
        /// </summary>
        public static readonly float CHUNK_WIDTH = (float)(MAP_HEXAGON.OuterDiameters * 1.5f * (CHUNK_SIZE - 1));
        public static readonly float CHUNK_WIDTH_HALF = CHUNK_WIDTH / 2;

        /// <summary>
        /// 地形块高度;
        /// </summary>
        public static readonly float CHUNK_HEIGHT = (float)MAP_HEXAGON.InnerDiameters * CHUNK_SIZE;
        public static readonly float CHUNK_HEIGHT_HALF = CHUNK_HEIGHT / 2;

        static readonly RectGrid CHUNK_GRID = new RectGrid(CHUNK_WIDTH, CHUNK_HEIGHT);
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
        /// 获取到这个地形块覆盖到的所有地图节点坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetChunkCover(RectCoord coord)
        {
            CubicHexCoord hexCenter = GetHexCenter(coord);
            CubicHexCoord startCoord = CubicHexCoord.GetDirectionOffset(HexDirections.Southwest) * CHUNK_SIZE + hexCenter + CubicHexCoord.DIR_South;

            for (short endX = (short)(Math.Abs(hexCenter.X - startCoord.X) + hexCenter.X);
                startCoord.X <= endX;
                startCoord += (startCoord.X & 1) == 0 ?
                (((CHUNK_SIZE & 1) == 0) ? CubicHexCoord.DIR_Northeast : CubicHexCoord.DIR_Southeast) :
                (((CHUNK_SIZE & 1) == 0) ? CubicHexCoord.DIR_Southeast : CubicHexCoord.DIR_Northeast))
            {
                CubicHexCoord startRow = startCoord;
                for (short endY = (short)(Math.Abs(hexCenter.Z - startCoord.Z) + hexCenter.Y);
                    startRow.Y <= endY;
                    startRow += CubicHexCoord.DIR_North)
                {
                    yield return startRow;
                }
            }
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


        #region 地形块网格(静态)

        const string MESH_NAME = "Terrain Mesh";

        //为了地形相接的地方不存在明显的缝隙,所以加上 小数 的数值;
        static readonly float MESH_HALF_WIDTH = CHUNK_WIDTH_HALF + 0.0005f;
        static readonly float MESH_HALF_HEIGHT = CHUNK_HEIGHT_HALF + 0.0005f;

        /// <summary>
        /// 网格生成的高度;
        /// </summary>
        const float ALTITUDE = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        internal static readonly Vector3[] VERTICES = new Vector3[]
            {
                new Vector3(-MESH_HALF_WIDTH , ALTITUDE, MESH_HALF_HEIGHT),
                new Vector3(MESH_HALF_WIDTH, ALTITUDE, MESH_HALF_HEIGHT),
                new Vector3(MESH_HALF_WIDTH, ALTITUDE, -MESH_HALF_HEIGHT),
                new Vector3(-MESH_HALF_WIDTH, ALTITUDE, -MESH_HALF_HEIGHT),
            };

        /// <summary>
        /// 网格三角形数据;
        /// </summary>
        internal static readonly int[] TRIANGLES = new int[]
           {
                0,1,2,
                0,2,3,
           };

        /// <summary>
        /// 网格UV坐标数据;
        /// </summary>
        internal static readonly Vector2[] UV = new Vector2[]
           {
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f),
           };

        /// <summary>
        /// 创建一个新的地形块网格结构;
        /// </summary>
        internal static Mesh CreateTerrainMesh()
        {
            Mesh mesh = new Mesh();

            mesh.name = MESH_NAME;
            mesh.vertices = VERTICES;
            mesh.triangles = TRIANGLES;
            mesh.uv = UV;
            mesh.RecalculateNormals();

            return mesh;
        }

        static Mesh terrainMesh;

        /// <summary>
        /// 获取到公共使用的地形块网格结构;
        /// </summary>
        static Mesh GetTerrainMesh()
        {
            return terrainMesh ?? (terrainMesh = CreateTerrainMesh());
        }

        #endregion


        #region 地形块(实例)

        RectCoord coord;
        Material material;
        Texture2D heightTexture;
        Texture2D diffuseTexture;
        Texture2D normalMap;

        /// <summary>
        /// 地形块坐标;
        /// </summary>
        public RectCoord Coord
        {
            get { return coord; }
            private set { transform.position = ChunkGrid.GetCenter(value); coord = value; }
        }

        /// <summary>
        /// 正在使用的材质;
        /// </summary>
        Material Material
        {
            get { return material ?? (material = new Material(TerrainShader)); }
        }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseTexture
        {
            get { return diffuseTexture; }
            set { Material.SetTexture("_MainTex", value); diffuseTexture = value; }
        }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightTexture
        {
            get { return heightTexture; }
            set { Material.SetTexture("_HeightTex", value); heightTexture = value; }
        }

        /// <summary>
        /// 法线贴图;
        /// </summary>
        public Texture2D NormalMap
        {
            get { return normalMap; }
            set { Material.SetTexture("_NormalMap", value); normalMap = value; }
        }

        void Awake()
        {
            GetComponent<MeshFilter>().mesh = GetTerrainMesh();
            GetComponent<MeshRenderer>().material = Material;
        }

        /// <summary>
        /// 设置参数到地形块;
        /// </summary>
        public void SetChunk(RectCoord coord, TerrainTexPack tex)
        {
            Coord = coord;
            DiffuseTexture = tex.diffuseMap;
            HeightTexture = tex.heightMap;
            NormalMap = tex.normalMap;
        }

        /// <summary>
        /// 清空贴图引用,但是不销毁;
        /// </summary>
        public void ClearTextures()
        {
            Coord = RectCoord.Self;
            DiffuseTexture = null;
            HeightTexture = null;
            NormalMap = null;
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public void DestroyTextures()
        {
            Coord = RectCoord.Self;
            Destroy(DiffuseTexture);
            Destroy(HeightTexture);
            Destroy(NormalMap);
        }

        void Reset()
        {
            GetComponent<MeshFilter>().mesh = CreateTerrainMesh();
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer.material == null)
                renderer.material = Material;
        }

        [ContextMenu("保存贴图")]
        void SAVE_Tex()
        {
            string path = Application.dataPath + "\\TestTex";

            diffuseTexture.SavePNG(path);
            heightTexture.SavePNG(path);
            normalMap.SavePNG(path);
        }

        [ContextMenu("显示地形模式")]
        void TerrainDisplay()
        {
            Material.shader = TerrainShader;

            DiffuseTexture = diffuseTexture;
            HeightTexture = heightTexture;
        }

        [ContextMenu("显示高度模式")]
        void HeightDisplay()
        {
            Material.shader = HeightShader;
        }

        #endregion

    }

}
