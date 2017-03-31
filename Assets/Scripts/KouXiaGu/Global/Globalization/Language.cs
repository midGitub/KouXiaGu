using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Globalization
{


    public class Culture : CultureInfo
    {

        static readonly Culture currentLanguage = new Culture("Default");

        public static Culture CurrentLanguage
        {
            get { return currentLanguage; }
        }


        public Culture(string name) : base(CurrentCulture.Name)
        {
            this.LanguageName = name;
        }

        /// <summary>
        /// 若Tag未知则返回ArgumentException
        /// </summary>
        public Culture(string name, string languageTag) : base(languageTag)
        {
            this.LanguageName = name;
            this.LanguageTag = languageTag;
        }

        public string LanguageName { get; private set; }
        public string LanguageTag { get; private set; }

    }

}
