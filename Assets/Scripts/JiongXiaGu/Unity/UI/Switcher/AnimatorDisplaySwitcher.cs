using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.UI
{


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

        [ContextMenu(nameof(Display))]
        public override void Display()
        {
            animator.SetBool(DisplayBoolParameter, true);
        }

        [ContextMenu(nameof(Hide))]
        public override void Hide()
        {
            animator.SetBool(DisplayBoolParameter, false);
        }
    }
}
