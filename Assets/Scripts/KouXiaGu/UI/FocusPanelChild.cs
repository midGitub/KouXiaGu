using UnityEngine;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 用于子控件响应面板排列顺序;
    /// </summary>
    [DisallowMultipleComponent]
    public class FocusPanelChild : MonoBehaviour, IPointerDownHandler
    {
        FocusPanelChild()
        {
        }

        FocusPanel parent;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (parent == null)
            {
                parent = GetComponentInParent<FocusPanel>();
            }
            parent.SetOnFocus();
        }
    }
}
