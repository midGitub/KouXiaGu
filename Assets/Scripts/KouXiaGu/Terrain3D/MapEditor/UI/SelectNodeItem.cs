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
    public class SelectNodeItem : UserInterfaceItem<CubicHexCoord>
    {

        [SerializeField]
        SelectNodeList parentList;
        [SerializeField]
        CubicHexCoord value;
        [SerializeField]
        Text textObject;
        [SerializeField]
        Button deleteButtom;

        public override CubicHexCoord Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnValidate();
            }
        }

        protected override ItemList<CubicHexCoord> parent
        {
            get { return parentList; }
            set { parentList = (SelectNodeList)value; }
        }

        protected override void Awake()
        {
            base.Awake();
            deleteButtom.onClick.AddListener(OnDeleteButtomClick);
        }

        void OnDeleteButtomClick()
        {
            if (parentList != null)
            {
                parentList.Remove(this);
            }
        }

        void OnValidate()
        {
            textObject.text = value.ToString();
        }
    }
}
