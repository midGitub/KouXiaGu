using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Globalization
{


    public class Language : CultureInfo
    {

        static readonly Language currentLanguage = new Language("Default");

        public static Language CurrentLanguage
        {
            get { return currentLanguage; }
        }


        public Language(string name) : base(CurrentCulture.Name)
        {
            this.LanguageName = name;
        }

        /// <summary>
        /// 若Tag未知则返回ArgumentException
        /// </summary>
        public Language(string name, string languageTag) : base(languageTag)
        {
            this.LanguageName = name;
            this.LanguageTag = languageTag;
        }

        public string LanguageName { get; private set; }
        public string LanguageTag { get; private set; }

    }

}
