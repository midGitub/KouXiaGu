using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Water;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 水特效显示,挂载物体需要在 Water 层;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer))]
    public class WaterChunk : Water
    {
        [CustomUnityLayer("Unity标准资源指定的水特效层;")]
        public const string DefaultLayer = "Water";

        public float Size
        {
            get { return transform.localScale.x; }
            set { transform.localScale = new Vector3(value, 1, value); }
        }
    }
}
