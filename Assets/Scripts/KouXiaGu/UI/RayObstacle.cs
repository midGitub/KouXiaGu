using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KouXiaGu.UI
{

    /// <summary>
    /// UI射线阻挡;
    /// </summary>
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public class RayObstacle : MonoBehaviour, IPointerDownHandler
    {
        RayObstacle()
        {
        }

        Action<PointerEventData> pointerDownEvent;

        public event Action<PointerEventData> PointerDownEvent
        {
            add { pointerDownEvent += value; }
            remove { pointerDownEvent -= value; }
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (pointerDownEvent != null)
            {
                pointerDownEvent(eventData);
            }
        }
    }
}
