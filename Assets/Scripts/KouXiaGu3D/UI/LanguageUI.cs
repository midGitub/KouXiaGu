using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Localizations;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public sealed class LanguageUI : MonoBehaviour, IResponsive
    {

        [SerializeField]
        Dropdown languageDropdown;

        void Awake()
        {
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(new List<string>(GetLanguageFiles()));
        }

        public IEnumerable<string> GetLanguageFiles()
        {
            return Localizations.Resources.GetLanguageFiles().Select(item => item.Language);
        }

        void IResponsive.OnApply()
        {
            throw new NotImplementedException();
        }

        void IResponsive.OnReset()
        {
            throw new NotImplementedException();
        }

    }

}
