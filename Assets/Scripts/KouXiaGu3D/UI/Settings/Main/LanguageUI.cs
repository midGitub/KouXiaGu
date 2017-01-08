using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Localizations;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public sealed class LanguageUI : MonoBehaviour, IResponsive
    {
        LanguageUI() { }

        [SerializeField]
        Dropdown languageDropdown;

        void Start()
        {
            languageDropdown.onValueChanged.AddListener(OnChanged);
        }

        void OnChanged(int id)
        {
            if (Localization.LanguageIndex != id)
                Localization.SetConfig(id, false);
        }

        public void OnApply()
        {
            return;
        }

        public void OnReset()
        {
            languageDropdown.ClearOptions();

            if (Localization.Initialized)
            {
                languageDropdown.AddOptions(Localization.ReadOnlyLanguages);
                languageDropdown.value = Localization.LanguageIndex;
            }
        }

    }

}
