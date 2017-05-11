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
        [SerializeField]
        MeshRenderer waterMeshRenderer;
        [SerializeField]
        Material daytimeMaterial;
        [SerializeField]
        Material nighttimeMaterial;
        [SerializeField, Range(1, 100)]
        int size = 3;

        public Material DaytimeMaterial
        {
            get { return daytimeMaterial; }
        }

        public Material NighttimeMaterial
        {
            get { return nighttimeMaterial; }
        }

        public Water.WaterMode WaterMode
        {
            get { return waterScript.waterMode; }
            set { waterScript.waterMode = value; }
        }

        public bool IsDisplay
        {
            get { return waterScript.enabled; }
            set { waterScript.enabled = value; }
        }

        void Start()
        {
            SetSize(size);
            SetWaterTypes(WaterTypes.DayTime);
        }

        public void SetSize(int size)
        {
            transform.localScale = new Vector3(size, 1, size);
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
