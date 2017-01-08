using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using KouXiaGu.Localizations;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public sealed class LanguageUI : MonoBehaviour, IResponsive
    {
        LanguageUI() { }


        [SerializeField]
        Dropdown languageDropdown;


        void OnEnable()
        {
            languageDropdown.ClearOptions();

            if (Localization.Initialized)
            {
                languageDropdown.AddOptions(Localization.ReadOnlyLanguages);
                languageDropdown.value = Localization.LanguageIndex;
            }
        }

        void Start()
        {
            languageDropdown.onValueChanged.AddListener(OnChanged);
        }

        void OnChanged(int id)
        {
            Localization.SetConfig(id, false);
        }

        void IResponsive.OnApply()
        {
            return;
        }

        void IResponsive.OnReset()
        {
            languageDropdown.value = Localization.LanguageIndex;
        }

    }

}
