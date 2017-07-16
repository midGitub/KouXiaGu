using KouXiaGu.Grids;
using KouXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Terrain3D.MapEditor
{

    [DisallowMultipleComponent]
    public class PointToggleGroup : MultiselectToggleGroup<CubicHexCoord>
    {
        PointToggleGroup()
        {
        }

        [SerializeField]
        PointToggle pointTogglePrefab;

        protected override MultiselectToggle<CubicHexCoord> TogglePrefab
        {
            get { return pointTogglePrefab; }
        }

        [ContextMenu("test")]
        void Test()
        {
            Create(new CubicHexCoord(0, 0, 0));
            Create(new CubicHexCoord(1, 0, -1));
            Create(new CubicHexCoord(2, 0, -2));
        }
    }
}
