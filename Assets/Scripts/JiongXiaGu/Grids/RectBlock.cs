using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Grids
{

    /// <summary>
    /// 将一个网格的多个元素分为块;
    /// </summary>
    public struct RectBlock
    {

        readonly int width;
        readonly int height;

        /// <summary>
        /// 块宽度;
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// 块高度;
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// 块内元素个数;
        /// </summary>
        public int ChunkElementCount
        {
            get { return width * height; }
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="size">需要为奇数</param>
        public RectBlock(int size)
        {
            if ((size & 1) != 1)
                throw new ArgumentOutOfRangeException("参数需要为奇数;");

            this.width = this.height = size;
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="width">需要为奇数</param>
        /// <param name="height">需要为奇数</param>
        public RectBlock(int width, int height)
        {
            if ((width & 1) != 1 || (height & 1) != 1)
                throw new ArgumentOutOfRangeException("参数需要为奇数;");

            this.width = width;
            this.height = height;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RectBlock))
                return false;
            return this == (RectBlock)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + width + "," + height + ")";
        }

        /// <summary>
        /// 获取到所属于的块;
        /// </summary>
        public RectCoord GetChunk(RectCoord coord)
        {
            short x = (short)Math.Round(coord.x / (float)width);
            short y = (short)Math.Round(coord.y / (float)height);
            return new Grids.RectCoord(x, y);
        }

        /// <summary>
        /// 获取到块的中心;
        /// </summary>
        public RectCoord GetCenter(RectCoord chunk)
        {
            int x = chunk.x * width;
            int y = chunk.y * height;
            return new RectCoord((short)x, (short)y);
        }

        /// <summary>
        /// 获取到块内所有的点;
        /// </summary>
        IEnumerable<RectCoord> ChunkRange(RectCoord chunk)
        {
            RectCoord center = GetCenter(chunk);
            RectCoord southwest = SouthwestAdge(center);
            RectCoord northeast = NorthEastAdge(center);

            for (short x = southwest.x; x <= northeast.x; x++)
            {
                for (short y = southwest.y; y <= northeast.y; y++)
                {
                    yield return new RectCoord(x, y);
                }
            }
        }

        RectCoord SouthwestAdge(RectCoord coord)
        {
            int x = coord.x - width / 2;
            int y = coord.y - height / 2;
            return new RectCoord((short)x, (short)y);
        }

        RectCoord NorthEastAdge(RectCoord coord)
        {
            int x = coord.x + width / 2;
            int y = coord.y + height / 2;
            return new RectCoord((short)x, (short)y);
        }

        public static bool operator ==(RectBlock a, RectBlock b)
        {
            return a.width == b.width
                && a.height == b.height;
        }

        public static bool operator !=(RectBlock a, RectBlock b)
        {
            return !(a == b);
        }

    }

}
