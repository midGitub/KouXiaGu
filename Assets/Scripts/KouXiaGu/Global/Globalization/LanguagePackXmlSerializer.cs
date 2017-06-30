using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    public class LanguagePackXmlSerializer
    {
        public LanguagePackXmlSerializer()
        {
            serializer = new XmlSerializer(typeof(LanguagePack));
        }

        public const string RootName = "Language";
        public const string LanguageNameAttributeName = "name";
        public const string LanguageAttributeName = "language";
        public const string TextsRootName = "Texts";
        public const string TextElementName = "Text";
        public const string TextKeyAttributeName = "key";
        public const string TextValueAttributeName = "value";
        readonly XmlSerializer serializer;

        /// <summary>
        /// 序列化;
        /// </summary>
        public void Serialize(LanguagePack pack, Stream stream)
        {
            serializer.SerializeXiaGu(pack, stream);
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        public LanguagePack Deserialize(LanguagePackStream pack)
        {
            return Deserialize(pack.Stream);
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        public LanguagePack Deserialize(Stream stream)
        {
            return (LanguagePack)serializer.Deserialize(stream);
        }
    }
}
