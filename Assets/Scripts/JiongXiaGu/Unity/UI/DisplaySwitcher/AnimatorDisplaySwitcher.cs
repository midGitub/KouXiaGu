using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.UI
{

    /// <summary>
    /// 控制动画切换显示和隐藏状态;
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimatorDisplaySwitcher : DisplaySwitcher
    {
        protected AnimatorDisplaySwitcher()
        {
        }

        private const string DisplayBoolParameter = "IsDisplay";
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override bool IsDisplay()
        {
            return animator.GetBool(DisplayBoolParameter);
        }

        protected override void InternalDisplay()
        {
            if (animator != null)
            {
                animator.Play("Display");
                animator.SetBool(DisplayBoolParameter, true);
            }
        }

        protected override void InternalHide()
        {
            if (animator != null)
            {
                animator.Play("Hide");
                animator.SetBool(DisplayBoolParameter, false);
            }
        }
    }
}
