using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Grids
{

    /// <summary>
    /// 六边形半径范围;
    /// </summary>
    public class HexRadiusRange : IRange<CubicHexCoord>
    {

        public HexRadiusRange()
        {
        }

        /// <summary>
        /// 若 radius 为负数则是相反的范围;
        /// </summary>
        public HexRadiusRange(int radius, CubicHexCoord starting)
        {
            SetRange(radius, starting);
        }

        public int Radius { get; set; }

        public CubicHexCoord Starting { get; set; }

        int[] boundary;

        public void SetRange(int radius, CubicHexCoord starting)
        {
            this.Radius = radius;
            this.Starting = starting;

            this.boundary = GetBoundary(radius, starting);
        }

        int[] GetBoundary(int radius, CubicHexCoord starting)
        {
            int[] boundary = new int[6];

            boundary[0] = starting.X + radius;
            boundary[1] = starting.Y + radius;
            boundary[2] = starting.Z + radius;

            boundary[3] = starting.X - radius;
            boundary[4] = starting.Y - radius;
            boundary[5] = starting.Z - radius;

            return boundary;
        }

        public bool IsOutRange(CubicHexCoord point)
        {
            return
                boundary[0] < point.X ||
                boundary[1] < point.Y ||
                boundary[2] < point.Z ||

                boundary[3] > point.X ||
                boundary[4] > point.Y ||
                boundary[5] > point.Z;
        }

    }

}
