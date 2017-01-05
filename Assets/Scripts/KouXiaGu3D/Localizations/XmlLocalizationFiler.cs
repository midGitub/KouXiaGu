using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Localizations
{


    public class XmlLocalizationFiler
    {

        const string ROOT_ELEMENT_NAME = "Localization";
        const string TEXT_ELEMENT_NAME = "Text";
        const string KEY_ATTRIBUTE_NAME = "key";
        const string VALUE_ATTRIBUTE_NAME = "value";


        XmlLocalizationFiler(string filePath)
        {
            this.FilePath = filePath;
        }


        public string FilePath { get; private set; }

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

        public static XmlWriterSettings XmlWriterSettings
        {
            get { return xmlWriterSettings; }
        }


        public IEnumerable<KeyValuePair<string, string>> GetAllTexts()
        {
            using (XmlReader reader = XmlReader.Create(FilePath, xmlReaderSettings))
            {
                return ReadTexts(reader);
            }
        }


        /// <summary>
        /// 保存所有文字结构到XML;
        /// </summary>
        public static void WriteTexts(XmlWriter writer, IEnumerable<KeyValuePair<string, string>> texts)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(ROOT_ELEMENT_NAME);

            foreach (var pair in texts)
            {
                string key = pair.Key;
                string value = pair.Value;

                writer.WriteStartElement(TEXT_ELEMENT_NAME);

                writer.WriteStartAttribute(KEY_ATTRIBUTE_NAME);
                writer.WriteString(key);

                writer.WriteStartAttribute(VALUE_ATTRIBUTE_NAME);
                writer.WriteString(value);

                writer.WriteEndElement();
            }

            writer.WriteEndDocument();
        }

        /// <summary>
        /// 读取到所有文字结构;
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> ReadTexts(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement(TEXT_ELEMENT_NAME))
                {
                    string key = null;
                    string value = null;

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
                        }
                    }

                    if (key != null && value != null)
                        yield return new KeyValuePair<string, string>(key, value);
                }
            }
        }



        //public void 


        //[ContextMenu("Output")]
        //void Output()
        //{
        //    Dictionary<string, string> texts = new Dictionary<string, string>()
        //    {
        //        { "你的名字", "A佬"},
        //        { "我的名字", "B佬"},
        //        { "你的名字1", "A佬"},
        //        { "我的名字1", "B佬"},
        //        { "你的名字2", "A佬"},
        //        { "我的名字2", "B佬"},
        //    };

        //    using (XmlWriter xmlWriter = XmlWriter.Create(filePath, XmlWriterSettings))
        //    {
        //        WriteTexts(xmlWriter, texts);
        //    }
        //}

        //[ContextMenu("Input")]
        //void Input()
        //{
        //    using (XmlReader xmlReader = XmlReader.Create(filePath, xmlReaderSettings))
        //    {
        //        Debug.Log(ReadTexts(xmlReader).ToEnumerableLog());
        //    }
        //}

    }

}
