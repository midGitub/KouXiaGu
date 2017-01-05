using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace KouXiaGu.Localizations
{


    public class XmlFile : ITextReader, ITextWriter
    {

        /// <summary>
        /// 文件后缀;
        /// </summary>
        public const string FILE_EXTENSION = ".xml";

        const string ROOT_ELEMENT_NAME = "LocalizationTexts";
        const string LANGUAGE_ATTRIBUTE_NAME = "Language";

        const string TEXT_ELEMENT_NAME = "Text";
        const string KEY_ATTRIBUTE_NAME = "key";
        const string VALUE_ATTRIBUTE_NAME = "value";
        const string UPDATE_MARK_ATTRIBUTE_NAME = "update";

        const bool DEFAULT_UPDATE_MARK = false;


        protected static readonly XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            NewLineChars = Environment.NewLine,
            NewLineOnAttributes = false,
            Encoding = Encoding.UTF8,
        };

        protected static readonly XmlReaderSettings xmlReaderSettings = new XmlReaderSettings()
        {
            IgnoreWhitespace = true,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
        };


        /// <summary>
        /// 读取所有文本节点的内容并且返回;
        /// </summary>
        protected static IEnumerable<TextPack> ReadTexts(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement(TEXT_ELEMENT_NAME))
                {
                    string key = null;
                    string value = null;
                    bool updateMark = DEFAULT_UPDATE_MARK;

                    while (reader.MoveToNextAttribute())
                    {
                        switch (reader.Name)
                        {
                            case KEY_ATTRIBUTE_NAME:
                                key = reader.Value;
                                break;
                            case VALUE_ATTRIBUTE_NAME:
                                value = reader.Value;
                                break;
                            case UPDATE_MARK_ATTRIBUTE_NAME:
                                updateMark = reader.ReadContentAsBoolean();
                                break;
                        }
                    }

                    if (key != null && value != null)
                        yield return new TextPack(key, value, updateMark);
                }
            }
        }

        /// <summary>
        /// 保存所有文字结构到XML;
        /// </summary>
        protected static void WriteTexts(XmlWriter writer, string language, IEnumerable<TextPack> texts)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(ROOT_ELEMENT_NAME);
            writer.WriteStartAttribute(LANGUAGE_ATTRIBUTE_NAME);
            writer.WriteString(language);

            foreach (var pair in texts)
            {
                string key = pair.Key;
                string value = pair.Value;
                bool updateMark = pair.IsUpdate;

                writer.WriteStartElement(TEXT_ELEMENT_NAME);

                writer.WriteStartAttribute(KEY_ATTRIBUTE_NAME);
                writer.WriteString(key);

                writer.WriteStartAttribute(VALUE_ATTRIBUTE_NAME);
                writer.WriteString(value);

                if (updateMark != DEFAULT_UPDATE_MARK)
                {
                    writer.WriteStartAttribute(UPDATE_MARK_ATTRIBUTE_NAME);
                    writer.WriteValue(updateMark);
                }

                writer.WriteEndElement();
            }

            writer.WriteEndDocument();
        }


        /// <summary>
        /// 语言文件匹配的搜索字符串;
        /// </summary>
        const string LANGUAGE_PACK_SEARCH_PATTERN = "*" + FILE_EXTENSION;

        /// <summary>
        /// 寻找目录下存在的语言和其路径;
        /// </summary>
        public static IEnumerable<LanguagePack> LanguagePackExists(string directoryPath, SearchOption searchOption)
        {
            var paths = Directory.GetFiles(directoryPath, LANGUAGE_PACK_SEARCH_PATTERN, searchOption);

            foreach (var path in paths)
            {
                LanguagePack language;
                string fileName = Path.GetFileNameWithoutExtension(path);

                if (TryGetLanguagePack(path, out language))
                    yield return language;
            }
        }

        /// <summary>
        /// 尝试获取到语言包,若无法获取到则返回false;
        /// </summary>
        public static bool TryGetLanguagePack(string filePath, out LanguagePack language)
        {
            try
            {
                language = GetLanguagePack(filePath);
                return true;
            }
            catch
            {
                language = default(LanguagePack);
                return false;
            }
        }

        /// <summary>
        /// 获取到这个文件的语言类型,若无法获取到则返回 空白;
        /// </summary>
        public static string GetLanguage(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement(ROOT_ELEMENT_NAME))
                    {
                        if (reader.MoveToAttribute(LANGUAGE_ATTRIBUTE_NAME))
                        {
                            return reader.Value;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// 获取到语言包;
        /// </summary>
        public static LanguagePack GetLanguagePack(string filePath)
        {
            string language = GetLanguage(filePath);
            return new LanguagePack(language, new XmlFile(filePath));
        }


        public XmlFile(string filePath)
        {
            this.FilePath = filePath;
        }

        public string FilePath { get; private set; }

        public IEnumerable<TextPack> ReadTexts()
        {
            using (XmlReader reader = XmlReader.Create(FilePath, xmlReaderSettings))
            {
                return ReadTexts(reader);
            }
        }

        public void WriteTexts(string language, IEnumerable<TextPack> texts)
        {
            using (XmlWriter writer = XmlWriter.Create(FilePath, xmlWriterSettings))
            {
                WriteTexts(writer, language, texts);
            }
        }

    }

}
