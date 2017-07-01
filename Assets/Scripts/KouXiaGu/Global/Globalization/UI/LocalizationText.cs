using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 挂载到需要本地化的文本组件上;
    /// </summary>
    [RequireComponent(typeof(Text))]
    [DisallowMultipleComponent]
    public class LocalizationText : MonoBehaviour, ILocalizationText
    {
        LocalizationText()
        {
        }

        Text textObject;
        public string Key { get; private set; }

        void Awake()
        {
            textObject = GetComponent<Text>();
            Key = textObject.text;
            Localization.Subscribe(this);
        }

        void ILocalizationText.OnLanguageUpdate(LanguagePack pack)
        {
            textObject.text = pack.GetText(Key);
        }
    }
}
