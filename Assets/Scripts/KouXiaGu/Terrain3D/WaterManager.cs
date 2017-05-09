using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Water;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Water))]
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
        Material daytimeMaterial;
        [SerializeField]
        Material nighttimeMaterial;
        [SerializeField, Range(1, 100)]
        int size = 3;

        MeshRenderer _renderer;
        Water waterScript;

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

        void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            waterScript = GetComponent<Water>();
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

            _renderer.sharedMaterial = material;
        }

    }

}
