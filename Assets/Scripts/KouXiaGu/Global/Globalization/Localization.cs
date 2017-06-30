using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Globalization
{


    public class Localization
    {
        public Localization(LanguagePack pack)
        {
            Pack = pack;
        }

        public static Localization Default { get; private set; }
        public LanguagePack Pack { get; private set; }

        public string Language
        {
            get { return Pack.Language; }
        }

        public string LanguagePackName
        {
            get { return Pack.Name; }
        }

        public IDictionary<string, string> TextDictionary
        {
            get { return Pack.TextDictionary; }
        }

        /// <summary>
        /// 初始化默认;
        /// </summary>
        public static void Initialize()
        {
            Default = Create();
        }

        public static Localization Create()
        {
            LanguagePackXmlSearcher searcher = new LanguagePackXmlSearcher();
            var packs = searcher.EnumeratePacks().ToList();
            var item = Create(packs);
            LanguagePackStream.CloseAll(packs);
            return item;
        }

        public static Localization Create(ICollection<LanguagePackStream> packs)
        {
            ConfigReader configReader = new ConfigReader();
            var config = configReader.Read();
            return Create(packs, config);
        }

        public static Localization Create(ICollection<LanguagePackStream> packs, LocalizationConfig config)
        {
            LanguagePackXmlSerializer serializer = new LanguagePackXmlSerializer();
            LanguagePackStream stream = config.Find(packs);
            if (stream == null)
            {
                stream = packs.First();
            }
            var pack = serializer.Deserialize(stream);
            return new Localization(pack);
        }
    }
}
