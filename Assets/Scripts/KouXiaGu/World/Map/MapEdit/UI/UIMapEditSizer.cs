using KouXiaGu.Grids;
using KouXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 提供编辑的坐标;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIMapEditSizer : MonoBehaviour
    {
        UIMapEditSizer()
        {
        }

        [SerializeField]
        SelectablePanel panel;
        List<CubicHexCoord> selectedArea;
        public IReadOnlyCollection<CubicHexCoord> SelectedArea { get; private set; }
        public PointSizer PointSizer { get; private set; }

        void Awake()
        {
            selectedArea = new List<CubicHexCoord>()
            {
                new CubicHexCoord(0, 0, 0),
                new CubicHexCoord(0, 1, -1),
            };
            SelectedArea = selectedArea.AsReadOnlyCollection();
        }

        public void SetPointSizer(PointSizer sizer)
        {
            PointSizer = sizer;
        }
    }
}
