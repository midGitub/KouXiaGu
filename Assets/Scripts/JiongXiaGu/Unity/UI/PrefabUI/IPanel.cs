using UnityEngine;

namespace JiongXiaGu.Unity.UI
{

    public interface IPanel
    {
        bool enabled { get; }

        /// <summary>
        /// 当激活时调用;
        /// </summary>
        void OnActivate();

        /// <summary>
        /// 当取消激活时调用;
        /// </summary>
        void OnUnactivate();
    }
}
