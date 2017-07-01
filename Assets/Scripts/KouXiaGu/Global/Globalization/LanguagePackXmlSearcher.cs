using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using KouXiaGu.Resources;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 语言包搜索;
    /// </summary>
    public class LanguagePackXmlSearcher
    {
        public LanguagePackXmlSearcher()
        {
            SearchDirectory = Path.Combine(Resource.ConfigDirectoryPath, "Localization");
            SearchPattern = "language_*.xml";
            SearchOption = SearchOption.AllDirectories;
        }

        public LanguagePackXmlSearcher(string searchDirectory, string searchPattern, SearchOption searchOption)
        {
            SearchDirectory = searchDirectory;
            SearchPattern = searchPattern;
            SearchOption = searchOption;
        }

        public string SearchDirectory { get; private set; }
        public string SearchPattern { get; private set; }
        public SearchOption SearchOption { get; private set; }

        /// <summary>
        /// 搜索目录下的语言包文件;
        /// </summary>
        public IEnumerable<LanguagePackStream> EnumeratePacks()
        {
            return EnumeratePacks(SearchDirectory, SearchPattern, SearchOption);
        }

        /// <summary>
        /// 搜索目录下的语言包文件;
        /// </summary>
        public IEnumerable<LanguagePackStream> EnumeratePacks(string searchDirectory, string searchPattern, SearchOption searchOption)
        {
            var filePaths = Directory.GetFiles(searchDirectory, searchPattern, searchOption);
            LanguagePackStream pack;
            FileStream fStream;

            foreach (var filePath in filePaths)
            {
                try
                {
                    fStream = new FileStream(filePath, FileMode.Open);
                    pack = SerializeHead(fStream);
                }
                catch (Exception ex)
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
        /// 仅序列化头部信息;
        /// </summary>
        LanguagePackStream SerializeHead(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                reader.MoveToContent();
                if (reader.IsStartElement(LanguagePackXmlSerializer.RootName))
                {
                    string name = reader.GetAttribute(LanguagePackXmlSerializer.LanguageNameAttributeName);
                    string locName = reader.GetAttribute(LanguagePackXmlSerializer.LanguageAttributeName);

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
