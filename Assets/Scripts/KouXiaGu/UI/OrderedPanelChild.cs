using UnityEngine;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 用于子控件响应面板排列顺序;
    /// </summary>
    [DisallowMultipleComponent]
    public class OrderedPanelChild : MonoBehaviour, IPointerDownHandler
    {
        OrderedPanelChild()
        {
        }

        OrderedPanel parent;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (parent == null)
            {
                parent = GetComponentInParent<OrderedPanel>();
            }
            parent.ActivatePanel();
        }
    }
}
