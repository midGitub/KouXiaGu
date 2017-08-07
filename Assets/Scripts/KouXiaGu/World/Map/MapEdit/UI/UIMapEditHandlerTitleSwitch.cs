using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KouXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class UIMapEditHandlerTitleSwitch : MonoBehaviour, IPointerDownHandler
    {
        UIMapEditHandlerTitleSwitch()
        {
        }

        [SerializeField]
        UIMapEditHandlerTitle titleScript;
        [SerializeField]
        Sprite displaySprite;
        [SerializeField]
        Sprite hideSprite;
        [SerializeField]
        bool isDisplay;
        Image imageObject;

        Image ImageObject
        {
            get { return imageObject ?? (imageObject = GetComponent<Image>()); }
        }

        public bool IsDisplay
        {
            get { return isDisplay; }
        }

        void OnValidate()
        {
            ApplayDisplay(isDisplay);
        }

        public void SwitchDisplay()
        {
            if (isDisplay)
            {
                ApplayHide();
            }
            else
            {
                ApplayDisplay();
            }
        }

        public void ApplayDisplay(bool isDisplay)
        {
            if (isDisplay)
            {
                ApplayDisplay();
            }
            else
            {
                ApplayHide();
            }
        }

        public void ApplayDisplay()
        {
            isDisplay = true;
            ImageObject.sprite = displaySprite;
            titleScript.DisplayContent();
        }

        public void ApplayHide()
        {
            isDisplay = false;
            ImageObject.sprite = hideSprite;
            titleScript.HideContent();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            SwitchDisplay();
        }
    }
}
