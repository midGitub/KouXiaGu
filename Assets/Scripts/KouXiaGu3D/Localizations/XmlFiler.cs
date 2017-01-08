using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using KouXiaGu.Collections;

namespace KouXiaGu.Localizations
{


    public static class XmlFiler
    {

        /// <summary>
        /// 文件后缀;
        /// </summary>
        public const string FILE_EXTENSION = ".xml";

        /// <summary>
        /// 语言文件匹配的搜索字符串;
        /// </summary>
        const string LANGUAGE_PACK_SEARCH_PATTERN = "*" + FILE_EXTENSION;


        const string ROOT_ELEMENT_NAME = "LocalizationTexts";
        const string LANGUAGE_ATTRIBUTE_NAME = "Language";

        const string TEXT_ELEMENT_NAME = "Text";
        const string KEY_ATTRIBUTE_NAME = "key";
        const string VALUE_ATTRIBUTE_NAME = "value";
        const string UPDATE_MARK_ATTRIBUTE_NAME = "update";

        const bool DEFAULT_UPDATE_MARK = false;


        static readonly XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            NewLineChars = Environment.NewLine,
            NewLineOnAttributes = false,
            Encoding = Encoding.UTF8,
        };

        static readonly XmlReaderSettings xmlReaderSettings = new XmlReaderSettings()
        {
            IgnoreWhitespace = true,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
        };


        #region Read


        /// <summary>
        /// 使用枚举的方式获取到所有文本条目;
        /// </summary>
        public static IEnumerable<TextItem> ReadTextsEnumerate(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath, xmlReaderSettings))
            {
                IEnumerable<TextItem> texts = ReadTexts(reader);
                foreach (var text in texts)
                {
                    yield return text;
                }
            }
        }

        /// <summary>
        /// 读取到文件所有文本条目;
        /// </summary>
        public static List<TextItem> ReadTexts(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath, xmlReaderSettings))
            {
                IEnumerable<TextItem> texts = ReadTexts(reader);
                return new List<TextItem>(texts);
            }
        }

        /// <summary>
        /// 读取并返回所有文本条目;
        /// </summary>
        static IEnumerable<TextItem> ReadTexts(XmlReader reader)
        {
            reader.MoveToContent();

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
                        yield return new TextItem(key, value, updateMark);
                }
            }
        }


        /// <summary>
        /// 使用迭代的方式获取到文件的所有Key关键词;
        /// </summary>
        public static IEnumerable<string> ReadKeysEnumerate(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath, xmlReaderSettings))
            {
                IEnumerable<string> keys = ReadKeys(reader);
                foreach (var key in keys)
                {
                    yield return key;
                }
            }
        }

        /// <summary>
        /// 获取到文件的所有Key关键词;
        /// </summary>
        public static List<string> ReadKeys(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath, xmlReaderSettings))
            {
                IEnumerable<string> keys = ReadKeys(reader);
                return new List<string>(keys);
            }
        }

        static IEnumerable<string> ReadKeys(XmlReader reader)
        {
            reader.MoveToContent();

            while (reader.Read())
            {
                if (reader.IsStartElement(TEXT_ELEMENT_NAME))
                {
                    string content = reader.GetAttribute(KEY_ATTRIBUTE_NAME);

                    if (!string.IsNullOrEmpty(content))
                        yield return content;
                }
            }
        }


        /// <summary>
        /// 获取到目录下的所有语言包文件;
        /// </summary>
        public static IEnumerable<XmlLanguageFile> GetPacks(string directoryPath, SearchOption searchOption)
        {
            var paths = Directory.GetFiles(directoryPath, LANGUAGE_PACK_SEARCH_PATTERN, searchOption);

            foreach (var path in paths)
            {
                XmlLanguageFile pack;
                if (TryGetPack(path, out pack))
                    yield return pack;
            }
        }

        /// <summary>
        /// 尝试获取到这个文件的信息,若无法获取到则返回false;
        /// </summary>
        public static bool TryGetPack(string filePath, out XmlLanguageFile pack)
        {
            string language;
            if (TryGetLanguage(filePath, out language))
            {
                pack = new XmlLanguageFile(language, filePath);
                return true;
            }
            pack = default(XmlLanguageFile);
            return false;
        }

        /// <summary>
        /// 尝试获取到这个文件的语言信息,若无法获取到则返回false;
        /// </summary>
        public static bool TryGetLanguage(string filePath, out string language)
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                return TryGetLanguage(reader, out language);
            }
        }

        static bool TryGetLanguage(XmlReader reader, out string language)
        {
            language = default(string);
            reader.MoveToContent();

            if (reader.IsStartElement(ROOT_ELEMENT_NAME))
            {
                language = reader.GetAttribute(LANGUAGE_ATTRIBUTE_NAME);

                if (language != default(string))
                    return true;
            }
            return false;
        }

        #endregion


        #region Write


        /// <summary>
        /// 创建新的文件,并且写入所有文本条目;
        /// </summary>
        public static void CreateTexts(string filePath, string language, IEnumerable<TextItem> texts)
        {
            using (XmlWriter writer = XmlWriter.Create(filePath, xmlWriterSettings))
            {
                CreateTexts(writer, language, texts);
            }
        }

        /// <summary>
        /// 写入所有文本条目;
        /// </summary>
        static void CreateTexts(XmlWriter writer, string language, IEnumerable<TextItem> texts)
        {
            writer.WriteStartRoot(language);
            WriteTextElements(writer, texts);
            writer.WriteEndRoot();
        }


        /// <summary>
        /// 添加到Texts到文件内;
        /// </summary>
        public static void AppendTexts(string filePath, IEnumerable<TextItem> texts)
        {
            string language;

            if (TryGetLanguage(filePath, out language))
            {
                List<TextItem> original = ReadTexts(filePath);
                IEnumerable<TextItem> newTests = original.Append(texts);

                CreateTexts(filePath, language, newTests);
            }
            else
            {
                throw new InvalidOperationException("可能格式不符,无法读取文件内容;");
            }
        }


        /// <summary>
        /// 仅写入条目中的Key的值,其它值留空;
        /// </summary>
        public static void CreateKeys(string filePath, string language, IEnumerable<string> keys)
        {
            using (XmlWriter writer = XmlWriter.Create(filePath, xmlWriterSettings))
            {
                CreateKeys(writer, language, keys);
            }
        }

        /// <summary>
        /// 仅写入条目中的Key的值,其它值留空;
        /// </summary>
        static void CreateKeys(XmlWriter writer, string language, IEnumerable<string> keys)
        {
            writer.WriteStartRoot(language);
            WriteKeyElements(writer, keys);
            writer.WriteEndRoot();
        }




        static void WriteStartRoot(this XmlWriter writer, string language)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(ROOT_ELEMENT_NAME);

            writer.WriteStartAttribute(LANGUAGE_ATTRIBUTE_NAME);
            writer.WriteString(language);
            writer.WriteEndAttribute();
        }

        static void WriteEndRoot(this XmlWriter writer)
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        static void WriteTextElements(XmlWriter writer, IEnumerable<TextItem> texts)
        {
            foreach (var text in texts)
            {
                WriteTextElement(writer, text);
            }
        }

        static void WriteTextElement(this XmlWriter writer, TextItem text)
        {
            string key = text.Key;
            string value = text.Value;
            bool updateMark = text.IsUpdate;

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

        static void WriteKeyElements(XmlWriter writer, IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                WriteKeyElement(writer, key);
            }
        }

        /// <summary>
        /// 节点只输出 Key 和 Value 属性;
        /// </summary>
        static void WriteKeyElement(XmlWriter writer, string key)
        {
            writer.WriteStartElement(TEXT_ELEMENT_NAME);

            writer.WriteStartAttribute(KEY_ATTRIBUTE_NAME);
            writer.WriteString(key);

            writer.WriteStartAttribute(VALUE_ATTRIBUTE_NAME);

            writer.WriteEndElement();
        }

        #endregion

    }

}
