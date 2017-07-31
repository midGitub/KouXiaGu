using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map.MapEdit
{

    public class HexSpiralSizer : IPointSizer
    {
        public CubicHexCoord Centre { get; set; }
        public int Size { get; set; }

        public IEnumerable<CubicHexCoord> GetSelectedArea()
        {
            return CubicHexCoord.Spiral(Centre, Size);
        }
    }
}
