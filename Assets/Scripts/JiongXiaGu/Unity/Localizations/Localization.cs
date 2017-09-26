using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 本地化;
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// 文本字典结构;
        /// </summary>
        public static LanguageDictionary LanguageDictionary { get; internal set; }

        /// <summary>
        /// 观察者合集;
        /// </summary>
        static readonly List<ILanguageHandle> languageHandler;


    }
}
