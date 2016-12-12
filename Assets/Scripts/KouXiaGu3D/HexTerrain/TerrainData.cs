using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #region 实例;

        const string shaderTerrain = "HexTerrain/Terrain";
        const string shaderHeight = "HexTerrain/Heigt";

        static Shader terrainShader
        {
            get { return Shader.Find(shaderTerrain); }
        }

        static Shader heightShader
        {
            get { return Shader.Find(shaderHeight); }
        }

        RectCoord coord;
        Material material;
        Texture2D heightTexture;
        Texture2D diffuseTexture;
        float tessellation;
        float displacement;

        /// <summary>
        /// 地图块坐标;
        /// </summary>
        public RectCoord Coord
        {
            get { return coord; }
            private set { transform.position = BlockToPixelCenter(value); coord = value; }
        }

        /// <summary>
        /// 正在使用的材质;
        /// </summary>
        Material Material
        {
            get { return material ?? (material = new Material(terrainShader)); }
        }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseTexture
        {
            get { return diffuseTexture; }
            private set { Material.SetTexture("_MainTex", value); diffuseTexture = value; }
        }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightTexture
        {
            get { return heightTexture; }
            private set { Material.SetTexture("_HeightTex", value); heightTexture = value; }
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
            GetComponent<MeshFilter>().mesh = GetMesh();
            GetComponent<MeshRenderer>().material = Material;
        }

        void Clear()
        {
            Coord = RectCoord.Self;
            DiffuseTexture = null;
            HeightTexture = null;
        }



        [ContextMenu("显示地形模式")]
        void TerrainDisplay()
        {
            Material.shader = terrainShader;

            DiffuseTexture = diffuseTexture;
            HeightTexture = heightTexture;
            Tessellation = tessellation;
            Displacement = displacement;
        }

        [ContextMenu("显示高度模式")]
        void HeightDisplay()
        {
            Material.shader = heightShader;
        }


        public override int GetHashCode()
        {
            return Coord.GetHashCode();
        }

        #endregion


        #region 地图块实例信息(静态)

        static float globalTessellation = 16f;
        static float globalDisplacement = 3f;

        /// <summary>
        /// 在场景中激活的地图块;
        /// </summary>
        static Dictionary<RectCoord, TerrainData> activatedBlocks = new Dictionary<RectCoord, TerrainData>();
        /// <summary>
        /// 休眠的地图块;
        /// </summary>
        static Queue<TerrainData> restingBlocks = new Queue<TerrainData>();

        /// <summary>
        /// 全局的网格细分程度;
        /// </summary>
        public static float GlobalTessellation
        {
            get { return globalTessellation; }
            set { globalTessellation = value; }
        }

        /// <summary>
        /// 全局的高度系数;
        /// </summary>
        public static float GlobalDisplacement
        {
            get { return globalDisplacement; }
            set { globalDisplacement = value; }
        }

        /// <summary>
        /// 创建地图块到场景;
        /// </summary>
        public static void Create(RectCoord coord, Texture2D diffuse, Texture2D height)
        {
            if (activatedBlocks.ContainsKey(coord))
                throw new ArgumentException("地图块已经创建到场景;");
            if (diffuse == null || height == null)
                throw new NullReferenceException("空的贴图!");
            
            TerrainData terrainBlock = GetTerrainBlock(coord.ToString());

            terrainBlock.Coord = coord;
            terrainBlock.DiffuseTexture = diffuse;
            terrainBlock.HeightTexture = height;
            terrainBlock.Tessellation = GlobalTessellation;
            terrainBlock.Displacement = GlobalDisplacement;

            activatedBlocks.Add(coord, terrainBlock);
        }

        /// <summary>
        /// 移除这个地图块;
        /// </summary>
        public static bool Destroy(RectCoord coord)
        {
            TerrainData terrainBlock;
            if (activatedBlocks.TryGetValue(coord, out terrainBlock))
            {
                ReleaseTerrainBlock(terrainBlock);
                activatedBlocks.Remove(coord);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从池内获取到或者实例化一个;
        /// </summary>
        static TerrainData GetTerrainBlock(string name)
        {
            TerrainData terrainBlock;
            if (restingBlocks.Count > 0)
            {
                terrainBlock = restingBlocks.Dequeue();
                terrainBlock.gameObject.SetActive(true);
            }
            else
            {
                GameObject gameObject = new GameObject(name, typeof(TerrainData));
                terrainBlock = gameObject.GetComponent<TerrainData>();
#if UNITY_EDITOR
                terrainBlock.transform.SetParent(BlockParent, false);
#endif
            }
            return terrainBlock;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 放置地图块的父节点;
        /// </summary>
        static Transform blockParent;

        static Transform BlockParent
        {
            get { return blockParent ?? (blockParent = new GameObject("TerrainBlocks").transform); }
        }
#endif

        /// <summary>
        /// 将地图块放回池内,备下次使用;
        /// </summary>
        static void ReleaseTerrainBlock(TerrainData terrainBlock)
        {
            terrainBlock.Clear();
            terrainBlock.gameObject.SetActive(false);
            restingBlocks.Enqueue(terrainBlock);
        }

        /// <summary>
        /// 获取到高度,若超出地图边界则返回0;
        /// </summary>
        public static float GetHeight(Vector3 position)
        {
            TerrainData block;
            RectCoord coord;
            Vector2 uv = RectGrid.GetUV(position, out coord);

            if (activatedBlocks.TryGetValue(coord, out block))
            {
                int x = (int)(uv.x * block.HeightTexture.width);
                int y = (int)(uv.y * block.HeightTexture.height);

                Color pixelColor = block.HeightTexture.GetPixel(x, y);

                return pixelColor.a * GlobalDisplacement;
            }
            return 0f;
        }

        #endregion


        #region 地图块大小定义(静态);


        /// <summary>
        /// 地图节点所使用的六边形参数;
        /// </summary>
        static Hexagon hexagon
        {
            get { return GridConvert.hexagon; }
        }
        
        /// <summary>
        /// 地图块大小(需要大于或等于2);
        /// </summary>
        public const int size = 3;

        /// <summary>
        /// 地图块宽度(像素*100);
        /// </summary>
        public static readonly float BlockWidth = (float)(hexagon.OuterDiameters * 1.5f * (size - 1));
        public static readonly float BlockWidthHalf = BlockWidth / 2;

        /// <summary>
        /// 地图块高度(像素*100);
        /// </summary>
        public static readonly float BlockHeight = (float)hexagon.InnerDiameters * size;
        public static readonly float BlockHeightHalf = BlockHeight / 2;


        static readonly RectGrid rectGrid = new RectGrid(BlockWidth, BlockHeight);
        public static RectGrid RectGrid
        {
            get { return rectGrid; }
        }

        /// <summary>
        /// 地图块坐标 获取到其像素中心点;
        /// </summary>
        internal static Vector3 BlockToPixelCenter(RectCoord coord)
        {
            float x = coord.x * BlockWidth;
            float z = coord.y * BlockHeight;
            return new Vector3(x, 0, z);
        }

        /// <summary>
        /// 地图块坐标 获取到其中心的六边形坐标;
        /// </summary>
        internal static CubicHexCoord BlockToHexCenter(RectCoord coord)
        {
            Vector3 pixelCenter = RectGrid.GetCenter(coord);
            return GridConvert.Grid.GetCubic(pixelCenter);
        }


        /// <summary>
        /// 获取到这个地图块覆盖到的所有地图节点坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetBlockCover(RectCoord coord)
        {
            CubicHexCoord hexCenter = BlockToHexCenter(coord);
            CubicHexCoord startCoord = CubicHexCoord.GetDirectionOffset(HexDirections.Southwest) * size + hexCenter + CubicHexCoord.DIR_South;

            for (short endX = (short)(Math.Abs(hexCenter.X - startCoord.X) + hexCenter.X);
                startCoord.X <= endX;
                startCoord += (startCoord.X & 1) == 0 ?
                (((size & 1) == 0) ? CubicHexCoord.DIR_Northeast : CubicHexCoord.DIR_Southeast) :
                (((size & 1) == 0) ? CubicHexCoord.DIR_Southeast : CubicHexCoord.DIR_Northeast))
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
        /// 获取到地图节点所属的地图块;
        /// </summary>
        public static RectCoord[] GetBelongBlocks(Vector3 point)
        {
            CubicHexCoord coord = GridConvert.Grid.GetCubic(point);
            return GetBelongBlocks(coord);
        }

        /// <summary>
        /// 获取到地图节点所属的地图块;
        /// </summary>
        public static RectCoord[] GetBelongBlocks(CubicHexCoord coord)
        {
            RectCoord[] blocks = new RectCoord[2];
            GetBelongBlocks(coord, ref blocks);
            return blocks;
        }

        /// <summary>
        /// 获取到地图节点所属的地图块;
        /// 传入数组容量需要大于或者等于2,所属的地图块编号放置在 0 和 1 下标处;
        /// </summary>
        public static void GetBelongBlocks(CubicHexCoord coord, ref RectCoord[] blocks)
        {
            Vector3 point = GridConvert.Grid.GetPixel(coord);
            GetBelongBlocks(point, ref blocks);
        }

        static readonly Vector3 cBelongPoint1 =
            new Vector3((float)hexagon.OuterRadius / 2, 0, (float)hexagon.InnerRadius / 2);
        static readonly Vector3 cBelongPoint2 =
            new Vector3(-(float)hexagon.OuterRadius / 2, 0, -(float)hexagon.InnerRadius / 2);

        /// <summary>
        /// 获取到地图节点所属的地图块;
        /// 传入数组容量需要大于或者等于2,所属的地图块编号放置在 0 和 1 下标处;
        /// </summary>
        static void GetBelongBlocks(Vector3 pointCenter, ref RectCoord[] blocks)
        {
            try
            {
                Vector3 point1 = pointCenter + cBelongPoint1;
                blocks[0] = rectGrid.GetCoord(point1);

                Vector3 point2 = pointCenter + cBelongPoint2;
                blocks[1] = rectGrid.GetCoord(point2);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }


        #endregion


        #region 网格数据(静态);

        const string meshName = "Terrain Mesh";

        //为了地形相接的地方不存在明显的缝隙,所以加上 小数 的数值;
        static readonly float meshHalfWidth = BlockWidthHalf + 0.005f;
        static readonly float meshHalfHeight = BlockHeightHalf + 0.005f;

        /// <summary>
        /// 网格生成的高度;
        /// </summary>
        const float altitude = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        static readonly Vector3[] vertices = new Vector3[]
            {
                new Vector3(-meshHalfWidth , altitude, meshHalfHeight),
                new Vector3(meshHalfWidth, altitude, meshHalfHeight),
                new Vector3(meshHalfWidth, altitude, -meshHalfHeight),
                new Vector3(-meshHalfWidth, altitude, -meshHalfHeight),
            };

        /// <summary>
        /// 网格三角形数据;
        /// </summary>
        static readonly int[] triangles = new int[]
           {
                0,1,2,
                0,2,3,
           };

        /// <summary>
        /// 网格UV坐标数据;
        /// </summary>
        static readonly Vector2[] uv = new Vector2[]
           {
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f),
           };

        /// <summary>
        /// 创建一个新的网格;
        /// </summary>
        static Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();

            mesh.name = meshName;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();

            return mesh;
        }

        static Mesh terrainMesh;

        /// <summary>
        /// 获取到公共的网格;
        /// </summary>
        static Mesh GetMesh()
        {
            return terrainMesh ?? (terrainMesh = CreateMesh());
        }

        #endregion


    }

}
