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
    public sealed class MovablePanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
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

        public bool isMovable = true;
        Vector2 originalLocalPointerPosition;
        Vector3 originalPanelLocalPosition;
        bool isDragging;
        RootCanvas rootCanvas;
        IDisposable edgeAlignmentDisposer;

        public bool IsMovable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }

        void OnEnable()
        {
            rootCanvas = GetComponentInParent<RootCanvas>();
            edgeAlignmentDisposer = rootCanvas.EdgeAlignment.Subscribe(panel);
        }

        void OnDisable()
        {
            edgeAlignmentDisposer.Dispose();
            edgeAlignmentDisposer = null;
        }

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
            isDragging = true;
            originalPanelLocalPosition = panel.localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (isDragging && isMovable)
            {
                Vector2 localPointerPosition;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position, eventData.pressEventCamera, out localPointerPosition))
                {
                    Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                    panel.localPosition = originalPanelLocalPosition + offsetToOriginal;
                }
                rootCanvas.EdgeAlignment.Clamp(panel);
                ClampPanel();
            }
        }

        void ClampPanel()
        {
            Vector3 pos = panel.localPosition;
            Vector2 minPosition = parent.rect.min - panel.rect.min;
            Vector2 maxPosition = parent.rect.max - panel.rect.max;
            pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);
            panel.localPosition = pos;
        }
    }
}
