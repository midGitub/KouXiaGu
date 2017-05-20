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
    [RequireComponent(typeof(MeshRenderer))]
    public class WaterChunk : Water
    {
        public enum WaterTypes
        {
            DayTime,
            NeightTime,
        }

        [CustomUnityLayer("Unity标准资源指定的水特效层;")]
        public const string DefaultLayer = "Water";

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
            SetWaterTypes(waterType);
        }

        void OnValidate()
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
