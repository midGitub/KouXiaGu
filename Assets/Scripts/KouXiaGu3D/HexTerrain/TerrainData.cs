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

        ShortVector2 coord;
        Material material;
        Texture2D heightTexture;
        Texture2D diffuseTexture;
        float tessellation;
        float displacement;

        /// <summary>
        /// 地图块坐标;
        /// </summary>
        public ShortVector2 Coord
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
            Coord = ShortVector2.Zero;
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
        static Dictionary<ShortVector2, TerrainData> activatedBlocks = new Dictionary<ShortVector2, TerrainData>();
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
        public static void Create(ShortVector2 coord, Texture2D diffuse, Texture2D height)
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
        public static bool Destroy(ShortVector2 coord)
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
            ShortVector2 coord = PixelToBlock(position);
            if (activatedBlocks.TryGetValue(coord, out block))
            {
                Vector2 uv = PixelToUV(position);

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
            get { return HexGrids.hexagon; }
        }
        
        /// <summary>
        /// 地图块大小(需要大于或等于2),根据需要修改;
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


        /// <summary>
        /// 从像素节点 获取到所属的地形块;
        /// </summary>
        internal static ShortVector2 PixelToBlock(Vector3 position)
        {
            short x = (short)Math.Round(position.x / BlockWidth);
            short y = (short)Math.Round(position.z / BlockHeight);
            return new ShortVector2(x, y);
        }


        /// <summary>
        /// 从像素坐标 转换为 所在块的中心像素坐标;
        /// </summary>
        internal static Vector3 PixelToCenter(Vector3 position)
        {
            ShortVector2 coord = PixelToBlock(position);
            return BlockToPixelCenter(coord);
        }

        /// <summary>
        /// 地图块坐标 获取到其像素中心点;
        /// </summary>
        internal static Vector3 BlockToPixelCenter(ShortVector2 coord)
        {
            float x = coord.X * BlockWidth;
            float z = coord.Y * BlockHeight;
            return new Vector3(x, 0, z);
        }

        /// <summary>
        /// 地图块坐标 获取到其中心的六边形坐标;
        /// </summary>
        internal static CubicHexCoord BlockToHexCenter(ShortVector2 coord)
        {
            Vector3 pixelCenter = BlockToPixelCenter(coord);
            return HexGrids.ToHexCubic(pixelCenter);
        }



        /// <summary>
        /// 地图块坐标 到获取到其在场景中的矩形大小;
        /// </summary>
        internal static Rect BlockToRect(ShortVector2 coord)
        {
            Vector3 blockCenter = BlockToPixelCenter(coord);
            return CenterToRect(blockCenter);
        }

        /// <summary>
        /// 地图块中心坐标 获取到其在场景中的矩形大小;
        /// </summary>
        internal static Rect CenterToRect(Vector3 blockCenter)
        {
            Vector2 southwestPoint = new Vector2(blockCenter.x - BlockWidthHalf, blockCenter.z - BlockHeightHalf);
            Vector2 size = new Vector2(BlockWidth, BlockHeight);
            return new Rect(southwestPoint, size);
        }


        /// <summary>
        /// 像素坐标转换成地图块的本地坐标;
        /// </summary>
        internal static Vector2 PixelToLocal(Vector3 position)
        {
            Vector3 blockCenter = PixelToCenter(position);
            Rect block = CenterToRect(blockCenter);
            Vector2 local = new Vector2(position.x - block.xMin, position.z - block.yMin);
            return local;
        }

        /// <summary>
        /// 像素坐标 转换为 地图块的UV坐标;
        /// </summary>
        internal static Vector2 PixelToUV(Vector3 position)
        {
            Vector3 blockCenter = PixelToCenter(position);
            Rect block = CenterToRect(blockCenter);
            Vector2 local = new Vector2(position.x - block.xMin, position.z - block.yMin);
            Vector2 uv = new Vector2(local.x / block.width, local.y / block.height);
            return uv;
        }



        /// <summary>
        /// 获取到这个地图块覆盖到的所有地图节点坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetBlockCover(ShortVector2 coord)
        {
            CubicHexCoord hexCenter = BlockToHexCenter(coord);
            CubicHexCoord startCoord = HexGrids.GetDirection(HexDirections.Southwest) * size + hexCenter + CubicHexCoord.DIR_South;

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
        public static ShortVector2[] GetBelongBlocks(Vector3 point)
        {
            CubicHexCoord coord = HexGrids.ToHexCubic(point);
            return GetBelongBlocks(coord);
        }

        /// <summary>
        /// 获取到地图节点所属的地图块;
        /// </summary>
        public static ShortVector2[] GetBelongBlocks(CubicHexCoord coord)
        {
            ShortVector2[] blocks = new ShortVector2[2];
            GetBelongBlocks(coord, ref blocks);
            return blocks;
        }

        /// <summary>
        /// 获取到地图节点所属的地图块;
        /// 传入数组容量需要大于或者等于2,所属的地图块编号放置在 0 和 1 下标处;
        /// </summary>
        public static void GetBelongBlocks(CubicHexCoord coord, ref ShortVector2[] blocks)
        {
            Vector3 point = HexGrids.ToPixel(coord);
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
        static void GetBelongBlocks(Vector3 pointCenter, ref ShortVector2[] blocks)
        {
            try
            {
                Vector3 point1 = pointCenter + cBelongPoint1;
                blocks[0] = PixelToBlock(point1);

                Vector3 point2 = pointCenter + cBelongPoint2;
                blocks[1] = PixelToBlock(point2);
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
