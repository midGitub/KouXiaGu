using System;
using KouXiaGu.Localizations;
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
#if COLLECT_LACKED_KEYS
            LackingTextCollecter.Collecting(key);
            textObject.text = key;
#else
            Debug.Log(name + " 无法找到对应文本:" + key);
#endif
        }

    }

}
