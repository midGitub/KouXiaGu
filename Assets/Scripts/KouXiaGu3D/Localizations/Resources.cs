using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Localizations
{


    public class Resources
    {

        /// <summary>
        /// 语言包存放的文件夹;
        /// </summary>
        const string RES_DIRECTORY = "Localization";

        const string DESCRIPTION_NAME = "Description.xml";

        public static string ResPath
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, RES_DIRECTORY); }
        }

        public static string ConfigFilePath
        {
            get { return Path.Combine(ResPath, DESCRIPTION_NAME); }
        }

        public static LocalizationConfig ReadConfig()
        {
            var descr = (LocalizationConfig)LocalizationConfig.Serializer.DeserializeXiaGu(ConfigFilePath);
            return descr;
        }

        public static void WriteConfig(LocalizationConfig config)
        {
            LocalizationConfig.Serializer.SerializeXiaGu(ConfigFilePath, config);
        }


        public static List<ITextReader> GetTextReader(string language, string secondLanguage)
        {
            List<ITextReader> readers = new List<ITextReader>(1);
            IEnumerable<LanguagePack> packs = LanguagePack.Find(ResPath);

            LanguagePack first = null;
            LanguagePack second = null;

            foreach (var pack in packs)
            {
                if (pack.Language == language)
                    first = pack;
                else if (pack.Language == secondLanguage)
                    second = pack;
            }

            if (first != null)
                readers.Add(first);
            else if (second != null)
                readers.Add(second);
            else
                Debug.LogError("未找到合适的语言包;");

            return readers;
        }

        public static IEnumerable<LanguagePack> LanguagePacks()
        {
            return LanguagePack.Find(ResPath);
        }

    }

}
