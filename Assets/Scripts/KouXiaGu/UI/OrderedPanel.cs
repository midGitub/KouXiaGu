using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 提供顶部面板挂载;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class OrderedPanel : FocusPanel, IPointerDownHandler
    {
        OrderedPanel()
        {
        }

        public bool IsDisplay
        {
            get { return gameObject.activeSelf; }
        }

        public void ChangeDisplay()
        {
            if (IsDisplay)
            {
                HidePanel();
            }
            else
            {
                DisplayPanel();
            }
        }

        /// <summary>
        /// 显示面板,并且设置焦点;
        /// </summary>
        public void DisplayPanel()
        {
            if (!IsDisplay)
            {
                gameObject.SetActive(true);
            }
            SetOnFocus();
        }

        /// <summary>
        /// 隐藏面板;
        /// </summary>
        public void HidePanel()
        {
            if (IsDisplay)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
