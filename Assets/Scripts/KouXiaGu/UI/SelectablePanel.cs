using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 可进行选择的面板;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SelectablePanel : MonoBehaviour
    {
        SelectablePanel()
        {
        }

        static readonly LinkedList<SelectablePanel> stack = new LinkedList<SelectablePanel>();

        [SerializeField]
        bool isDisplay = true;
        [SerializeField]
        UnityEvent onFocus = null;
        [SerializeField]
        UnityEvent onBlur = null;
        LinkedListNode<SelectablePanel> current;

        public bool IsFocus
        {
            get { return current != null && stack.Last == current; }
        }

        public UnityEvent OnFocusEvent
        {
            get { return onFocus; }
        }

        public UnityEvent OnBlurEvent
        {
            get { return onBlur; }
        }

        void OnValidate()
        {
            SetDisplay_internal(isDisplay);
        }

        void OnDestroy()
        {
            if (current != null)
            {
                stack.Remove(current);
                current = null;
            }
        }

        /// <summary>
        /// 显示面板,并且获得焦点;
        /// </summary>
        public void Display()
        {
            isDisplay = true;
            SetDisplay_internal(isDisplay);
            OnFocus();
        }

        /// <summary>
        /// 隐藏面板,并把焦点让出;
        /// </summary>
        public void Hide()
        {
            isDisplay = false;
            SetDisplay_internal(isDisplay);
            OnBlur();
        }

        void SetDisplay_internal(bool isDisplay)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isDisplay);
            }
        }

        /// <summary>
        /// 当获得焦点时调用;
        /// </summary>
        public void OnFocus()
        {
            if (current == null)
            {
                var last = stack.Last;
                if (last != null)
                {
                    last.Value.OnBlur_internal();
                }
                current = stack.AddLast(this);
                (transform as RectTransform).SetAsLastSibling();
                onFocus.Invoke();
            }
            else
            {
                if (current != stack.Last)
                {
                    var last = stack.Last;
                    if (last != null)
                    {
                        last.Value.OnBlur_internal();
                    }
                    stack.Remove(current);
                    current = stack.AddLast(this);
                    (transform as RectTransform).SetAsLastSibling();
                    onFocus.Invoke();
                }
            }
        }

        /// <summary>
        /// 当失去焦点时调用;
        /// </summary>
        void OnBlur()
        {
            if (current != null)
            {
                if (current == stack.Last)
                {
                    var newLast = stack.Last.Previous;
                    if (newLast != null)
                    {
                        newLast.Value.OnFocus_internal();
                    }
                }
                onBlur.Invoke();
                stack.Remove(current);
                current = null;
            }
        }

        void OnFocus_internal()
        {
            onFocus.Invoke();
        }

        void OnBlur_internal()
        {
            onBlur.Invoke();
        }
    }
}
