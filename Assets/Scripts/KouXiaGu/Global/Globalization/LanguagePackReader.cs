using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using KouXiaGu.Collections;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 语言包读写基类;
    /// </summary>
    public abstract class LanguagePackReader
    {
        public LanguagePackReader()
        {
        }

        public abstract string FileExtension { get; }

        protected virtual string LanguagePackSearchPattern
        {
            get { return "*" + FileExtension; }
        }


        /// <summary>
        /// 读取并返回所有文本的字典结构,若存在相同的 Key,则保留其后加入的内容;
        /// </summary>
        public LanguagePack Read(LanguageFile file)
        {
            var enumerateText = Read(file.FilePath);
            Dictionary<string, string> texts = ReadToDictionary(enumerateText);
            return new LanguagePack(file, texts);
        }

        /// <summary>
        /// 读取并返回所有文本的字典结构,若存在相同的 Key,则保留其后加入的内容;
        /// </summary>
        public Dictionary<string, string> ReadToDictionary(IEnumerable<KeyValuePair<string, string>> enumerateText)
        {
            Dictionary<string, string> texts = new Dictionary<string, string>();
            texts.AddOrUpdate(enumerateText);
            return texts;
        }

        /// <summary>
        /// 读取并返回所有文本条目;
        /// </summary>
        public abstract IEnumerable<KeyValuePair<string, string>> Read(string filePath);


        /// <summary>
        /// 获取到文件的所有Key关键词;
        /// </summary>
        public virtual IEnumerable<string> ReadKeysOnly(string filePath)
        {
            IEnumerable<KeyValuePair<string, string>> texts = Read(filePath);
            foreach (var text in texts)
            {
                yield return text.Key;
            }
        }


        /// <summary>
        /// 创建新的文件,并且写入所有文本条目;
        /// </summary>
        public LanguageFile CreateAndWrite(LanguagePack pack, string filePath)
        {
            var file = new LanguageFile(pack, filePath);
            CreateAndWrite(file, pack.TextDictionary);
            return file;
        }

        /// <summary>
        /// 创建新的文件,并且写入所有文本条目;
        /// </summary>
        public abstract void CreateAndWrite(LanguageFile pack, IEnumerable<KeyValuePair<string, string>> texts);

        /// <summary>
        /// 添加到Texts到文件内;
        /// </summary>
        public virtual void Append(string filePath, IDictionary<string, string> texts)
        {
            LanguageFile language;
            if (TryLoadFile(filePath, out language))
            {
                IEnumerable<KeyValuePair<string, string>> original = Read(filePath);
                var newTests = new Dictionary<string, string>(texts);
                newTests.AddOrUpdate(original);
                CreateAndWrite(language, newTests);
            }
            else
            {
                throw new InvalidOperationException("可能格式不符,无法读取文件内容;");
            }
        }

        /// <summary>
        /// 获取到目录下的所有语言包文件;
        /// </summary>
        public virtual IEnumerable<LanguageFile> SearchLanguagePacks(string directoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var paths = Directory.GetFiles(directoryPath, LanguagePackSearchPattern, searchOption);

            foreach (var path in paths)
            {
                LanguageFile pack;
                if (TryLoadFile(path, out pack))
                    yield return pack;
            }
        }

        /// <summary>
        /// 尝试获取到这个文件的语言信息,若无法获取到则返回false;
        /// </summary>
        public abstract bool TryLoadFile(string filePath, out LanguageFile pack);

    }

    /// <summary>
    /// 读取 Xml 格式的语言包;
    /// </summary>
    public class XmlLanguagePackReader : LanguagePackReader
    {
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

        const string RootElementName = "LocalizationTexts";
        const string LanguageNameAttributeName = "name";
        const string LanguageLocNameAttributeName = "locName";

        const string TextElementName = "Text";
        const string KeyAttributeName = "key";
        const string ValueAttributeName = "value";

        public override string FileExtension
        {
            get { return ".xml"; }
        }

        /// <summary>
        /// 读取并返回所有文本条目 延迟方法;
        /// </summary>
        public override IEnumerable<KeyValuePair<string, string>> Read(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath, xmlReaderSettings))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.IsStartElement(TextElementName))
                    {
                        string key = null;
                        string value = null;

                        while (reader.MoveToNextAttribute())
                        {
                            switch (reader.Name)
                            {
                                case KeyAttributeName:
                                    key = reader.Value;
                                    break;
                                case ValueAttributeName:
                                    value = reader.Value;
                                    break;
                            }
                        }

                        if (key != null && value != null)
                            yield return new KeyValuePair<string, string>(key, value);
                    }
                }
            }
        }

        /// <summary>
        /// 获取到文件的所有Key关键词 延迟方法;
        /// </summary>
        public override IEnumerable<string> ReadKeysOnly(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath, xmlReaderSettings))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.IsStartElement(TextElementName))
                    {
                        string content = reader.GetAttribute(KeyAttributeName);

                        if (!string.IsNullOrEmpty(content))
                            yield return content;
                    }
                }
            }
        }

        /// <summary>
        /// 尝试获取到这个文件的语言信息,若无法获取到则返回false;
        /// </summary>
        public override bool TryLoadFile(string filePath, out LanguageFile pack)
        {
            if (!File.Exists(filePath))
                goto OnFail;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                string name = null;
                string tag = null;

                reader.MoveToContent();

                if (reader.IsStartElement(RootElementName))
                {
                    name = reader.GetAttribute(LanguageNameAttributeName);
                    tag = reader.GetAttribute(LanguageLocNameAttributeName);

                    if (!string.IsNullOrEmpty(tag))
                    {
                        pack = new LanguageFile(name, tag, filePath);
                        return true;
                    }
                }
            }

            OnFail:
            pack = default(LanguageFile);
            return false;
        }


        /// <summary>
        /// 创建新的文件,并且写入所有文本条目;
        /// </summary>
        public override void CreateAndWrite(LanguageFile pack, IEnumerable<KeyValuePair<string, string>> texts)
        {
            using (XmlWriter writer = XmlWriter.Create(pack.FilePath, xmlWriterSettings))
            {
                WriteStartRoot(writer, pack);
                WriteTextElements(writer, texts);
                WriteEndRoot(writer);
            }
        }

        void WriteStartRoot(XmlWriter writer, LanguageFile pack)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(RootElementName);

            writer.WriteStartAttribute(LanguageNameAttributeName);
            writer.WriteString(pack.Name);

            writer.WriteStartAttribute(LanguageLocNameAttributeName);
            writer.WriteString(pack.LocName);
            writer.WriteEndAttribute();
        }

        void WriteEndRoot(XmlWriter writer)
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        void WriteTextElements(XmlWriter writer, IEnumerable<KeyValuePair<string, string>> texts)
        {
            foreach (var text in texts)
            {
                WriteTextElement(writer, text);
            }
        }

        void WriteTextElement(XmlWriter writer, KeyValuePair<string, string> text)
        {
            string key = text.Key;
            string value = text.Value;

            writer.WriteStartElement(TextElementName);

            writer.WriteStartAttribute(KeyAttributeName);
            writer.WriteString(key);

            writer.WriteStartAttribute(ValueAttributeName);
            writer.WriteString(value);

            writer.WriteEndElement();
        }

    }

}
