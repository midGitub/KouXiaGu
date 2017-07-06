using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 排序的面板;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class OrderedPanel : MonoBehaviour, IPointerDownHandler
    {
        protected OrderedPanel()
        {
        }

        /// <summary>
        /// 需要移动的目标;
        /// </summary>
        RectTransform panel;
        Action onFocus;

        public bool IsDisplay
        {
            get { return gameObject.activeSelf; }
        }

        public event Action OnFocus
        {
            add { onFocus += value; }
            remove { onFocus -= value; }
        }

        void Awake()
        {
            panel = GetComponent<RectTransform>();
        }

        void OnEnable()
        {
            ActivatePanel();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            ActivatePanel();
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
            ActivatePanel();
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

        /// <summary>
        /// 将焦点设置到该面板上;
        /// </summary>
        public void ActivatePanel()
        {
            panel.SetAsLastSibling();

            if (onFocus != null)
            {
                onFocus();
            }
        }
    }
}
