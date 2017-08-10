using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map.MapEdit
{

    public interface IPointSizer
    {
        /// <summary>
        /// 中心点;
        /// </summary>
        CubicHexCoord Centre { get; }

        /// <summary>
        /// 获取到选中的坐标;
        /// </summary>
        IReadOnlyList<CubicHexCoord> SelectedArea { get; }

        /// <summary>
        /// 设置新的中心点;
        /// </summary>
        void SetCentre(CubicHexCoord centre);
    }
}
