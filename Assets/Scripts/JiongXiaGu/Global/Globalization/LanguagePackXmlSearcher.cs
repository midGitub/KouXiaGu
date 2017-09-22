using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using JiongXiaGu.Resources;

namespace JiongXiaGu.Globalization
{

    /// <summary>
    /// 语言包搜索;
    /// </summary>
    public class LanguagePackXmlSearcher
    {
        public LanguagePackXmlSearcher()
        {
            SearchDirectory = Path.Combine(Resource.DataDirectoryPath, "Localization");
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
            LanguagePackStream pack = null;
            FileStream fStream = null;

            foreach (var filePath in filePaths)
            {
                try
                {
                    fStream = new FileStream(filePath, FileMode.Open);
                    pack = SerializeHead(fStream);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex);
                }

                if (pack != null)
                {
                    fStream.Seek(0, SeekOrigin.Begin);
                    yield return pack;
                    pack = null;
                }
                else
                {
                    fStream.Dispose();
                }
                fStream = null;
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
