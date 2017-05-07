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
    class LandformSettings : UnitySington<LandformSettings>
    {
        LandformSettings()
        {
        }

        #region 编辑器设置变量;

        [SerializeField]
        Shader landformShader;
        [SerializeField, Range(0, 64)]
        float tessellation = 16f;
        [SerializeField, Range(0, 5)]
        float displacement = 1.3f;
        [SerializeField]
        Texture gridLineMap;
        [SerializeField]
        Color gridLineColor;

        #endregion

        /// <summary>
        /// 地形Shader;
        /// </summary>
        public Shader LandformShader
        {
            get { return landformShader; }
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
            SetTessellation(tessellation);
            SetDisplacement(displacement);
            SetGridLineMap(gridLineMap);
            SetGridLineColor(gridLineColor);
        }

        void OnValidate()
        {
            Awake();
        }


        const string TessellationName = "_Tess";

        public float GetTessellation()
        {
            float value = Shader.GetGlobalFloat(TessellationName);
            return value;
        }

        public void SetTessellation(float value)
        {
            Shader.SetGlobalFloat(TessellationName, value);
        }


        const string DisplacementName = "_Displacement";

        public float GetDisplacement()
        {
            float value = Shader.GetGlobalFloat(DisplacementName);
            return value;
        }

        public void SetDisplacement(float value)
        {
            Shader.SetGlobalFloat(DisplacementName, value);
        }


        const string GridLineMapName = "_GridLineMap";

        public Texture GetGridLineMap()
        {
            var texture = Shader.GetGlobalTexture(GridLineMapName);
            return texture;
        }

        public void SetGridLineMap(Texture texture)
        {
            Shader.SetGlobalTexture(GridLineMapName, texture);
        }


        const string GridLineColorName = "_GridLineColor";

        public Color GetGridLineColor()
        {
            var color = Shader.GetGlobalColor(GridLineColorName);
            return color;
        }

        public void SetGridLineColor(Color color)
        {
            Shader.SetGlobalColor(GridLineColorName, color);
        }

    }

}
