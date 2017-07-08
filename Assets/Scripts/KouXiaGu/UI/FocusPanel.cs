using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 可作为焦点的面板;
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class FocusPanel : MonoBehaviour, IPointerDownHandler
    {
        protected FocusPanel()
        {
        }

        /// <summary>
        /// 当获得焦点时调用;
        /// </summary>
        [SerializeField]
        UnityEvent onFocus;

        /// <summary>
        /// 当获得焦点时调用;
        /// </summary>
        public UnityEvent OnFocus
        {
            get { return onFocus; }
        }

        void OnEnable()
        {
            SetOnFocus();
        }

        /// <summary>
        /// 设置为焦点;
        /// </summary>
        public void SetOnFocus()
        {
            _OnFocus();
        }

        /// <summary>
        /// 设置为最顶部的面板;
        /// </summary>
        void SetAsTopPanel()
        {
            (transform as RectTransform).SetAsLastSibling();
        }

        void _OnFocus()
        {
            SetAsTopPanel();
            onFocus.Invoke();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _OnFocus();
        }
    }
}
