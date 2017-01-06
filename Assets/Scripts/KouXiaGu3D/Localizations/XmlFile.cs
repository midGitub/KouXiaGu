using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace KouXiaGu.Localizations
{


    public static class XmlFile
    {

        /// <summary>
        /// 文件后缀;
        /// </summary>
        public const string FILE_EXTENSION = ".xml";

        const string ROOT_ELEMENT_NAME = "Texts";
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

        public static IEnumerable<TextPack> ReadTexts(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath, xmlReaderSettings))
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
        }

        public static void WriteTexts(string filePath, string language, IEnumerable<TextPack> texts)
        {
            using (XmlWriter writer = XmlWriter.Create(filePath, xmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(ROOT_ELEMENT_NAME);
                writer.WriteStartAttribute(LANGUAGE_ATTRIBUTE_NAME);
                writer.WriteString(language);
                writer.WriteEndAttribute();

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


    }

}
