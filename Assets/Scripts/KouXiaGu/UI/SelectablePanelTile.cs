using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public class SelectablePanelTile : MonoBehaviour, IPointerDownHandler
    {
        SelectablePanelTile()
        {
        }

        [SerializeField]
        Color onFocusColor = ColorExtensions.New(240, 80, 80);
        [SerializeField]
        Color onBlurColor = ColorExtensions.New(240, 100, 100);

        SelectablePanel parent;
        Image imageObject;
        bool isShowWarning;

        void Awake()
        {
            parent = GetComponentInParent<SelectablePanel>();
            imageObject = GetComponent<Image>();
            parent.OnFocusEvent += OnFocus;
            parent.OnBlurEvent += OnBlur;
            ResetState();
        }

        void ResetState()
        {
            if (parent.IsFocus)
                OnFocus();
            else
                OnBlur();
        }

        void OnFocus()
        {
            imageObject.color = onFocusColor;
        }

        void OnBlur()
        {
            imageObject.color = onBlurColor;
        }

        /// <summary>
        /// 闪动显示标题栏;
        /// </summary>
        public void OnWarning()
        {
            if (!isShowWarning)
            {
                isShowWarning = true;
                StartCoroutine(_OnWarning());
            }
        }

        IEnumerator _OnWarning()
        {
            const float seconds = 0.2f;
            for (int i = 0; i < 2; i++)
            {
                imageObject.color = onFocusColor;
                yield return new WaitForSecondsRealtime(seconds);
                imageObject.color = onBlurColor;
                yield return new WaitForSecondsRealtime(seconds);
            }
            ResetState();
            isShowWarning = false;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            parent.OnFocus();
        }
    }
}
