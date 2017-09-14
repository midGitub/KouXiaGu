using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 区别于UnityEngine.Rect,表示带一个中心点的矩形;
    /// </summary>
    public struct RectRange
    {
        public RectRange(RectCoord center, int width, int height) : this(center.x, center.y, width, height)
        {
        }

        public RectRange(int width, int height) : this(0, 0, width, height)
        {
        }

        public RectRange(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        [SerializeField]
        int x;
        [SerializeField]
        int y;
        [SerializeField]
        int width;
        [SerializeField]
        int height;

        /// <summary>
        /// 矩形中心坐标的X;
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// 矩形中心坐标的Y;
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// 左或右边存在多少单位空隙;
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// 矩形实际宽度;
        /// </summary>
        public int RealWidth
        {
            get { return width * 2 + 1; }
        }

        /// <summary>
        /// 上或下边存在多少单位空隙;
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// 矩形实际高度;
        /// </summary>
        public int RealHeight
        {
            get { return height * 2 + 1; }
        }

        /// <summary>
        /// 最左上角坐标;
        /// </summary>
        public RectCoord Northeast
        {
            get { return new RectCoord(x - width, y + height); }
        }

        /// <summary>
        /// 最右上角坐标;
        /// </summary>
        public RectCoord Northwest
        {
            get { return new RectCoord(x + width, y + height); }
        }

        /// <summary>
        /// 最左下角坐标;
        /// </summary>
        public RectCoord Southeast
        {
            get { return new RectCoord(x - width, y - height); }
        }

        /// <summary>
        /// 最右下角坐标;
        /// </summary>
        public RectCoord Southwest
        {
            get { return new RectCoord(x + width, y - height); }
        }

        /// <summary>
        /// 使 parent 的范围包含 child 的范围(最小位移改变parent的中心坐标);
        /// </summary>
        /// <param name="parent">长宽比child大的范围;</param>
        /// <param name="child">较小的范围;</param>
        /// <returns>改变中心坐标以后的范围</returns>
        public static RectRange Contain(RectRange parent, RectRange child)
        {
            RectCoord offset;

            offset = child.Northeast - parent.Northeast;
            if (offset.x > 0)
            {
                parent.x += offset.x;
            }
            if (offset.y > 0)
            {
                parent.y += offset.y;
            }

            offset = child.Southwest - parent.Southwest;
            if (offset.x < 0)
            {
                parent.x += offset.x;
            }
            if (offset.y < 0)
            {
                parent.y += offset.y;
            }

            return parent;
        }
    }
}
