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

        public RectRange(ShortVector2 center, ShortVector2 range): this()
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
            this.southwest = new ShortVector2(center.X - range.X, center.Y - range.Y);
            this.northeast = new ShortVector2(center.X + range.X, center.Y + range.Y);
        }

        /// <summary>
        /// 这个点是否超出了定义的最大范围;超出返回true;
        /// </summary>
        public bool IsOutRange(ShortVector2 point)
        {
            return point.X < southwest.X || point.Y < southwest.Y ||
                point.X > northeast.X || point.Y > northeast.Y;
        }

    }

}
