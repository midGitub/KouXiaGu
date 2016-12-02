using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 烘焙贴图队列;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class Oven : MonoBehaviour
    {
        Oven() { }

        public Camera bakingCamera;

        public Material mixerMaterial;
        public Material heightMaterial;
        public Material shadowsAndHeightMaterial;
        public Material diffuseMaterial;

        void Awake()
        {
            bakingCamera = GetComponent<Camera>();
            bakingCamera.orthographicSize = 5;
        }



    }

}
