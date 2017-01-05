using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Localizations
{

    [DisallowMultipleComponent, RequireComponent(typeof(Text))]
    public class LocalizationText : MonoBehaviour, ITextObserver
    {

        Text textObject;

        void Awake()
        {
            textObject = GetComponent<Text>();
            Localization.ObservableText.Subscribe(this);
        }

        string ITextObserver.Key
        {
            get { return textObject.text; }
        }

        void ITextObserver.SetText(string text)
        {
            textObject.text = text;
        }

        void ITextObserver.OnTextNotFound()
        {
            Debug.Log("无法找到对应文本:" + (this as ITextObserver).Key);
        }
    }

}
