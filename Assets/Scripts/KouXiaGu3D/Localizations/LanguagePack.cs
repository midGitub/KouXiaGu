using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Localizations
{


    public class LanguagePack
    {

        static readonly Dictionary<string, SystemLanguage> languageDictionary = GetLanguageDictionary();

        static Dictionary<string, SystemLanguage> GetLanguageDictionary()
        {
            var languagesArray = Enum.GetValues(typeof(SystemLanguage));
            var languageDictionary = new Dictionary<string, SystemLanguage>(languagesArray.Length);

            foreach (SystemLanguage language in languagesArray)
            {
                string key = language.ToString();
                languageDictionary.AddOrUpdate(key, language);
            }

            return languageDictionary;
        }

        /// <summary>
        /// 返回枚举表示的语言,若不存在则返回 Unknown
        /// </summary>
        public static SystemLanguage GetLanguage(string language)
        {
            SystemLanguage systemLanguage;
            if (languageDictionary.TryGetValue(language, out systemLanguage))
            {
                return systemLanguage;
            }
            return SystemLanguage.Unknown;
        }


        public LanguagePack(string language, ITextReader file)
        {
            this.Language = GetLanguage(language);
            this.Reader = file;
        }

        public LanguagePack(SystemLanguage language, ITextReader file)
        {
            this.Language = language;
            this.Reader = file;
        }

        public SystemLanguage Language { get; private set; }

        public ITextReader Reader { get; private set; }



    }

}
