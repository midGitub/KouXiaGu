using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [RequireComponent(typeof(MeshRenderer)), DisallowMultipleComponent]
    public class TerrainRenderer : MonoBehaviour
    {
        TerrainRenderer()
        {
        }

        static Shader TerrainShader
        {
            get { return TerrainData.TerrainShader; }
        }

        Material material;
        [SerializeField, HideInInspector]
        Texture2D heightTexture;
        [SerializeField, HideInInspector]
        Texture2D diffuseTexture;
        [SerializeField, HideInInspector]
        Texture2D normalMap;

        /// <summary>
        /// 正在使用的材质;
        /// </summary>
        Material Material
        {
            get { return material ?? (material = new Material(TerrainShader)); }
        }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture2D DiffuseTexture
        {
            get { return diffuseTexture; }
            set { Material.SetTexture("_MainTex", value); diffuseTexture = value; }
        }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture2D HeightTexture
        {
            get { return heightTexture; }
            set { Material.SetTexture("_HeightTex", value); heightTexture = value; }
        }

        /// <summary>
        /// 法线贴图;
        /// </summary>
        public Texture2D NormalMap
        {
            get { return normalMap; }
            set { Material.SetTexture("_NormalMap", value); normalMap = value; }
        }

        void Awake()
        {
            GetComponent<MeshRenderer>().material = Material;
        }

        /// <summary>
        /// 清空贴图引用,但是不销毁;
        /// </summary>
        public void ClearTextures()
        {
            DiffuseTexture = null;
            HeightTexture = null;
            NormalMap = null;
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public void DestroyTextures()
        {
            Destroy(DiffuseTexture);
            Destroy(HeightTexture);
            Destroy(NormalMap);
        }

        void Reset()
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer.sharedMaterial == null)
                renderer.sharedMaterial = Material;
        }

    }

}
