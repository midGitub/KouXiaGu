using JiongXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using JiongXiaGu.Grids;

namespace JiongXiaGu.Terrain3D.MapEditor
{

    [DisallowMultipleComponent]
    public sealed class PointToggle : MultiselectToggle<CubicHexCoord>
    {
        PointToggle()
        {
        }

        [SerializeField]
        PointToggleGroup group;
        [SerializeField]
        Text textObject;
        [SerializeField]
        CubicHexCoord value;

        public override CubicHexCoord Value
        {
            get { return value; }
            protected set { this.value = value; }
        }

        protected override MultiselectToggleGroup<CubicHexCoord> Group
        {
            get { return group; }
            set { group = (PointToggleGroup)value; }
        }

        protected override void Awake()
        {
            base.Awake();
            if (textObject != null)
            {
                textObject.text = Value.ToString();
            }
        }
    }
}
