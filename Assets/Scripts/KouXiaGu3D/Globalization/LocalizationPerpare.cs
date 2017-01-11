using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 负责游戏一开始对本地化信息初始化;
    /// </summary>
    public class LocalizationPerpare : MonoBehaviour
    {

        void Start()
        {
            List<XmlLanguageFile> ReadOnlyLanguageFiles = Resources.FindLanguageFiles().ToList();
            Localization.Initialize(ReadOnlyLanguageFiles);
            LocalizationText.UpdateTextDictionary(Localization.LanguageFile.ReadTexts());
        }

    }

}
