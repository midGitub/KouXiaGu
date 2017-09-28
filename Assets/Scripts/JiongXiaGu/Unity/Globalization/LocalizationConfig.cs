using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Globalization
{

    [XmlType("LocalizationConfig")]
    public class LocalizationConfig
    {

        [XmlElement("LanguageName")]
        public string LanguageName { get; set; }

        [XmlElement("Language")]
        public string Language { get; set; }
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
        static readonly IOFileSerializer<LocalizationConfig> fileSerializer = new XmlFileSerializer<LocalizationConfig>();

        public ConfigReader() : base(new ConfigFilePath(), fileSerializer)
        {
        }

        public ConfigReader(ISingleFilePath file, IOFileSerializer<LocalizationConfig> serializer) : base(file, serializer)
        {
        }
    }
}
