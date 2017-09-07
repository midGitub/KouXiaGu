using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.Resources;

namespace KouXiaGu.OGlobalization
{

    [XmlType("LocalizationConfig")]
    public class LocalizationConfig
    {
        [XmlElement("LocName")]
        public string LocName;
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
