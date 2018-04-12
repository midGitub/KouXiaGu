using UnityEngine;

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

        private Animator animator;
        [SerializeField]
        private bool isDisplay;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override bool IsDisplay()
        {
            return isDisplay;
        }

        protected override void InternalDisplay()
        {
            if (animator != null)
            {
                animator.Play("Display");
                isDisplay = true;
            }
        }

        protected override void InternalHide()
        {
            if (animator != null)
            {
                animator.Play("Hide");
                isDisplay = false;
            }
        }
    }
}
