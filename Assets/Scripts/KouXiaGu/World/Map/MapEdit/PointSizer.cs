using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map.MapEdit
{


    public abstract class PointSizer
    {
        public abstract IReadOnlyCollection<CubicHexCoord> SelectedOffsets { get; }

        public IEnumerable<CubicHexCoord> EnumeratePoints(CubicHexCoord centre)
        {
            foreach (var offset in SelectedOffsets)
            {
                yield return offset + centre;
            }
        }
    }
}
