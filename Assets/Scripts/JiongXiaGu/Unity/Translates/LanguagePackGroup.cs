using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Translates
{

    /// <summary>
    /// 语言包合集;
    /// </summary>
    public class LanguagePackGroup
    {
        /// <summary>
        /// 语言类型;
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// 语言包合集;
        /// </summary>
        public List<LanguagePack> LanguagePacks { get; private set; }

        public LanguagePackGroup(LanguagePack main)
        {
            LanguagePacks = new List<LanguagePack>();
            LanguagePacks.Add(main);
        }

        /// <summary>
        /// 添加语言包;
        /// </summary>
        public void Add(LanguagePack languagePack)
        {
            if (languagePack.Language != Language)
                throw new ArgumentException("languagePack 语言不同于该组;");

            LanguagePacks.Add(languagePack);
        }

        /// <summary>
        /// 移除语言包;
        /// </summary>
        public bool Remove(LanguagePack languagePack)
        {
            return LanguagePacks.Remove(languagePack);
        }

        /// <summary>
        /// 获取到对应文本(从LanguagePackPacks最后一个元素向前查询),若未能获取到则返回 key;
        /// </summary>
        public string Translate(string key)
        {
            for (int i = LanguagePacks.Count - 1; i >= 0; i--)
            {
                var languagePack = LanguagePacks[i];
                string value;
                if (languagePack.LanguageDictionary.TryGetValue(key, out value))
                {
                    return value;
                }
            }
            return key;
        }
    }
}
