using UnityEngine;
using UnityEngine.EventSystems;

namespace JiongXiaGu.UI
{

    /// <summary>
    /// 用于子控件响应面板排列顺序;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SelectablePanelChild : MonoBehaviour, IPointerDownHandler
    {
        SelectablePanelChild()
        {
        }

        SelectablePanel parent;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (parent == null)
            {
                parent = GetComponentInParent<SelectablePanel>();
            }
            parent.OnFocus();
        }
    }
}
