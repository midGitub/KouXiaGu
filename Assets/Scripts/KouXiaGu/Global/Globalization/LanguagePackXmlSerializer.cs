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
        public const string LanguageLocNameAttributeName = "locName";
        public const string TextsRootName = "Texts";
        public const string TextElementName = "Text";
        public const string TextKeyAttributeName = "key";
        public const string TextValueAttributeName = "value";
        readonly XmlSerializer serializer;

        /// <summary>
        /// 搜索目录下的语言包文件;
        /// </summary>
        public IEnumerable<LanguagePackStream> Search(string directory, string searchPattern, SearchOption searchOption)
        {
            var filePaths = Directory.GetFiles(directory, searchPattern, searchOption);
            LanguagePackStream pack;
            FileStream fStream;

            foreach (var filePath in filePaths)
            {
                try
                {
                    fStream = new FileStream(filePath, FileMode.Open);
                    pack = SerializeHead(fStream);
                }
                catch(Exception ex)
                {
                    pack = null;
                    fStream = null;
                    Debug.LogWarning(ex);
                }

                if (pack != null)
                {
                    yield return pack;
                }
                else
                {
                    fStream.Dispose();
                }
            }
        }

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

        /// <summary>
        /// 仅序列化头部信息;
        /// </summary>
        LanguagePackStream SerializeHead(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                reader.MoveToContent();
                if (reader.IsStartElement(RootName))
                {
                    string name = reader.GetAttribute(LanguageNameAttributeName);
                    string locName = reader.GetAttribute(LanguageLocNameAttributeName);

                    if (!string.IsNullOrEmpty(locName))
                    {
                        var head = new LanguagePackStream(name, locName, stream);
                        return head;
                    }
                }
            }
            return null;
        }
    }
}
