using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{
    public class LandformRenderer
    {
        public const string tessellationName = "_LandformTess";
        public const string DisplacementName = "_LandformDisplacement";
        public const string DiffuseMapName = "_DiffuseMap";
        public const string HeightMapName = "_HeightMap";
        public const string NormalMapName = "_NormalMap";

        public static float GetTessellation()
        {
            float value = Shader.GetGlobalFloat(tessellationName);
            return value;
        }

        public static void SetTessellation(float value)
        {
            Shader.SetGlobalFloat(tessellationName, value);
        }

        public static float GetDisplacement()
        {
            float value = Shader.GetGlobalFloat(DisplacementName);
            return value;
        }

        public static void SetDisplacement(float value)
        {
            Shader.SetGlobalFloat(DisplacementName, value);
        }



        private MeshRenderer meshRenderer;
        private Material material => meshRenderer.material;

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseMap { get; private set; }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightMap { get; private set; }

        /// <summary>
        /// 法线贴图;
        /// </summary>
        public Texture2D NormalMap { get; private set; }

        public LandformRenderer(MeshRenderer meshRenderer)
        {
            this.meshRenderer = meshRenderer;
        }

        /// <summary>
        /// 设置到贴图,并且销毁旧贴图;
        /// </summary>
        public void SetDiffuseMap(Texture2D diffuseMap)
        {
            if (DiffuseMap != diffuseMap)
            {
                if (DiffuseMap != null)
                {
                    GameObject.Destroy(DiffuseMap);
                }
                material.SetTexture(DiffuseMapName, diffuseMap);
                DiffuseMap = diffuseMap;
            }
        }

        /// <summary>
        /// 设置到贴图,并且销毁旧贴图;
        /// </summary>
        public void SetHeightMap(Texture2D heightMap)
        {
            if (HeightMap != heightMap)
            {
                if (HeightMap != null)
                {
                    GameObject.Destroy(HeightMap);
                }
                material.SetTexture(HeightMapName, heightMap);
                HeightMap = heightMap;
            }
        }

        /// <summary>
        /// 设置到贴图,并且销毁旧贴图;
        /// </summary>
        public void SetNormalMap(Texture2D normalMap)
        {
            if (NormalMap != normalMap)
            {
                if (NormalMap != null)
                {
                    GameObject.Destroy(NormalMap);
                }
                material.SetTexture(NormalMapName, normalMap);
                NormalMap = normalMap;
            }
        }

        ///// <summary>
        ///// 销毁所有贴图;
        ///// </summary>
        //public void Destroy()
        //{
        //    SetDiffuseMap(null);
        //    SetHeightMap(null);
        //    SetNormalMap(null);
        //}

        /// <summary>
        /// 获取到高度,若不存在高度信息,或超出范围,则返回0;
        /// </summary>
        public float GetHeight(Vector2 uv)
        {
            if (HeightMap == null)
                return 0;

            Color pixelColor = HeightMap.GetPixel(uv);
            return pixelColor.r * GetDisplacement();
        }
    }


}
