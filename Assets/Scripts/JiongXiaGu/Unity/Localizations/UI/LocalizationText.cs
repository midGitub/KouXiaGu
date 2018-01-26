using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.Localizations.UI
{

    [RequireComponent(typeof(Text))]
    public sealed class LocalizationText : MonoBehaviour, IObserver<LanguageChangedEvent>
    {
        private LocalizationText()
        {
        }

        private Text textObject;
        public string Key { get; private set; }

        private void Awake()
        {
            textObject = GetComponent<Text>();
            Key = textObject.text;
        }

        private void OnEnable()
        {
            Localization.Subscribe(this);
            Translate();
        }

        private void OnDisable()
        {
            Localization.Unsubscribe(this);
        }

        void IObserver<LanguageChangedEvent>.OnNext(LanguageChangedEvent value)
        {
            Translate(value.LanguagePack.LanguageDictionary);
        }

        void IObserver<LanguageChangedEvent>.OnCompleted()
        {
            return;
        }

        void IObserver<LanguageChangedEvent>.OnError(Exception error)
        {
            return;
        }

        public void Translate()
        {
            if (Localization.LanguageDictionary != null)
            {
                Translate(Localization.LanguageDictionary);
            }
        }

        public void Translate(IReadOnlyLanguageDictionary languageDictionary)
        {
            string text;
            if (languageDictionary.TryGetValue(Key, out text))
            {
                textObject.text = text;
            }
        }
    }
}
