using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JiongXiaGu.UI
{

    /// <summary>
    /// 隐藏窗口;
    /// </summary>
    [DisallowMultipleComponent]
    public class HidePanelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        HidePanelButton()
        {
        }

        SelectablePanel parent;
        IDisposable cursorDefaultStyleDisposer;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (parent == null)
            {
                parent = GetComponentInParent<SelectablePanel>();
            }
            parent.Hide();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (cursorDefaultStyleDisposer == null)
            {
                cursorDefaultStyleDisposer = CustomCursor.Instance.SetCursor(CursorType.Default);
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (cursorDefaultStyleDisposer != null)
            {
                cursorDefaultStyleDisposer.Dispose();
                cursorDefaultStyleDisposer = null;
            }
        }
    }
}
