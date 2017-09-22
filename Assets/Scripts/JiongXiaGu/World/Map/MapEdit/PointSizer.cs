using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using JiongXiaGu.Terrain3D;
using UnityEngine;

namespace JiongXiaGu.World.Map.MapEdit
{

    [Serializable]
    public abstract class PointSizer
    {
        [SerializeField]
        BoundaryMesh terrainBoundaryMesh;

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
