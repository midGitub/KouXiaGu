using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形数据管理;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainData : SceneSington<TerrainData>
    {
        [SerializeField]
        Shader terrainShader;

        /// <summary>
        /// 地形Shader;
        /// </summary>
        public static Shader TerrainShader
        {
            get { return GetInstance.terrainShader; }
        }


        public static bool IsInitialized { get; private set; }
        public static SceneCreater Creater { get; private set; }

        /// <summary>
        /// 地形细分程度;
        /// </summary>
        public static float Tessellation
        {
            get { return GetInstance.tessellation; }
            set { Shader.SetGlobalFloat("_TerrainTess", value); GetInstance.tessellation = value; }
        }

        /// <summary>
        /// 地形高度缩放;
        /// </summary>
        public static float Displacement
        {
            get { return GetInstance.displacement; }
            set { Shader.SetGlobalFloat("_TerrainDisplacement", value); GetInstance.displacement = value; }
        }

        /// <summary>
        /// 降雪程度;
        /// </summary>
        public static float SnowLevel
        {
            get { return GetInstance.snowLevel; }
            set { Shader.SetGlobalFloat("_TerrainSnow", value); GetInstance.snowLevel = value; }
        }


        public static void Initialize(IDictionary<CubicHexCoord, MapNode> data)
        {
            if (!IsInitialized)
            {
                Creater = new SceneCreater(data);

                IsInitialized = true;
            }
        }

        static void Uninitialize()
        {
            if (IsInitialized)
            {
                Creater = null;
                IsInitialized = false;
            }
        }


        TerrainData()
        {
        }


        [SerializeField, Range(0, 32)]
        float tessellation = 32f;
        [SerializeField, Range(0, 5)]
        float displacement = 2f;
        [SerializeField, Range(0, 20)]
        float snowLevel = 0f;


        void Start()
        {
            Tessellation = tessellation;
            Displacement = displacement;
            SnowLevel = snowLevel;
        }

        void OnValidate()
        {
            Start();
        }

        protected override void OnDestroy()
        {
            Uninitialize();
            base.OnDestroy();
        }

        #region 地形方法(静态)

        /// <summary>
        /// 获取到高度,若超出地图边界则返回0;
        /// </summary>
        public static float GetHeight(Vector3 position)
        {
            LandformChunk chunk;
            RectCoord coord;
            Vector2 uv = LandformChunk.ChunkGrid.GetUV(position, out coord);

            if (Creater != null && Creater.Landform.ActivatedChunks.TryGetValue(coord, out chunk))
            {
                return GetHeight(chunk.Renderer, uv);
            }
            return 0f;
        }

        /// <summary>
        /// 获取到对应的高度;
        /// </summary>
        public static float GetHeight(LandformRenderer chunk, Vector2 uv)
        {
            Color pixelColor = chunk.HeightMap.GetPixel(uv);
            return pixelColor.r * Displacement;
        }


        /// <summary>
        /// 限制到指定地形块,并获取到高度;
        /// </summary>
        public static float GetHeight(RectCoord clamp, Vector3 position, Texture2D heightMap)
        {
            Vector2 uv = LandformChunk.ChunkGrid.GetUV(clamp, position);
            Color pixelColor = heightMap.GetPixel(uv);
            return pixelColor.r * Displacement;
        }


        /// <summary>
        /// 是否超出了地形的定义范围;
        /// </summary>
        public static bool IsOutTerrain(Vector3 position)
        {
            RectCoord coord = LandformChunk.ChunkGrid.GetCoord(position);
            return Creater == null && Creater.Landform.ActivatedChunks.ContainsKey(coord);
        }

        public void SetSnowLevel(float snow)
        {
            SnowLevel = snow;
        }

        #endregion

    }

}
