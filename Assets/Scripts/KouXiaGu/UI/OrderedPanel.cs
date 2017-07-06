using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 可排序的面板;
    /// </summary>
    [DisallowMultipleComponent]
    public class OrderedPanel : MonoBehaviour, IPointerDownHandler
    {
        OrderedPanel()
        {
        }

        /// <summary>
        /// 需要移动的目标;
        /// </summary>
        RectTransform panel;

        void Awake()
        {
            panel = GetComponent<RectTransform>();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            ActivatePanel();
        }

        /// <summary>
        /// 将焦点设置到该面板上;
        /// </summary>
        public virtual void ActivatePanel()
        {
            panel.SetAsLastSibling();
        }
    }
}
