using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Grids
{

    /// <summary>
    /// 坐标 元素 结构;
    /// </summary>
    public struct CoordPack<TVector, TDirection>
    {
        public CoordPack(TVector point, TDirection direction)
        {
            this.Point = point;
            this.Direction = direction;
        }

        public TVector Point { get; private set; }
        public TDirection Direction { get; private set; }

        public override string ToString()
        {
            return "[" + Point.ToString() + "," + Direction.ToString() + "]";
        }

        public static implicit operator TVector(CoordPack<TVector, TDirection> item)
        {
            return item.Point;
        }

        public static implicit operator TDirection(CoordPack<TVector, TDirection> item)
        {
            return item.Direction;
        }
    }

    /// <summary>
    /// 方向 ,坐标,元素 结构;
    /// </summary>
    public struct CoordPack<TVector, TDirection, T>
    {

        public CoordPack(CoordPack<TVector, TDirection> pack, T item)
        {
            this.Point = pack.Point;
            this.Direction = pack.Direction;
            this.Item = item;
        }

        public CoordPack(TVector point, TDirection direction, T item)
        {
            this.Point = point;
            this.Direction = direction;
            this.Item = item;
        }

        public TVector Point { get; private set; }
        public TDirection Direction { get; private set; }
        public T Item { get; private set; }

        public static implicit operator CoordPack<TVector, TDirection>(CoordPack<TVector, TDirection, T> item)
        {
            return new CoordPack<TVector, TDirection>(item.Point, item.Direction);
        }

        public override string ToString()
        {
            return "[" + Direction.ToString() + "," + Point.ToString() + "," + Item.ToString() + "]";
        }
    }

}
