using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 本地化;
    /// </summary>
    public static class Localization
    {
        public static LanguagePack Pack { get; private set; }
        readonly static HashSet<ILocalizationText> observers = new HashSet<ILocalizationText>();

        public static string Language
        {
            get { return Pack != null ? Pack.Language : string.Empty; }
        }

        public static string LanguagePackName
        {
            get { return Pack != null ? Pack.Name : string.Empty; }
        }

        internal static IDictionary<string, string> TextDictionary
        {
            get { return Pack != null ? Pack.TextDictionary : null; }
        }

        /// <summary>
        /// 设置指定的语言,需要在Unity线程调用;
        /// </summary>
        public static void SetLanguage(LanguagePack pack)
        {
            if (pack == null)
                throw new ArgumentNullException("pack");

            Pack = pack;
            ObserverTracker();
        }

        /// <summary>
        /// 将语言包推送到所有订阅者;
        /// </summary>
        static void ObserverTracker()
        {
            foreach (var observer in observers)
            {
                observer.OnLanguageUpdate(Pack);
            }
        }

        /// <summary>
        /// 订阅到语言;
        /// </summary>
        public static IDisposable Subscribe(ILocalizationText observer)
        {
            if (!observers.Add(observer))
            {
                throw new ArgumentException("已经订阅到;");
            }
            return new CollectionUnsubscriber<ILocalizationText>(observers, observer);
        }

        /// <summary>
        /// 获取到对应文本,若不存在则返回key;
        /// </summary>
        public static string GetText(string key)
        {
            if (Pack != null)
            {
                return Pack.GetText(key);
            }
            return key;
        }

        /// <summary>
        /// 尝试获取到对应文本;
        /// </summary>
        public static bool TryGetText(string key, out string value)
        {
            if (Pack != null)
            {
                return Pack.TextDictionary.TryGetValue(key, out value);
            }
            value = string.Empty;
            return false;
        }

        /// <summary>
        /// 初始化;
        /// </summary>
        public static void Initialize()
        {
            var pack = Create();
            SetLanguage(pack);
        }

        public static LanguagePack Create()
        {
            LanguagePackXmlSearcher searcher = new LanguagePackXmlSearcher();
            var packs = searcher.EnumeratePacks().ToList();
            var item = Create(packs);
            LanguagePackStream.CloseAll(packs);
            return item;
        }

        public static LanguagePack Create(ICollection<LanguagePackStream> packs)
        {
            ConfigReader configReader = new ConfigReader();
            var config = configReader.Read();
            return Create(packs, config);
        }

        public static LanguagePack Create(ICollection<LanguagePackStream> packs, LocalizationConfig config)
        {
            LanguagePackXmlSerializer serializer = new LanguagePackXmlSerializer();
            LanguagePackStream stream = config.Find(packs);
            if (stream == null)
            {
                stream = packs.First();
            }
            return serializer.Deserialize(stream);
        }
    }
}
