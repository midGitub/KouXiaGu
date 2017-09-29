using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 对 UnityEngine.UI.Text 文本进行本地化处理;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public sealed class LocalizedText : MonoBehaviour, ILanguageObserver
    {
        LocalizedText()
        {
        }

        IDisposable unsubscriber;
        public Text TextObject { get; private set; }
        public string Key { get; private set; }

        void Awake()
        {
            TextObject = GetComponent<Text>();
            Key = TextObject.text;
            unsubscriber = Localization.Subscribe(this);
        }

        void OnDestroy()
        {
            unsubscriber.Dispose();
            unsubscriber = null;
        }

        void ILanguageObserver.OnLanguageChanged()
        {
            TextObject.text = Localization.Translate(Key);
        }
    }
}
