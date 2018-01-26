//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//namespace JiongXiaGu.Unity.Localizations
//{

//    /// <summary>
//    /// 一个相同语言的包组成的合集,包含一个主语言字典和多个补充语言字典;
//    /// </summary>
//    public class LanguageGroup : IReadOnlyCollection<LanguagePack>, IReadOnlyPack
//    {
//        /// <summary>
//        /// 主要语言字典;
//        /// </summary>
//        public LanguagePack MainPack { get; private set; }

//        /// <summary>
//        /// 补充语言字典合集;
//        /// </summary>
//        private List<LanguagePack> supplementPacks;

//        /// <summary>
//        /// 语言类型;
//        /// </summary>
//        public string Language
//        {
//            get { return MainPack.Description.Language; }
//        }

//        /// <summary>
//        /// 语言包总数;
//        /// </summary>
//        public int Count
//        {
//            get { return supplementPacks.Count + 1; }
//        }

//        /// <summary>
//        /// 补充语言字典合集;
//        /// </summary>
//        public IReadOnlyList<LanguagePack> SupplementPacks
//        {
//            get { return supplementPacks; }
//        }

//        public LanguageGroup(LanguagePack mainPack)
//        {
//            if (mainPack == null)
//                throw new ArgumentNullException(nameof(mainPack));

//            this.MainPack = mainPack;
//            supplementPacks = new List<LanguagePack>();
//        }

//        public LanguageGroup(LanguagePack mainPack, IEnumerable<LanguagePack> supplementPacks)
//        {
//            if (mainPack == null)
//                throw new ArgumentNullException(nameof(mainPack));
//            if (supplementPacks == null)
//                throw new ArgumentNullException(nameof(supplementPacks));

//            this.MainPack = mainPack;
//            this.supplementPacks = new List<LanguagePack>(supplementPacks);
//        }

//        /// <summary>
//        /// 添加新的补充语言包,若已经存在则置为返回异常;
//        /// </summary>
//        public void Add(LanguagePack pack)
//        {
//            if (pack == null)
//                throw new ArgumentNullException(nameof(pack));
//            if (pack.Description.Language != Language)
//                throw new ArgumentException("传入语言不同于该合集;");
//            if (MainPack == pack)
//                throw new ArgumentException(string.Format("传入语言包和主语言包相同[{0}]", pack));
//            if (supplementPacks.Contains(pack))
//                throw new ArgumentException(string.Format("已经存在语言包[{0}]", pack));

//            supplementPacks.Add(pack);
//        }

//        /// <summary>
//        /// 移除补充语言包;
//        /// </summary>
//        public bool Remove(LanguagePack pack)
//        {
//            if (pack == null)
//                throw new ArgumentNullException(nameof(pack));

//            return supplementPacks.Remove(pack);
//        }

//        /// <summary>
//        /// 确认是否存在该语言包(包括检查主语言包);
//        /// </summary>
//        public bool Contains(LanguagePack languagePack)
//        {
//            if (languagePack == null)
//                throw new ArgumentNullException(nameof(languagePack));
//            if (languagePack == MainPack)
//                return true;

//            return supplementPacks.Contains(languagePack);
//        }

//        /// <summary>
//        /// 清除所有补充语言包;
//        /// </summary>
//        public void Clear()
//        {
//            supplementPacks.Clear();
//        }

//        /// <summary>
//        /// 词条总数;
//        /// </summary>
//        public int TextItemCount()
//        {
//            return EnumerateLanguageDictionary().Sum(item => item.LanguageDictionary.Count);
//        }

//        /// <summary>
//        /// 获取到对应文本,若未能获取到则返回false;(从补充语言包合集最后一个元素向前查询,最后查询主语言字典)
//        /// </summary>
//        public bool TryTranslate(string key, out string value)
//        {
//            foreach (var languageDictionary in EnumerateLanguageDictionary())
//            {
//                if (languageDictionary.LanguageDictionary.TryGetValue(key, out value))
//                {
//                    return true;
//                }
//            }
//            value = string.Empty;
//            return false;
//        }

//        /// <summary>
//        /// 枚举所有语言字典(按字典查询优先级返回);
//        /// </summary>
//        private IEnumerable<LanguagePack> EnumerateLanguageDictionary()
//        {
//            for (int i = SupplementPacks.Count - 1; i >= 0; i--)
//            {
//                yield return supplementPacks[i];
//            }
//            yield return MainPack;
//        }

//        /// <summary>
//        /// 枚举所有语言包(按字典查询优先级返回);
//        /// </summary>
//        public IEnumerator<LanguagePack> GetEnumerator()
//        {
//            return EnumerateLanguageDictionary().GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//    }
//}
