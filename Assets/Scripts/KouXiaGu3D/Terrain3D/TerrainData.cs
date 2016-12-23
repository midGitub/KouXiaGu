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
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode, DisallowMultipleComponent]
    public class TerrainData : MonoBehaviour
    {
        TerrainData() { }

        const string SHADER_TERRAIN_NAME = "HexTerrain/Terrain";
        const string SHADER_HEIGHT_NAME = "HexTerrain/HeigtDisplay";

        static Shader terrainShader;

        static Shader heightShader;

        static Shader TerrainShader
        {
            get { return terrainShader ?? (terrainShader = Shader.Find(SHADER_TERRAIN_NAME)); }
        }

        static Shader HeightShader
        {
            get { return heightShader ?? (heightShader = Shader.Find(SHADER_HEIGHT_NAME)); }
        }


        #region 地形块大小(静态)

        /// <summary>
        /// 地图节点所使用的六边形参数;
        /// </summary>
        static Hexagon MAP_HEXAGON
        {
            get { return GridConvert.hexagon; }
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
            return GridConvert.Grid.GetCubic(pixelCenter);
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
            Vector3 point = GridConvert.Grid.GetPixel(coord);
            GetBelongChunks(point, ref chunks);
        }

        //检测六边形节点所在块 的两个点;
        static readonly Vector3 CheckBelongChunkPoint1 =
            new Vector3((float)MAP_HEXAGON.OuterRadius / 2, 0, (float)MAP_HEXAGON.InnerRadius / 2);
        static readonly Vector3 CheckBelongChunkPoint2 =
            new Vector3(-(float)MAP_HEXAGON.OuterRadius / 2, 0, -(float)MAP_HEXAGON.InnerRadius / 2);

        /// <summary>
        /// 获取到地图节点所属的地形块;
        /// 传入数组容量需要大于或者等于2,所属的地形块编号放置在 0 和 1 下标处;
        /// </summary>
        static void GetBelongChunks(Vector3 pointCenter, ref RectCoord[] chunks)
        {
            try
            {
                Vector3 point1 = pointCenter + CheckBelongChunkPoint1;
                chunks[0] = CHUNK_GRID.GetCoord(point1);

                Vector3 point2 = pointCenter + CheckBelongChunkPoint2;
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
        static readonly float MESH_HALF_WIDTH = CHUNK_WIDTH_HALF + 0.005f;
        static readonly float MESH_HALF_HEIGHT = CHUNK_HEIGHT_HALF + 0.005f;

        /// <summary>
        /// 网格生成的高度;
        /// </summary>
        const float ALTITUDE = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        static readonly Vector3[] VERTICES = new Vector3[]
            {
                new Vector3(-MESH_HALF_WIDTH , ALTITUDE, MESH_HALF_HEIGHT),
                new Vector3(MESH_HALF_WIDTH, ALTITUDE, MESH_HALF_HEIGHT),
                new Vector3(MESH_HALF_WIDTH, ALTITUDE, -MESH_HALF_HEIGHT),
                new Vector3(-MESH_HALF_WIDTH, ALTITUDE, -MESH_HALF_HEIGHT),
            };

        /// <summary>
        /// 网格三角形数据;
        /// </summary>
        static readonly int[] TRIANGLES = new int[]
           {
                0,1,2,
                0,2,3,
           };

        /// <summary>
        /// 网格UV坐标数据;
        /// </summary>
        static readonly Vector2[] UV = new Vector2[]
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
        float tessellation;
        float displacement;

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
        /// 网格细分程度;
        /// </summary>
        public float Tessellation
        {
            get { return tessellation; }
            private set { Material.SetFloat("_Tess", value); tessellation = value; }
        }

        /// <summary>
        /// 高度位移系数;
        /// </summary>
        public float Displacement
        {
            get { return displacement; }
            private set { Material.SetFloat("_Displacement", value); displacement = value; }
        }

        void Awake()
        {
            GetComponent<MeshFilter>().mesh = GetTerrainMesh();
            GetComponent<MeshRenderer>().material = Material;
        }

        /// <summary>
        /// 清空贴图引用,但是不销毁;
        /// </summary>
        void ClearTextures()
        {
            Coord = RectCoord.Self;
            DiffuseTexture = null;
            HeightTexture = null;
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        void DestroyTextures()
        {
            Coord = RectCoord.Self;
            Destroy(DiffuseTexture);
            Destroy(HeightTexture);
        }

        [ContextMenu("初始化网格")]
        void SetMesh()
        {
            GetComponent<MeshFilter>().mesh = CreateTerrainMesh();
        }

        [ContextMenu("显示地形模式")]
        void TerrainDisplay()
        {
            Material.shader = TerrainShader;

            DiffuseTexture = diffuseTexture;
            HeightTexture = heightTexture;
            Tessellation = tessellation;
            Displacement = displacement;
        }

        [ContextMenu("显示高度模式")]
        void HeightDisplay()
        {
            Material.shader = HeightShader;
        }

        #endregion


        #region 地形块实例管理(静态)

        static float globalTessellation = 32f;
        static float globalDisplacement = 2f;

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        static Dictionary<RectCoord, TerrainData> activatedChunks = new Dictionary<RectCoord, TerrainData>();

        /// <summary>
        /// 休眠的地形块;
        /// </summary>
        static Queue<TerrainData> restingChunks = new Queue<TerrainData>();

        /// <summary>
        /// 激活在场景的地形块数目;
        /// </summary>
        public static int ActivatedChunkCount
        {
            get { return activatedChunks.Count; }
        }

        /// <summary>
        /// 重置的\休眠的地形块数目;
        /// </summary>
        public static int RestingChunkCount
        {
            get { return restingChunks.Count; }
        }

        /// <summary>
        /// 全局的网格细分程度;
        /// </summary>
        public static float GlobalTessellation
        {
            get { return globalTessellation; }
            set
            {
                globalTessellation = value;
                foreach (var chunk in activatedChunks.Values)
                {
                    chunk.Tessellation = value;
                }
            }
        }

        /// <summary>
        /// 全局的高度系数;
        /// </summary>
        public static float GlobalDisplacement
        {
            get { return globalDisplacement; }
            set
            {
                globalDisplacement = value;
                foreach (var chunk in activatedChunks.Values)
                {
                    chunk.Displacement = value;
                }
            }
        }

        /// <summary>
        /// 创建地形块到场景;
        /// </summary>
        public static TerrainData Create(RectCoord coord, Texture2D diffuse, Texture2D height)
        {
            if (activatedChunks.ContainsKey(coord))
                throw new ArgumentException("地形块已经创建到场景;");

            TerrainData terrainChunk = GetTerrainChunk(coord.ToString());

            SetChunk(terrainChunk, coord, diffuse, height);

            activatedChunks.Add(coord, terrainChunk);

            return terrainChunk;
        }

        /// <summary>
        /// 创建地形块到场景,若已经存在,则更新其贴图;
        /// </summary>
        public static TerrainData CreateOrUpdate(RectCoord coord, Texture2D diffuse, Texture2D height)
        {
            if (diffuse == null || height == null)
                throw new NullReferenceException("空的贴图!");

            TerrainData chunk;

            if (TryGetChunk(coord, out chunk))
            {
                SetChunk(chunk, coord, diffuse, height);
            }
            else
            {
                chunk = Create(coord, diffuse, height);
            }
            return chunk;
        }

        /// <summary>
        /// 设置参数到地形块;
        /// </summary>
        static void SetChunk(TerrainData chunk, RectCoord coord, Texture2D diffuse, Texture2D height)
        {
            chunk.Coord = coord;
            chunk.DiffuseTexture = diffuse;
            chunk.HeightTexture = height;
            chunk.Tessellation = GlobalTessellation;
            chunk.Displacement = GlobalDisplacement;
        }

        /// <summary>
        /// 移除这个地形块;
        /// </summary>
        public static bool Destroy(RectCoord coord)
        {
            TerrainData terrainChunk;
            if (activatedChunks.TryGetValue(coord, out terrainChunk))
            {
                terrainChunk.DestroyTextures();

                ReleaseTerrainChunk(terrainChunk);
                activatedChunks.Remove(coord);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除这个地形块,但是返回其贴图信息;
        /// </summary>
        public static bool Destroy(RectCoord coord, out Texture2D diffuse, out Texture2D height)
        {
            TerrainData terrainChunk;
            if (activatedChunks.TryGetValue(coord, out terrainChunk))
            {
                diffuse = terrainChunk.DiffuseTexture;
                height = terrainChunk.HeightTexture;
                terrainChunk.ClearTextures();

                ReleaseTerrainChunk(terrainChunk);
                activatedChunks.Remove(coord);
                return true;
            }
            diffuse = default(Texture2D);
            height = default(Texture2D);
            return false;
        }

        /// <summary>
        /// 移除所有的地图块;
        /// </summary>
        public static void DestroyAll()
        {
            RectCoord[] coords = activatedChunks.Keys.ToArray();
            foreach (var coord in coords)
            {
                Destroy(coord);
            }
        }

        /// <summary>
        /// 获取到这个地形块实例;
        /// </summary>
        public static bool TryGetChunk(RectCoord coord , out TerrainData chunk)
        {
            return activatedChunks.TryGetValue(coord, out chunk);
        }

        /// <summary>
        /// 确认是否已经实例化这个地形块;
        /// </summary>
        public static bool Contains(RectCoord coord)
        {
            return activatedChunks.ContainsKey(coord);
        }

        /// <summary>
        /// 从池内获取到或者实例化一个;
        /// </summary>
        static TerrainData GetTerrainChunk(string name)
        {
            TerrainData terrainChunk;
            if (restingChunks.Count > 0)
            {
                terrainChunk = restingChunks.Dequeue();
                terrainChunk.gameObject.SetActive(true);
            }
            else
            {
                GameObject gameObject = new GameObject(name, typeof(TerrainData));
                terrainChunk = gameObject.GetComponent<TerrainData>();
#if UNITY_EDITOR
                terrainChunk.transform.SetParent(ChunkParent, false);
#endif
            }
            return terrainChunk;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 放置地形块的父节点;
        /// </summary>
        static Transform chunkParent;
        static Transform ChunkParent
        {
            get { return chunkParent ?? (chunkParent = new GameObject("TerrainChunks").transform); }
        }
#endif

        /// <summary>
        /// 将地形块放回池内,备下次使用;
        /// </summary>
        static void ReleaseTerrainChunk(TerrainData terrainChunk)
        {
            terrainChunk.gameObject.SetActive(false);
            restingChunks.Enqueue(terrainChunk);
        }

        #endregion


        #region 地形方法(静态)

        /// <summary>
        /// 获取到高度,若超出地图边界则返回0;
        /// </summary>
        public static float GetHeight(Vector3 position)
        {
            TerrainData chunk;
            RectCoord coord;
            Vector2 uv = ChunkGrid.GetUV(position, out coord);

            if (activatedChunks.TryGetValue(coord, out chunk))
            {
                int x = (int)(uv.x * chunk.HeightTexture.width);
                int y = (int)(uv.y * chunk.HeightTexture.height);

                Color pixelColor = chunk.HeightTexture.GetPixel(x, y);

                return pixelColor.a * GlobalDisplacement;
            }
            return 0f;
        }

        /// <summary>
        /// 是否超出了地形的定义范围;
        /// </summary>
        public static bool IsOutTerrain(Vector3 position)
        {
            RectCoord coord = ChunkGrid.GetCoord(position);
            return Contains(coord);
        }



        #endregion

    }

}
