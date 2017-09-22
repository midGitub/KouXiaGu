using JiongXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public class UIMapEditHandlerTitle : MonoBehaviour
    {
        [SerializeField]
        Text titleText;
        [SerializeField]
        Toggle activationToggle;
        UIMapEditHandlerView pater;
        UIMapEditHandler editHandler;
        ContentSizeFitterEx contentSizeFitter;

        /// <summary>
        /// 是否显示详细信息?
        /// </summary>
        public bool IsContentDisplay
        {
            get { return editHandler.gameObject.activeSelf; }
        }

        /// <summary>
        /// 是否激活?
        /// </summary>
        public bool IsActivate
        {
            get { return activationToggle.isOn; }
        }

        /// <summary>
        /// 操作接口;
        /// </summary>
        public UIMapEditHandler EditHandler
        {
            get { return editHandler; }
        }

        public void Initialize(UIMapEditHandlerView pater, UIMapEditHandler editHandler, ContentSizeFitterEx contentSizeFitter)
        {
            this.pater = pater;
            this.editHandler = editHandler;
            this.contentSizeFitter = contentSizeFitter;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
            editHandler.transform.SetParent(parent, false);
        }

        /// <summary>
        /// 获取到序列号;
        /// </summary>
        public int GetOrder()
        {
            int order = transform.GetSiblingIndex() / 2;
            return order;
        }

        /// <summary>
        /// 设置UI显示的顺序;
        /// </summary>
        /// <param name="order">0~max</param>
        public void SetOrder(int order)
        {
            order *= 2;
            if (transform.childCount <= order)
            {
                SetAsLastOrder();
            }
            else if (transform.GetSiblingIndex() >= order)
            {
                editHandler.transform.SetSiblingIndex(order);
                transform.SetSiblingIndex(order);
            }
            else
            {
                editHandler.transform.SetSiblingIndex(order + 1);
                transform.SetSiblingIndex(order);
            }
        }

        /// <summary>
        /// 设置为第一个顺序;
        /// </summary>
        [ContextMenu("SetAsFirstOrder")]
        public void SetAsFirstOrder()
        {
            editHandler.transform.SetAsFirstSibling();
            transform.SetAsFirstSibling();
        }

        /// <summary>
        /// 设置为最后的顺序;
        /// </summary>
        [ContextMenu("SetAsLastOrder")]
        public void SetAsLastOrder()
        {
            transform.SetAsLastSibling();
            editHandler.transform.SetAsLastSibling();
        }

        /// <summary>
        /// 显示详细操作;
        /// </summary>
        public void DisplayContent()
        {
            if (editHandler != null)
            {
                editHandler.gameObject.SetActive(true);
            }
            if (contentSizeFitter != null)
            {
                contentSizeFitter.RectTransformDimensionsChange();
            }
        }

        /// <summary>
        /// 隐藏详细操作;
        /// </summary>
        public void HideContent()
        {
            if (editHandler != null)
            {
                editHandler.gameObject.SetActive(false);
            }
            if (contentSizeFitter != null)
            {
                contentSizeFitter.RectTransformDimensionsChange();
            }
        }

        /// <summary>
        /// 设置消息;
        /// </summary>
        public void SetMessage(string massage)
        {
            titleText.text = massage;
        }

        /// <summary>
        /// 销毁所有;
        /// </summary>
        public void Destroy()
        {
            pater.Remove(this);
            Destroy(gameObject);
            Destroy(editHandler.gameObject);
        }
    }
}
