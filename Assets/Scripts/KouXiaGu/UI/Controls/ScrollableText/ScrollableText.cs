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

    /// <summary>
    /// 滚动文本;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class ScrollableText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        ScrollableText()
        {
        }

        /// <summary>
        /// 更新间隔;
        /// </summary>
        const float updateSeconds = 0.1f;

        [SerializeField]
        Text textObject;
        [SerializeField, Range(0.1f, 5)]
        float speed = 0.5f;
        [SerializeField, Range(0, 5)]
        float waitSecondsOfStart = 1;
        [SerializeField, Range(0, 5)]
        float waitSecondsOfEnd = 1;
        UnityEngine.Coroutine scrollCoroutineHandle;

        public Text TextObject
        {
            get { return textObject; }
            internal set { textObject = value; }
        }

        /// <summary>
        /// 文本前进速度;
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// 在从头开始时等待的时间;
        /// </summary>
        public float WaitSecondsOfStart
        {
            get { return waitSecondsOfStart; }
            set { waitSecondsOfStart = value; }
        }

        /// <summary>
        /// 到达末尾之后等待的时间;
        /// </summary>
        public float WaitSecondsOfEnd
        {
            get { return waitSecondsOfEnd; }
            set { waitSecondsOfEnd = value; }
        }

        RectTransform rectTransform
        {
            get { return (RectTransform)transform; }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (IsNeedScroll() && scrollCoroutineHandle == null)
            {
                scrollCoroutineHandle = StartCoroutine(ScrollCoroutine());
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (scrollCoroutineHandle != null)
            {
                StopCoroutine(scrollCoroutineHandle);
                scrollCoroutineHandle = null;
                ResetScroll();
            }
        }

        /// <summary>
        /// 是否需要滚动才能完全显示?
        /// </summary>
        /// <returns></returns>
        public bool IsNeedScroll()
        {
            return rectTransform.rect.width < textObject.rectTransform.rect.width;
        }

        void ResetScroll()
        {
            Vector3 pos = textObject.rectTransform.localPosition;
            pos.x = 0;
            textObject.rectTransform.localPosition = pos;
        }

        IEnumerator ScrollCoroutine()
        {
            float to = rectTransform.rect.width - textObject.rectTransform.rect.width;
            while (true)
            {
                Vector3 pos = textObject.rectTransform.localPosition;
                if (pos.x == 0)
                {
                    yield return new WaitForSecondsRealtime(waitSecondsOfStart);
                }
                else if (pos.x == to)
                {
                    yield return new WaitForSecondsRealtime(waitSecondsOfEnd);
                    pos.x = 0;
                    textObject.rectTransform.localPosition = pos;
                    continue;
                }
                pos.x = Mathf.MoveTowards(pos.x, to, speed);
                textObject.rectTransform.localPosition = pos;
                yield return new WaitForSecondsRealtime(updateSeconds);
            }
        }
    }
}
