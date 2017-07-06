using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 关闭 OrderedPanel 窗口;
    /// </summary>
    [DisallowMultipleComponent]
    public class ClosePanel : MonoBehaviour, IPointerDownHandler
    {
        ClosePanel()
        {
        }

        OrderedPanel parent;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (parent == null)
            {
                parent = GetComponentInParent<OrderedPanel>();
            }
            parent.HidePanel();
        }
    }
}
