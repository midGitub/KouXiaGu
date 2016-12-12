using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 多个矩形组成一个块;
    /// </summary>
    public struct Chunk<T>
        where T : IGridCoord, new()
    {

        readonly int width;
        readonly int height;

        /// <summary>
        /// 矩形宽度;
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// 矩形高度;
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="size">需要为奇数</param>
        public Chunk(int size)
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
        public Chunk(int width, int height)
        {
            if ((width & 1) != 1 || (height & 1) != 1)
                throw new ArgumentOutOfRangeException("参数需要为奇数;");

            this.width = width;
            this.height = height;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Chunk<T>))
                return false;
            return this == (Chunk<T>)obj;
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
        public RectCoord GetChunk(T coord)
        {
            short x = (short)Math.Round(coord.X / (float)width);
            short y = (short)Math.Round(coord.Y / (float)height);
            return new RectCoord(x, y);
        }

        /// <summary>
        /// 获取到块的中心;
        /// </summary>
        public T GetCenter(RectCoord chunk)
        {
            int x = chunk.x * width;
            int y = chunk.y * height;
            return Get((short)x, (short)y);
        }

        /// <summary>
        /// 获取到块内所有的点;
        /// </summary>
        IEnumerable<T> ChunkRange(RectCoord chunk)
        {
            T center = GetCenter(chunk);
            T southwest = SouthwestAdge(center);
            T northeast = NorthEastAdge(center);

            for (short x = southwest.X; x <= northeast.X; x++)
            {
                for (short y = southwest.Y; y <= northeast.Y; y++)
                {
                    yield return Get(x, y);
                }
            }
        }

        T Get(short x, short y)
        {
            T item = new T();
            item.SetValue(x, y);
            return item;
        }

        T SouthwestAdge(T coord)
        {
            int x = coord.X - width / 2;
            int y = coord.Y - height / 2;
            return Get((short)x, (short)y);
        }

        T NorthEastAdge(T coord)
        {
            int x = coord.X + width / 2;
            int y = coord.Y + height / 2;
            return Get((short)x, (short)y);
        }

        public static bool operator ==(Chunk<T> a, Chunk<T> b)
        {
            return a.width == b.width
                && a.height == b.height;
        }

        public static bool operator !=(Chunk<T> a, Chunk<T> b)
        {
            return !(a == b);
        }

    }

}
