using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.XmlLocalization;

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

            if (Localization.Initialized)
            {
                languageDropdown.AddOptions(Localization.ReadOnlyLanguages);
                languageDropdown.value = Localization.LanguageIndex;
            }

            languageDropdown.onValueChanged.AddListener(OnChanged);
        }

        void OnChanged(int id)
        {
            if (Localization.LanguageIndex != id)
                Localization.SetConfig(id);
        }

    }

}
