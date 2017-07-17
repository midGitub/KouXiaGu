using KouXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Terrain3D.MapEditor
{

    [DisallowMultipleComponent]
    public class SelectNodeList : ItemList<CubicHexCoord>
    {

        [SerializeField]
        SelectNodeItem prefab;
        [SerializeField]
        Transform uiParent;

        protected override UserInterfaceItem<CubicHexCoord> Prefab
        {
            get { return prefab; }
        }

        protected override Transform parent
        {
            get { return uiParent; }
        }

        [ContextMenu("test1")]
        void Test()
        {
            Add(new CubicHexCoord(0, 0, 0));
            Add(new CubicHexCoord(1, 0, -1));
            Add(new CubicHexCoord(2, 0, -2));
        }

        [ContextMenu("test2")]
        void Test2()
        {
            var pos = new CubicHexCoord[]
                {
                    new CubicHexCoord(0, 0, 0),
                    new CubicHexCoord(1, 0, -1),
                    new CubicHexCoord(2, 0, -2),
                };
            Add(pos);
            Remove(new CubicHexCoord(0, 0, 0));
        }
    }
}
