using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.xgLocalization
{


    public class Language
    {

        static readonly Language currentLanguage = new Language("Default");

        public static Language CurrentLanguage
        {
            get { return currentLanguage; }
        }


        public Language(string name)
        {
            this.Name = name;
            CultureInfo = CultureInfo.CurrentCulture;
        }

        public Language(string name, string languageTag)
        {
            this.Name = name;
            this.LanguageTag = languageTag;

            try
            {
                CultureInfo = new CultureInfo(languageTag);
            }
            catch(ArgumentException)
            {
                CultureInfo = CultureInfo.CurrentCulture;
                Debug.LogWarning("未知的区域性名称:" + languageTag + ",来自语言文件:" + name);
            }
        }

        public string Name { get; private set; }
        public string LanguageTag { get; private set; }

        public CultureInfo CultureInfo { get; private set; }

        public static implicit operator CultureInfo(Language item)
        {
            return item.CultureInfo;
        }

    }

}
