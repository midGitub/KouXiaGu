using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JiongXiaGu.UI
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class ImageResponder : MonoBehaviour, IPointerDownHandler
    {
        ImageResponder()
        {
        }

        [SerializeField]
        UnityEvent onPointerDown;

        public event UnityAction OnPointerDown
        {
            add { onPointerDown.AddListener(value); }
            remove { onPointerDown.RemoveListener(value); }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            onPointerDown.Invoke();
        }
    }
}
