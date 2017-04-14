using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Globalization
{

    [XmlType("LocalizationConfig")]
    public class LocalizationConfig
    {
        public static LocalizationConfig Current
        {
            get
            {
                return new LocalizationConfig()
                {
                    LocName = Localization.CurrentLanguage != null ? Localization.CurrentLanguage.LocName : "unknown",
                };
            }
        }

        public LocalizationConfig()
        {
        }

        [XmlElement("LocName")]
        public string LocName;
    }


    public class XmlLocalizationConfigReader
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LocalizationConfig));

        public LocalizationConfig Read(string filePath)
        {
            return (LocalizationConfig)serializer.DeserializeXiaGu(filePath);
        }

        public void Write(LocalizationConfig item, string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);

            if (Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            serializer.SerializeXiaGu(filePath, item);
        }
    }

}
