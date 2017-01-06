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

        string key;

        void Awake()
        {
            textObject = GetComponent<Text>();
            key = textObject.text;

            Localization.Subscribe(this);
        }

        string ITextObserver.Key
        {
            get { return key; }
        }

        void ITextObserver.SetText(string text)
        {
            textObject.text = text;
        }

        void ITextObserver.OnTextNotFound()
        {
            Debug.Log( name +" 无法找到对应文本:" + key);
        }
    }

}
