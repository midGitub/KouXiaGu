using System;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [Obsolete]
    [RequireComponent(typeof(MeshRenderer)), ExecuteInEditMode, DisallowMultipleComponent]
    public class OLandformRenderer : MonoBehaviour
    {
        OLandformRenderer()
        {
        }

        static Shader TerrainShader
        {
            get { return LandformParameter.Instance.LandformShader; }
        }

        Material material;

        [SerializeField]
        Texture2D diffuseMap;

        [SerializeField]
        Texture2D heightMap;

        [SerializeField]
        Texture2D normalMap;

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseMap
        {
            get { return diffuseMap; }
        }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightMap
        {
            get { return heightMap; }
        }

        /// <summary>
        /// 法线贴图;
        /// </summary>
        public Texture2D NormalMap
        {
            get { return normalMap; }
        }

        void Awake()
        {
            InitMaterial();
            OnValidate();
        }

        void Reset()
        {
            Awake();
        }

        public void OnValidate()
        {
            InitMaterial();

            SetDiffuseMap(diffuseMap);
            SetHeightMap(heightMap);
            SetNormalMap(normalMap);
        }

        void InitMaterial()
        {
            if (material == null)
                GetComponent<MeshRenderer>().sharedMaterial = material = new Material(TerrainShader);
        }

        /// <summary>
        /// 设置漫反射贴图;
        /// </summary>
        public void SetDiffuseMap(Texture2D diffuseMap)
        {
            material.SetTexture("_MainTex", diffuseMap);
            this.diffuseMap = diffuseMap;
        }

        /// <summary>
        /// 设置高度贴图;
        /// </summary>
        public void SetHeightMap(Texture2D heightMap)
        {
            material.SetTexture("_HeightTex", heightMap);
            this.heightMap = heightMap;
        }

        /// <summary>
        /// 设置法线贴图;
        /// </summary>
        public void SetNormalMap(Texture2D normalMap)
        {
            material.SetTexture("_NormalMap", normalMap);
            this.normalMap = normalMap;
        }

        /// <summary>
        /// 清空贴图引用,但是不销毁;
        /// </summary>
        public void ClearTextures()
        {
            diffuseMap = null;
            heightMap = null;
            normalMap = null;
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public void DestroyTextures()
        {
            Destroy(DiffuseMap);
            Destroy(HeightMap);
            Destroy(NormalMap);
        }

    }

}
