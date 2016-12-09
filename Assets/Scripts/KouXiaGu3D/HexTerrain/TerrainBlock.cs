using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 地块烘焙信息;
    /// </summary>
    [DisallowMultipleComponent]
    public class TerrainBlock : TerrainBlockMesh
    {
        TerrainBlock() { }

        #region 实例的

        const string shaderName = "HexTerrain/Terrain";

        ShortVector2 coord;

        Material material;
        Texture2D heightTexture;
        Texture2D diffuseTexture;
        float tessellation;

        /// <summary>
        /// 地图块坐标;
        /// </summary>
        public ShortVector2 Coord
        {
            get { return coord; }
            private set { transform.position = BlockCoordToBlockCenter(Coord); }
        }

        Shader shader
        {
            get { return Shader.Find(shaderName); }
        }

        /// <summary>
        /// 正在使用的材质;
        /// </summary>
        Material Material
        {
            get { return material ?? (material = new Material(shader)); }
        }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseTexture
        {
            get { return diffuseTexture; }
            private set { material.SetTexture("_MainTex", value); diffuseTexture = value; }
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


        void Start()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = Material;
        }

        void Clear()
        {
            Coord = ShortVector2.Zero;
            DiffuseTexture = null;
            HeightTexture = null;
        }

        /// <summary>
        /// 获取到高度;
        /// </summary>
        public float GetHeight(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return Coord.GetHashCode();
        }

        #endregion


        #region 地图块创建(静态)

        static float globalTessellation = 16;

        /// <summary>
        /// 在场景中激活的地图块;
        /// </summary>
        static Dictionary<ShortVector2, TerrainBlock> activatedBlocks = new Dictionary<ShortVector2, TerrainBlock>();
        /// <summary>
        /// 休眠的地图块;
        /// </summary>
        static Queue<TerrainBlock> restingBlocks = new Queue<TerrainBlock>();

        /// <summary>
        /// 全局的网格细分程度;
        /// </summary>
        public static float GlobalTessellation
        {
            get { return globalTessellation; }
            set { globalTessellation = value; }
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
            
            TerrainBlock terrainBlock = GetTerrainBlock(coord.ToString());

            terrainBlock.Coord = coord;
            terrainBlock.DiffuseTexture = diffuse;
            terrainBlock.HeightTexture = height;
            terrainBlock.Tessellation = GlobalTessellation;

            activatedBlocks.Add(coord, terrainBlock);
        }

        /// <summary>
        /// 移除这个地图块;
        /// </summary>
        public static bool Destroy(ShortVector2 coord)
        {
            TerrainBlock terrainBlock;
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
        static TerrainBlock GetTerrainBlock(string name)
        {
            TerrainBlock terrainBlock;
            if (restingBlocks.Count > 0)
            {
                terrainBlock = restingBlocks.Dequeue();
                terrainBlock.gameObject.SetActive(true);
            }
            else
            {
                GameObject gameObject = new GameObject(name, typeof(TerrainBlock));
                terrainBlock = gameObject.GetComponent<TerrainBlock>();
            }
            return terrainBlock;
        }

        /// <summary>
        /// 将地图块放回池内,备下次使用;
        /// </summary>
        static void ReleaseTerrainBlock(TerrainBlock terrainBlock)
        {
            terrainBlock.Clear();
            terrainBlock.gameObject.SetActive(false);
            restingBlocks.Enqueue(terrainBlock);
        }

        #endregion

        //static public Rect GetRect(Vector2i pos)
        //{
        //    Rect r = new Rect(pos.x * ChunkSizeInWorld - ChunkSizeInWorld * 0.5f, pos.y * ChunkSizeInWorld - ChunkSizeInWorld * 0.5f, ChunkSizeInWorld, ChunkSizeInWorld);
        //    return r;
        //}

        //public Vector2 GetWorldToUV(Vector3 world3DPos)
        //{
        //    Rect r = GetRect();
        //    Vector2 world2D = new Vector2(world3DPos.x, world3DPos.z);
        //    Vector2 uv = (world2D - new Vector2(r.xMin, r.yMin)) / r.width;
        //    return uv;
        //}

        //static public float GetWorldHeightAt(Vector3 world3Dposition)
        //{
        //    Chunk chunk = Chunk.WorldToChunk(world3Dposition);
        //    if (chunk == null || chunk.height == null)
        //        return 0f;

        //    Vector2 uv = chunk.GetWorldToUV(world3Dposition);
        //    int x = (int)(Mathf.Clamp01(1f - uv.x) * chunk.height.width);
        //    int y = (int)(Mathf.Clamp01(1f - uv.y) * chunk.height.height);

        //    Color pixel = chunk.height.GetPixel(x, y);

        //    //pixel is 0 - 1 value. we will move it to -1 to 1
        //    float heightBase = (pixel.a - 0.5f) * 2f;

        //    return heightBase * Chunk.ChunkSizeScale();
        //}


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
        public const int size = 2;

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
        public static ShortVector2 PixelToBlockCoord(Vector3 position)
        {
            short x = (short)Math.Round(position.x / BlockWidth);
            short y = (short)Math.Round(position.z / BlockHeight);
            return new ShortVector2(x, y);
        }


        /// <summary>
        /// 地图块坐标 获取到其像素中心点;
        /// </summary>
        public static Vector3 BlockCoordToBlockCenter(ShortVector2 coord)
        {
            float x = coord.x * BlockWidth;
            float z = coord.y * BlockHeight;
            return new Vector3(x, 0, z);
        }

        /// <summary>
        /// 地图块坐标 获取到其中心的六边形坐标;
        /// </summary>
        public static CubicHexCoord BlockCoordToHexCenter(ShortVector2 coord)
        {
            Vector3 pixelCenter = BlockCoordToBlockCenter(coord);
            return HexGrids.PixelToHex(pixelCenter);
        }


        /// <summary>
        /// 地图块坐标 到获取到其在场景中的矩形大小;
        /// </summary>
        public static Rect BlockCoordToRect(ShortVector2 coord)
        {
            Vector3 blockCenter = BlockCoordToBlockCenter(coord);
            return BlockCenterToRect(blockCenter);
        }

        /// <summary>
        /// 地图块中心坐标 获取到其在场景中的矩形大小;
        /// </summary>
        public static Rect BlockCenterToRect(Vector3 blockCenter)
        {
            Vector2 southwestPoint = new Vector2(blockCenter.x - BlockWidthHalf, blockCenter.z - BlockHeightHalf);
            Vector2 size = new Vector2(BlockWidth, BlockHeight);
            return new Rect(southwestPoint, size);
        }


        /// <summary>
        /// 获取到这个地图块覆盖到的所有地图节点坐标;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetBlockCover(ShortVector2 coord)
        {
            CubicHexCoord hexCenter = BlockCoordToHexCenter(coord);
            CubicHexCoord startCoord = HexGrids.HexDirectionVector(HexDirections.Southwest) * size + hexCenter + CubicHexCoord.DIR_South;

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

        #endregion

    }

}
