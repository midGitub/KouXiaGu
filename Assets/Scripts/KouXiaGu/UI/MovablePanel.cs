using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 可以移动的面板;
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public sealed class MovablePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        MovablePanel()
        {
        }

        /// <summary>
        /// 需要移动的目标;
        /// </summary>
        public RectTransform panel;

        /// <summary>
        /// 区域限制;
        /// </summary>
        public RectTransform parent;

        Vector2 originalLocalPointerPosition;
        Vector3 originalPanelLocalPosition;
        bool isDragging;

        [ContextMenu("自动设置参数")]
        void InitParameter()
        {
            if (panel == null)
            {
                panel = GetComponent<RectTransform>();
            }
            if (parent == null)
            {
                parent = transform.parent as RectTransform;
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == gameObject)
            {
                isDragging = true;
            }
            originalPanelLocalPosition = panel.localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                Vector2 localPointerPosition;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position, eventData.pressEventCamera, out localPointerPosition))
                {
                    Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                    panel.localPosition = originalPanelLocalPosition + offsetToOriginal;
                }
                ClampPanel();
            }
        }

        void ClampPanel()
        {
            Vector3 pos = panel.localPosition;
            Vector3 minPosition = parent.rect.min - panel.rect.min;
            Vector3 maxPosition = parent.rect.max - panel.rect.max;
            pos.x = Mathf.Clamp(panel.localPosition.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(panel.localPosition.y, minPosition.y, maxPosition.y);
            panel.localPosition = pos;
        }
    }
}
