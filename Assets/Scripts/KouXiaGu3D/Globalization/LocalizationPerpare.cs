using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Initialization;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 负责游戏一开始对本地化信息初始化;
    /// </summary>
    public class LocalizationPerpare : Preparation
    {

        protected override void Start()
        {
            base.Start();
            List<LanguageFile> ReadOnlyLanguageFiles = Resources.FindLanguageFiles().ToList();
            Localization.Initialize(ReadOnlyLanguageFiles);
            LocalizationText.UpdateTextDictionary(Localization.LanguageFile.ReadTexts());
            OnComplete();
        }

    }

}
