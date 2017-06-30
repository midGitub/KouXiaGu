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



        public static Localization Create()
        {
            LanguagePackXmlSearcher searcher = new LanguagePackXmlSearcher();
            var packs = searcher.Search();
            return Create(packs);
        }

        public static Localization Create(IEnumerable<LanguagePackStream> packs)
        {
            ConfigReader configReader = new ConfigReader();
            var config = configReader.Read();
            return Create(packs, config);
        }

        public static Localization Create(IEnumerable<LanguagePackStream> packs, LocalizationConfig config)
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
