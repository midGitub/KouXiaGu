using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Water;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 水特效显示,挂载物体需要在 Water 层;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class WaterChunk : Water
    {
        public enum WaterTypes
        {
            DayTime,
            NeightTime,
        }

        #region 静态;

        static Transform chunkObjectParent;
        public const string DefaultChunkName = "WaterChunk";
        public static readonly WaterTypes DefaultWaterType = WaterTypes.DayTime;
        [CustomUnityLayer("Unity标准资源指定的水特效层;")]
        public const string DefaultLayer = "Water";

        public static Transform ChunkObjectParent
        {
            get { return chunkObjectParent ?? (chunkObjectParent = new GameObject("WaterChunks").transform); }
        }

        /// <summary>
        /// 创建到一个统一的GameObject物体下;
        /// </summary>
        public static WaterChunk CreateInEditor(string name = DefaultChunkName)
        {
            WaterChunk chunk = Create(name);
            chunk.transform.SetParent(ChunkObjectParent);
            return chunk;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Create Other/WaterChunk")]
#endif
        static void _Craete()
        {
            WaterChunk chunk = Create();
            chunk.SetWaterTypes(DefaultWaterType);
            chunk.gameObject.layer = LayerMask.NameToLayer(DefaultLayer);
            chunk.transform.localScale = new Vector3(6, 1, 6);
        }

        public static WaterChunk Create(string name = DefaultChunkName)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            gameObject.name = name;
            WaterChunk chunk = gameObject.AddComponent<WaterChunk>();
            return chunk;
        }

        #endregion

        [SerializeField]
        WaterTypes waterType;
        MeshRenderer meshRenderer;

        WaterSettings settings
        {
            get { return LandformSettings.Instance.WaterSettings; }
        }

        public WaterTypes WaterType
        {
            get { return waterType; }
        }

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnValidate()
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
            SetWaterTypes(waterType);
        }

        public void SetWaterTypes(WaterTypes waterType)
        {
            Material material;
            switch (waterType)
            {
                case WaterTypes.DayTime:
                    material = settings.DaytimeMaterial;
                    break;
                case WaterTypes.NeightTime:
                    material = settings.NighttimeMaterial;
                    break;
                default:
                    throw new ArgumentException();
            }
            this.waterType = waterType;
            meshRenderer.sharedMaterial = material;
        }
    }
}
