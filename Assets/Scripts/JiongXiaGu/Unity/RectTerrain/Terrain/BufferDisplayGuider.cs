using System;
using System.Collections.Generic;
using JiongXiaGu.Grids;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 有缓冲区域的显示导航;
    /// </summary>
    [Serializable]
    public class BufferDisplayGuider : MonoBehaviour, IDisplayGuider<RectCoord>
    {
        /// <summary>
        /// 重要显示区域;
        /// </summary>
        [SerializeField]
        RectRange displayRange;

        /// <summary>
        /// 缓冲区域;
        /// </summary>
        [SerializeField]
        RectRange bufferRange;

        bool isChanged;
        List<RectCoord> displayPoints;

        public void Awake()
        {
            displayPoints = new List<RectCoord>();
        }

        public void OnValidate()
        {
            if (bufferRange.Height < displayRange.Height)
            {
                bufferRange.Height = displayRange.Height;
            }
            if (bufferRange.Width < displayRange.Width)
            {
                bufferRange.Width = displayRange.Width;
            }
            isChanged = true;
        }

        public void SetCenter(RectCoord center)
        {
            if (displayRange.Center != center)
            {
                isChanged = true;
            }
            displayRange.Center = center;
        }

        IReadOnlyCollection<RectCoord> IDisplayGuider<RectCoord>.GetPointsToDisplay()
        {
            if (isChanged)
            {
                isChanged = false;
                bufferRange = RectRange.Contain(bufferRange, displayRange);
                var points = bufferRange.Range();
                displayPoints.Clear();
                displayPoints.AddRange(points);
            }
            return displayPoints;
        }
    }
}
