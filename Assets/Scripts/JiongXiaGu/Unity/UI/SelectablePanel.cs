using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace JiongXiaGu.UI
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

        public static IEnumerable<SelectablePanel> Stack
        {
            get { return stack; }
        }

        /// <summary>
        /// 是否为显示状态的?
        /// </summary>
        [SerializeField]
        bool isDisplay = true;

        [SerializeField]
        UnityEvent onFocus = null;
        [SerializeField]
        UnityEvent onBlur = null;
        LinkedListNode<SelectablePanel> current;

        public bool IsDisplay
        {
            get { return isDisplay; }
        }

        public bool IsFocus
        {
            get { return current != null && stack.Last == current; }
        }

        public event UnityAction OnFocusEvent
        {
            add { onFocus.AddListener(value); }
            remove { onFocus.RemoveListener(value); }
        }

        public event UnityAction OnBlurEvent
        {
            add { onBlur.AddListener(value); }
            remove { onBlur.RemoveListener(value); }
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
        /// 使其获得焦点,获取成功则返回true,否则返回false;
        /// </summary>
        public bool OnFocus()
        {
            if (current == null)
            {
                var last = stack.Last;
                if (last != null)
                {
                    last.Value.onBlur.Invoke();
                }
                OnFocus_internal();
            }
            else
            {
                if (current != stack.Last)
                {
                    var last = stack.Last;
                    if (last != null)
                    {
                        last.Value.onBlur.Invoke();
                    }
                    stack.Remove(current);
                    OnFocus_internal();
                }
            }
            return true;
        }

        void OnFocus_internal()
        {
            current = stack.AddLast(this);
            onFocus.Invoke();
            (transform as RectTransform).SetAsLastSibling();
        }

        /// <summary>
        /// 使其失去焦点;
        /// </summary>
        public void OnBlur()
        {
            if (current != null)
            {
                if (current == stack.Last)
                {
                    var newLast = stack.Last.Previous;
                    if (newLast != null)
                    {
                        newLast.Value.onFocus.Invoke();
                    }
                }
                OnBlur_internal();
            }
        }

        void OnBlur_internal()
        {
            stack.Remove(current);
            current = null;
            onBlur.Invoke();
        }

        public override string ToString()
        {
            return "[Name:" + name + "]";
        }
    }
}
