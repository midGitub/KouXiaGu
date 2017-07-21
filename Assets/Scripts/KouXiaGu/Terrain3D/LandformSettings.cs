using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形参数设置;
    /// </summary>
    [DisallowMultipleComponent]
    class LandformSettings : SceneSington<LandformSettings>
    {
        /// <summary>
        /// 透明的黑色颜色;
        /// </summary>
        public static readonly Color BlackTransparent = new Color(0, 0, 0, 0);

        /// <summary>
        /// 地平线颜色;
        /// </summary>
        public static readonly Color Horizon = new Color(0.5f, 0.5f, 0.5f, 1);

        LandformSettings()
        {
        }

        #region 编辑器设置变量;

        public Shader landformShader = null;
        public BakeCamera bakeCamera = null;
        public BakeLandform bakeLandform = null;
        public BakeRoad bakeRoad = null;
        public WaterSettings waterSettings = default(WaterSettings);
        [Range(0, 64)]
        public float tessellation = 16f;
        [Range(0, 5)]
        public float displacement = 1.3f;
        public Texture gridLineMap = null;
        public Color gridLineColor = Color.black;
        public bool isDisplayGridLine = true;

        #endregion

        /// <summary>
        /// 地形Shader;
        /// </summary>
        public Shader LandformShader
        {
            get { return landformShader; }
        }

        public QualitySettings QualitySettings
        {
            get { return bakeCamera.qualitySettings; }
        }

        public WaterSettings WaterSettings
        {
            get { return waterSettings; }
        }

        /// <summary>
        /// 地形细分程度;
        /// </summary>
        public float Tessellation
        {
            get { return tessellation; }
        }

        /// <summary>
        /// 地形高度缩放;
        /// </summary>
        public float Displacement
        {
            get { return displacement; }
        }

        void Awake()
        {
            SetInstance(this);
            bakeLandform.Initialize();
            bakeRoad.Initialise();
            OnValidate();
        }

        void OnValidate()
        {
            SetTessellation(tessellation);
            SetDisplacement(displacement);
            SetGridLineMap(gridLineMap);
            SetGridLineColor(gridLineColor);
            SetIsDisplayGridLine(isDisplayGridLine);
        }

        const string TessellationName = "_LandformTess";

        public static float GetTessellation()
        {
            float value = Shader.GetGlobalFloat(TessellationName);
            return value;
        }

        public static void SetTessellation(float value)
        {
            Shader.SetGlobalFloat(TessellationName, value);
        }


        const string DisplacementName = "_LandformDisplacement";

        public static float GetDisplacement()
        {
            float value = Shader.GetGlobalFloat(DisplacementName);
            return value;
        }

        public static void SetDisplacement(float value)
        {
            Shader.SetGlobalFloat(DisplacementName, value);
        }


        const string GridLineMapName = "_LandformGridLineMap";

        public static Texture GetGridLineMap()
        {
            var texture = Shader.GetGlobalTexture(GridLineMapName);
            return texture;
        }

        public static void SetGridLineMap(Texture texture)
        {
            Shader.SetGlobalTexture(GridLineMapName, texture);
        }

        
        const string GridLineColorName = "_LandformGridLineColor";

        public static Color GetGridLineColor()
        {
            var color = Shader.GetGlobalColor(GridLineColorName);
            return color;
        }

        public static void SetGridLineColor(Color color)
        {
            Shader.SetGlobalColor(GridLineColorName, color);
        }


        const string IsDisplayGridLineName = "_LandformIsDisplayGridLine";

        public static bool GetIsDisplayGridLine()
        {
            var value = Shader.GetGlobalInt(IsDisplayGridLineName);
            return value != 0;
        }

        public static void SetIsDisplayGridLine(bool isDisplay)
        {
            int value = isDisplay ? 1 : 0;
            Shader.SetGlobalInt(IsDisplayGridLineName, value);
        }
    }
}
