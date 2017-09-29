using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 一个相同语言的包组成的合集,包含一个主语言字典和多个补充语言字典;
    /// </summary>
    public class LanguagePackGroup : IReadOnlyCollection<LanguagePack>
    {
        /// <summary>
        /// 主要语言字典;
        /// </summary>
        public LanguagePack MainLanguagePack { get; private set; }

        /// <summary>
        /// 补充语言字典合集;
        /// </summary>
        public List<LanguagePack> SupplementLanguagePacks { get; private set; }

        /// <summary>
        /// 语言类型;
        /// </summary>
        public string Language
        {
            get { return MainLanguagePack.Language; }
        }

        /// <summary>
        /// 语言包总数;
        /// </summary>
        public int Count
        {
            get { return SupplementLanguagePacks.Count + 1; }
        }

        public LanguagePackGroup(LanguagePack mainLanguagePack)
        {
            if (mainLanguagePack == null)
                throw new ArgumentNullException("mainLanguagePack");

            MainLanguagePack = mainLanguagePack;
            SupplementLanguagePacks = new List<LanguagePack>();
        }

        /// <summary>
        /// 添加补充语言包;
        /// </summary>
        public void Add(LanguagePack languagePack)
        {
            if (languagePack == null)
                throw new ArgumentNullException("languagePack");
            if (languagePack.Language != Language)
                throw new ArgumentException("languagePack 语言不同于该合集;");

            SupplementLanguagePacks.Add(languagePack);
        }

        /// <summary>
        /// 移除语言包;
        /// </summary>
        public bool Remove(LanguagePack languagePack)
        {
            if (languagePack == null)
                throw new ArgumentNullException("languagePack");

            return SupplementLanguagePacks.Remove(languagePack);
        }

        /// <summary>
        /// 确认是否存在改语言包;
        /// </summary>
        public bool Contains(LanguagePack languagePack)
        {
            if (languagePack == null)
                throw new ArgumentNullException("languagePack");

            return SupplementLanguagePacks.Contains(languagePack);
        }

        /// <summary>
        /// 词条总数;
        /// </summary>
        public int TextItemCount()
        {
            return EnumerateLanguageDictionary().Sum(item => item.LanguageDictionary.Count);
        }

        /// <summary>
        /// 获取到对应文本,若未能获取到则返回 key;(从补充语言包合集最后一个元素向前查询,最后查询主语言字典)
        /// </summary>
        public string Translate(string key)
        {
            foreach (var languageDictionary in EnumerateLanguageDictionary())
            {
                string value;
                if (languageDictionary.LanguageDictionary.TryGetValue(key, out value))
                {
                    return value;
                }
            }
            return key;
        }

        /// <summary>
        /// 枚举所有语言字典(按字典查询优先级返回);
        /// </summary>
        public IEnumerable<LanguagePack> EnumerateLanguageDictionary()
        {
            for (int i = SupplementLanguagePacks.Count - 1; i >= 0; i--)
            {
                yield return SupplementLanguagePacks[i];
            }
            yield return MainLanguagePack;
        }

        public IEnumerator<LanguagePack> GetEnumerator()
        {
            return EnumerateLanguageDictionary().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
