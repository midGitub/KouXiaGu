using UnityEngine;

namespace JiongXiaGu.Unity.UI
{


    [DisallowMultipleComponent]
    public abstract class Panel : MonoBehaviour
    {
        protected Panel()
        {
        }

        /// <summary>
        /// 当激活时调用;
        /// </summary>
        public abstract void OnActivate();

        /// <summary>
        /// 当取消激活时调用;
        /// </summary>
        public abstract void OnUnactivate();
    }
}
