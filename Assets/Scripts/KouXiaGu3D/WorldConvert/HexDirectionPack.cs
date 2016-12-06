using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public struct HexDirectionPack<TVector, T>
    {

        public HexDirectionPack(HexDirections direction, TVector point, T item)
        {
            this.Direction = direction;
            this.Point = point;
            this.Item = item;
        }

        public HexDirections Direction { get; private set; }
        public TVector Point { get; private set; }
        public T Item { get; private set; }

        public static implicit operator HexDirections(HexDirectionPack<TVector, T> pack)
        {
            return pack.Direction;
        }

        public static implicit operator TVector(HexDirectionPack<TVector, T> pack)
        {
            return pack.Point;
        }

        public static implicit operator T(HexDirectionPack<TVector, T> pack)
        {
            return pack.Item;
        }

        public override string ToString()
        {
            return "[" + Direction.ToString() + "," + Point.ToString() + "," + Item.ToString() + "]";
        }

    }

}
