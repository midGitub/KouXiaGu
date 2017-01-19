using KouXiaGu.Grids;
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

        [SerializeField, Range(0, 32)]
        float tessellation = 32f;
        [SerializeField, Range(0, 5)]
        float displacement = 2f;
        [SerializeField, Range(0,20)]
        float snowLevel = 0f;

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

        #region 地形方法(静态)

        /// <summary>
        /// 获取到高度,若超出地图边界则返回0;
        /// </summary>
        public static float GetHeight(Vector3 position)
        {
            TerrainChunk chunk;
            RectCoord coord;
            UV uv = TerrainChunk.ChunkGrid.GetUV(position, out coord);

            if (TerrainCreater.ActivatedChunks.TryGetValue(coord, out chunk))
            {
                return GetHeight(chunk.Renderer, uv);
            }
            return 0f;
        }

        /// <summary>
        /// 获取到对应的高度;
        /// </summary>
        public static float GetHeight(TerrainRenderer chunk, UV uv)
        {
            Color pixelColor = chunk.HeightMap.GetPixel(uv);
            return pixelColor.r * Displacement;
        }

        /// <summary>
        /// 是否超出了地形的定义范围;
        /// </summary>
        public static bool IsOutTerrain(Vector3 position)
        {
            RectCoord coord = TerrainChunk.ChunkGrid.GetCoord(position);
            return TerrainCreater.ActivatedChunks.ContainsKey(coord);
        }

        public void SetSnowLevel(float snow)
        {
            SnowLevel = snow;
        }

        #endregion

    }

}
