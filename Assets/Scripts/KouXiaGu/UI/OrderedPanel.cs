using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 可排序的面板;
    /// </summary>
    [DisallowMultipleComponent]
    public class OrderedPanel : MonoBehaviour, IPointerClickHandler
    {
        OrderedPanel()
        {
        }

        /// <summary>
        /// 需要移动的目标;
        /// </summary>
        RectTransform panel;

        void Awake()
        {
            panel = GetComponent<RectTransform>();
        }

        //void Update()
        //{
        //    if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Debug.Log("哒哒哒!");
        //    }
        //}

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            panel.SetAsLastSibling();
        }
    }
}
