using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 对子节点的所有 IResponsive 接口进行统一操作;
    /// </summary>
    public class Responser : StandaloneUI
    {
        protected Responser() { }

        IResponsive[] responsive;


        void Awake()
        {
            responsive = GetComponentsInChildren<IResponsive>();
        }


        protected override void ConcealAction()
        {
            base.ConcealAction();
            ApplyAll();
        }

        protected override void DisplayAction()
        {
            base.DisplayAction();
            ResetAll();
        }

        void ApplyAll()
        {
            foreach (var item in responsive)
            {
                item.OnApply();
            }
        }

        void ResetAll()
        {
            foreach (var item in responsive)
            {
                item.OnReset();
            }
        }

    }

}
