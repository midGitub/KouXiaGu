using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.UI
{


    public class SimpleDisplaySwitcher : DisplaySwitcher
    {
        protected SimpleDisplaySwitcher()
        {
        }

        [SerializeField]
        private GameObject target;

        public override bool IsDisplay()
        {
            return target.activeSelf;
        }

        protected override void InternalDisplay()
        {
            if (target != null)
            {
                target.SetActive(true);
            }
        }

        protected override void InternalHide()
        {
            if (target != null)
            {
                target.SetActive(false);
            }
        }
    }
}
