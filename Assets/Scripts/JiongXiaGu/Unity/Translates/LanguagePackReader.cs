﻿using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Translates
{

    /// <summary>
    /// 语言包读写器;
    /// </summary>
    public class LanguagePackReader
    {
        public LanguagePackReader()
        {
            languagePackSerializer = new XmlSerializer(typeof(LanguagePack));
        }

        XmlSerializer languagePackSerializer;
        internal const string NameXmlAttributeName = "name";
        internal const string LanguageXmlAttributeName = "language";
        const string languagePackPrefix = "Language_";
        const string languagePackExtension = ".xml";

        /// <summary>
        /// 语言包搜索模式;
        /// </summary>
        static string LanguagePackFileSearchPattern
        {
            get { return languagePackPrefix + "*" + languagePackExtension; }
        }

        /// <summary>
        /// 读取语言文件;
        /// </summary>
        public LanguagePack Read(LanguagePackFileInfo info)
        {
            using (var stream = info.FileInfo.OpenRead())
            {
                return (LanguagePack)languagePackSerializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 输出语言文件;
        /// </summary>
        public LanguagePackFileInfo Write(string directory, LanguagePack languagePack)
        {
            string packFileName = GetLanguagePackFileName(languagePack);
            string packPath = Path.Combine(directory, packFileName);
            var fileInfo = new FileInfo(packPath);
            using (var stream = fileInfo.Create())
            {
                languagePackSerializer.SerializeXiaGu(languagePack, stream);
            }
            return new LanguagePackFileInfo(fileInfo, languagePack.Name, languagePack.Language);
        }

        /// <summary>
        /// 获取到语言包对应的文件名;
        /// </summary>
        public string GetLanguagePackFileName(LanguagePack languagePack)
        {
            return languagePackPrefix + languagePack.Language + languagePackExtension;
        }


        /// <summary>
        /// 迭代获取到所有语言包信息;
        /// </summary>
        /// <param name="directory">语言包存放路径</param>
        /// <param name="searchOption">指定搜索方式</param>
        public static IEnumerable<LanguagePackFileInfo> EnumerateInfos(string directory, SearchOption searchOption)
        {
            foreach (var path in Directory.EnumerateFiles(directory, LanguagePackFileSearchPattern, searchOption))
            {
                FileInfo fileInfo = new FileInfo(path);
                LanguagePackFileInfo languagePack;
                if (TryReadInfo(fileInfo, out languagePack))
                {
                    yield return languagePack;
                }
            }
        }

        /// <summary>
        /// 读取到语言包信息,若不为语言文件则返回异常;
        /// </summary>
        public static LanguagePackFileInfo ReadInfo(FileInfo fileInfo)
        {
            using (Stream stream = fileInfo.OpenRead())
            {
                XmlReader xmlReader = XmlReader.Create(stream);
                xmlReader.MoveToContent();

                string name = xmlReader.GetAttribute(NameXmlAttributeName);
                string language = xmlReader.GetAttribute(LanguageXmlAttributeName);
                if (string.IsNullOrEmpty(language))
                    throw new XmlException("未指定属性language;");

                return new LanguagePackFileInfo(fileInfo, name, language);
            }
        }

        /// <summary>
        /// 尝试读取到语言包信息;
        /// </summary>
        public static bool TryReadInfo(FileInfo fileInfo, out LanguagePackFileInfo languagePack)
        {
            try
            {
                languagePack = ReadInfo(fileInfo);
                return true;
            }
            catch (XmlException)
            {
                languagePack = default(LanguagePackFileInfo);
                return false;
            }
        }
    }
}
