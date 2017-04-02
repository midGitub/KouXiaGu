using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using KouXiaGu.Globalization;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public sealed class LanguageSetting : MonoBehaviour
    {
        LanguageSetting() { }

        [SerializeField]
        Dropdown languageDropdown;

        void Start()
        {
            languageDropdown.ClearOptions();

            languageDropdown.AddOptions(Localization.Languages.Select(pack => pack.Name).ToList());
                languageDropdown.value = Localization.LanguageIndex;

            languageDropdown.onValueChanged.AddListener(OnChanged);
        }

        void OnChanged(int index)
        {
            if (Localization.LanguageIndex != index)
            {
                Localization.SetLanguage(index);
            }
        }

    }

}
