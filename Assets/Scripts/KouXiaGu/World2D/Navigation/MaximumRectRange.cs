using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 设定一个范围,可以检查是否超出这个范围;
    /// </summary>
    public struct MaximumRectRange
    {

        public MaximumRectRange(ShortVector2 center, ShortVector2 range): this()
        {
            SetMaximumRange(center, range);
        }

        /// <summary>
        /// x y 值允许的最大值;
        /// </summary>
        ShortVector2 northeast;
        /// <summary>
        /// x y 值允许的最小值;
        /// </summary>
        ShortVector2 southwest;

        /// <summary>
        /// 设置到最大范围;
        /// </summary>
        public void SetMaximumRange(ShortVector2 center, ShortVector2 range)
        {
            this.southwest = new ShortVector2(center.x - range.x, center.y - range.y);
            this.northeast = new ShortVector2(center.x + range.x, center.y + range.y);
        }

        /// <summary>
        /// 这个点是否超出了定义的最大范围;超出返回true;
        /// </summary>
        public bool IsOutRange(ShortVector2 point)
        {
            return point.x < southwest.x || point.y < southwest.y ||
                point.x > northeast.x || point.y > northeast.y;
        }

    }

}
