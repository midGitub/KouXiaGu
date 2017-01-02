using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent, CustomEditorTool]
    public class TerrainData : UnitySington<TerrainData>
    {
        [SerializeField]
        Shader terrainShader;
        [SerializeField]
        Shader heightShader;

        public static Shader TerrainShader
        {
            get { return GetInstance.terrainShader; }
        }

        public static Shader HeightShader
        {
            get { return GetInstance.heightShader; }
        }

        [SerializeField, Range(0, 32)]
        float tessellation = 32f;
        [SerializeField, Range(0, 5)]
        float displacement = 2f;
        [SerializeField, Range(0,20)]
        float snowLevel = 0f;

        public static float Tessellation
        {
            get { return GetInstance.tessellation; }
            set { Shader.SetGlobalFloat("_TerrainTess", value); GetInstance.tessellation = value; }
        }

        public static float Displacement
        {
            get { return GetInstance.displacement; }
            set { Shader.SetGlobalFloat("_TerrainDisplacement", value); GetInstance.displacement = value; }
        }

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
            Vector2 uv = TerrainChunk.ChunkGrid.GetUV(position, out coord);

            if (TerrainChunk.TryGetChunk(coord, out chunk))
            {
                Color pixelColor = chunk.HeightTexture.GetPixel(uv);
                return pixelColor.r * Displacement;
            }
            return 0f;
        }

        /// <summary>
        /// 是否超出了地形的定义范围;
        /// </summary>
        public static bool IsOutTerrain(Vector3 position)
        {
            RectCoord coord = TerrainChunk.ChunkGrid.GetCoord(position);
            return TerrainChunk.Contains(coord);
        }

        public void SetSnowLevel(float snow)
        {
            SnowLevel = snow;
        }

        #endregion

    }

}
