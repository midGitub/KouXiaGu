using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘培;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LandformBaker : MonoBehaviour
    {
        /// <summary>
        /// 透明的黑色颜色;
        /// </summary>
        public static readonly Color BlackTransparent = new Color(0, 0, 0, 0);

        /// <summary>
        /// 地平线颜色;
        /// </summary>
        public static readonly Color Horizon = new Color(0.5f, 0.5f, 0.5f, 1);

        LandformBaker()
        {
        }

        [SerializeField]
        BakeCamera bakeCamera = null;
        [SerializeField]
        BakeLandform landform = null;
        [SerializeField]
        Stopwatch runtimeStopwatch = null;
        Coroutine bakeCoroutine;

        internal BakeLandform Landform
        {
            get { return landform; }
        }

        void Awake()
        {
            bakeCamera.Initialize();
            bakeCoroutine = new Coroutine(BakeCoroutine());
        }

        void Update()
        {
            bakeCoroutine.Next();
        }

        public void Reset()
        {
            bakeCamera.Settings.UpdataTextureSize();
        }

        IEnumerator BakeCoroutine()
        {
            throw new NotImplementedException();
        }
    }

}
