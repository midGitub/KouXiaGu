﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 显示坐标提供;
    /// </summary>
    public interface IDisplayGuider<TPoint>
    {
        /// <summary>
        /// 获取到需要显示的坐标;
        /// </summary>
        IReadOnlyCollection<TPoint> GetPointsToDisplay();
    }
}
