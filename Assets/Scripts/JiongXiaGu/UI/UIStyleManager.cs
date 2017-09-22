using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.UI
{

    /// <summary>
    /// UI风格管理;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIStyleManager : UnitySington<UIStyleManager>
    {
        UIStyleManager()
        {
        }

        public SelectablePanelTitleStyleInfo SelectablePanelTitleStyleInfo;

        void Awake()
        {
            SetInstance(this);
        }
    }
}
