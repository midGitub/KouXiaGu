using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 设定一个范围,可以检查是否超出这个范围;
    /// </summary>
    public struct RectRange
    {

        public RectRange(RectCoord center, RectCoord range): this()
        {
            SetMaximumRange(center, range);
        }

        /// <summary>
        /// x y 值允许的最大值;
        /// </summary>
        RectCoord northeast;
        /// <summary>
        /// x y 值允许的最小值;
        /// </summary>
        RectCoord southwest;

        /// <summary>
        /// 设置到最大范围;
        /// </summary>
        public void SetMaximumRange(RectCoord center, RectCoord range)
        {
            this.southwest = new RectCoord(center.x - range.x, center.y - range.y);
            this.northeast = new RectCoord(center.x + range.x, center.y + range.y);
        }

        /// <summary>
        /// 这个点是否超出了定义的最大范围;超出返回true;
        /// </summary>
        public bool IsOutRange(RectCoord point)
        {
            return point.x < southwest.x || point.y < southwest.y ||
                point.x > northeast.x || point.y > northeast.y;
        }

    }

}
