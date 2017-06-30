using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    [XmlType("LocalizationConfig")]
    public class LocalizationConfig
    {

        [XmlElement("LanguageName")]
        public string LanguageName { get; set; }

        [XmlElement("Language")]
        public string Language { get; set; }

        /// <summary>
        /// 获取到指定语言文件,若未能找到合适的语言包则返回Null;
        /// </summary>
        public T Find<T>(ICollection<T> packs)
            where T : LanguagePackHead
        {
            if (packs == null)
                throw new ArgumentNullException("packs");

            T pack;
            if (!string.IsNullOrEmpty(LanguageName))
            {
                pack = FindLanguageName(packs, LanguageName);
                if (pack != null)
                    return pack;
            }
            if (!string.IsNullOrEmpty(Language))
            {
                pack = FindLanguage(packs, Language);
                if (pack != null)
                    return pack;
            }

            pack = FindSystemLanguage(packs);
            return pack;
        }

        /// <summary>
        /// 根据语言包名字获取到语言包;
        /// </summary>
        T FindLanguageName<T>(IEnumerable<T> packs, string name)
            where T : LanguagePackHead
        {
            foreach (var pack in packs)
            {
                if (pack.Name == LanguageName)
                    return pack;
            }
            return null;
        }

        /// <summary>
        /// 根据语言获取到语言包;
        /// </summary>
        T FindLanguage<T>(IEnumerable<T> packs, string language)
             where T : LanguagePackHead
        {
            foreach (var pack in packs)
            {
                if (pack.Language == language)
                    return pack;
            }
            return null;
        }

        /// <summary>
        /// 根据系统语言获取到语言包;
        /// </summary>
        T FindSystemLanguage<T>(IEnumerable<T> packs)
            where T : LanguagePackHead
        {
            string language = Application.systemLanguage.ToString().ToLower();
            return FindLanguage(packs, language);
        }
    }

    class ConfigFilePath : SingleFilePath
    {
        public override string FileName
        {
            get { return "Localization/Config.xml"; }
        }
    }

    class ConfigReader : FileReaderWriter<LocalizationConfig>
    {
        static readonly IFileSerializer<LocalizationConfig> fileSerializer = new XmlFileSerializer<LocalizationConfig>();

        public ConfigReader() : base(new ConfigFilePath(), fileSerializer)
        {
        }

        public ConfigReader(ISingleFilePath file, IFileSerializer<LocalizationConfig> serializer) : base(file, serializer)
        {
        }
    }
}
