using System;
using System.Collections.Generic;
using KouXiaGu.OGlobalization;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent, RequireComponent(typeof(Text))]
    public sealed class LocalizationText : MonoBehaviour, ITextObserver
    {

        string key;
        Text textObject;
        IDisposable unSubscriber;

        string Key
        {
            get { return key; }
        }

        public string Text
        {
            get { return textObject.text; }
            private set { textObject.text = value; }
        }

        void Awake()
        {
            textObject = GetComponent<Text>();

            key = textObject.text;
            unSubscriber = Localization.Subscribe(this);
        }

        void OnDestroy()
        {
            unSubscriber.Dispose();
            unSubscriber = null;
        }

        public void UpdateTexts(IDictionary<string, string> textDictionary)
        {
            string text;
            if (textDictionary.TryGetValue(Key, out text))
            {
                Text = text;
            }
            else
            {
                OnTextNotFound();
            }
        }

        void OnTextNotFound()
        {
            Debug.Log(name + " 无法找到对应文本:" + key);
            Text = key;
        }

    }

}
