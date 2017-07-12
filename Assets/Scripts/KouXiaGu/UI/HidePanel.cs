using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 隐藏窗口;
    /// </summary>
    [DisallowMultipleComponent]
    public class HidePanel : MonoBehaviour, IPointerDownHandler
    {
        HidePanel()
        {
        }

        SelectablePanel parent;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (parent == null)
            {
                parent = GetComponentInParent<SelectablePanel>();
            }
            parent.Hide();
        }
    }
}
