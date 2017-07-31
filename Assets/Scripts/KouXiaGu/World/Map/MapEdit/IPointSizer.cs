using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map.MapEdit
{

    public interface IPointSizer
    {
        CubicHexCoord Centre { get; set; }
        IEnumerable<CubicHexCoord> GetSelectedArea();
    }
}
