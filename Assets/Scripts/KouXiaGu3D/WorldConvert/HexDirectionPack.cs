using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public struct HexDirectionPack<TVector, T>
    {

        public HexDirectionPack(HexDirection direction, TVector point, T item)
        {
            this.Direction = direction;
            this.Point = point;
            this.Item = item;
        }

        public HexDirection Direction { get; private set; }
        public TVector Point { get; private set; }
        public T Item { get; private set; }
    }

}
