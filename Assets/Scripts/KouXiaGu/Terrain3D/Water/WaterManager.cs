using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Water;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class WaterManager : MonoBehaviour
    {
        WaterManager()
        {
        }

        public enum WaterTypes
        {
            DayTime,
            NeightTime,
        }

        [SerializeField]
        Water waterScript;
        MeshRenderer waterMeshRenderer;
        [SerializeField]
        Material daytimeMaterial;
        [SerializeField]
        Material nighttimeMaterial;
        [SerializeField]
        WaterTypes waterType;
        [SerializeField]
        float seaLevel;

        public Material DaytimeMaterial
        {
            get { return daytimeMaterial; }
        }

        public Material NighttimeMaterial
        {
            get { return nighttimeMaterial; }
        }

        public float Size
        {
            get { return waterScript.transform.localScale.x; }
            set { waterScript.transform.localScale = new Vector3(value, 1, value); }
        }

        public WaterTypes WaterType
        {
            get { return waterType; }
        }

        public Water.WaterMode WaterMode
        {
            get { return waterScript.waterMode; }
            set { waterScript.waterMode = value; }
        }

        public bool IsDisplay
        {
            get { return waterScript.gameObject.activeSelf; }
            set { waterScript.gameObject.SetActive(value); }
        }

        /// <summary>
        /// 水资源显示块的位置;世界坐标;
        /// </summary>
        public Vector3 ChunkPosition
        {
            get
            {
                Vector3 pos = waterScript.transform.position;
                pos.y = seaLevel;
                return pos;
            }
            set
            {
                Vector3 pos = value;
                pos.y = seaLevel;
                waterScript.transform.position = pos;
            }
        }

        void Awake()
        {
            waterMeshRenderer = waterScript.GetComponent<MeshRenderer>();
            SetWaterTypes(waterType);
        }

        public void SetWaterTypes(WaterTypes type)
        {
            Material material;

            switch (type)
            {
                case WaterTypes.DayTime:
                    material = daytimeMaterial;
                    break;
                case WaterTypes.NeightTime:
                    material = nighttimeMaterial;
                    break;
                default:
                    throw new ArgumentException();
            }

            waterMeshRenderer.sharedMaterial = material;
        }

    }

}
