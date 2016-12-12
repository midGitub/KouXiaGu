using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 坐标 元素 结构;
    /// </summary>
    public struct CoordPack<TVector, TDirection>
    {
        public CoordPack(TVector point, TDirection item)
        {
            this.Point = point;
            this.Item = item;
        }

        public TVector Point { get; private set; }
        public TDirection Item { get; private set; }

        public override string ToString()
        {
            return "[" + Point.ToString() + "," + Item.ToString() + "]";
        }
    }

    /// <summary>
    /// 方向 ,坐标,元素 结构;
    /// </summary>
    public struct CoordPack<TVector, TDirection, T>
    {

        public CoordPack(TVector point, TDirection direction, T item)
        {
            this.Point = point;
            this.Direction = direction;
            this.Item = item;
        }

        public TDirection Direction { get; private set; }
        public TVector Point { get; private set; }
        public T Item { get; private set; }

        public static implicit operator CoordPack<TVector, T>(CoordPack<TVector, TDirection, T> item)
        {
            return new CoordPack<TVector, T>(item.Point, item.Item);
        }

        public override string ToString()
        {
            return "[" + Direction.ToString() + "," + Point.ToString() + "," + Item.ToString() + "]";
        }
    }

}
