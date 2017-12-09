using System;
using System.Collections.Generic;
using JiongXiaGu.Grids;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 有缓冲区域的显示导航;
    /// </summary>
    public class BufferDisplayGuider : MonoBehaviour
    {
        /// <summary>
        /// 重要显示区域;
        /// </summary>
        [SerializeField]
        private RectRange displayRange;

        /// <summary>
        /// 缓冲区域;
        /// </summary>
        [SerializeField]
        private RectRange bufferRange;

        public void OnValidate()
        {
            bufferRange.Height = Math.Max(bufferRange.Height, displayRange.Height);
            bufferRange.Width = Math.Max(bufferRange.Width, displayRange.Width);
        }

        public void SetCenter(RectCoord center)
        {
            displayRange.Center = center;
        }

        public IEnumerable<RectCoord> GetPointsToDisplay()
        {
            bufferRange = RectRange.Contain(bufferRange, displayRange);
            return bufferRange.Range();
        }
    }
}
