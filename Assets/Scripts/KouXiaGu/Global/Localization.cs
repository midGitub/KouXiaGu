using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using KouXiaGu.Collections;
using KouXiaGu.Rx;

namespace KouXiaGu.Globalization
{

    public interface ITextObserver
    {
        void UpdateTexts(IDictionary<string, string> textDictionary);
    }

    /// <summary>
    /// 本地化语言文本管理;
    /// </summary>
    public static class LocalManager
    {

        public static IDictionary<string, string> textDictionary { get; private set; }
        static HashSet<ITextObserver> observers = new HashSet<ITextObserver>();

        /// <summary>
        /// 订阅到文本变化;
        /// </summary>
        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException();
            if (!observers.Add(observer))
                throw new ArgumentException();

            if(textDictionary != null)
                TrackObserver(observer);

            return new CollectionUnsubscriber<ITextObserver>(observers, observer);
        }

        public static void SetTextDictionary(IDictionary<string, string> textDictionary)
        {
            if (textDictionary == null)
                throw new ArgumentNullException();

            LocalManager.textDictionary = textDictionary;
            TrackAll();
        }

        static void TrackAll()
        {
            ITextObserver[] observerArray = observers.ToArray();
            foreach (var observer in observerArray)
            {
                TrackObserver(observer);
            }
        }

        static void TrackObserver(ITextObserver observer)
        {
            observer.UpdateTexts(textDictionary);
        }

        public static bool Contains(ITextObserver observer)
        {
            return observers.Contains(observer);
        }

    }

    /// <summary>
    /// 语言包读写基类;
    /// </summary>
    public abstract class TextPackFiler
    {

        const string DefaultLanguageTag = "zh-cn";

        public TextPackFiler()
        {
            SearchDirectoryPaths = new List<string>();
        }

        /// <summary>
        /// 搜寻语言包的文件夹路径;
        /// </summary>
        public List<string> SearchDirectoryPaths { get; private set; }

        public abstract string FileExtension { get; }

        protected virtual string LanguagePackSearchPattern
        {
            get { return "*" + FileExtension; }
        }


        /// <summary>
        /// 读取到这个语言的所以语言包;
        /// </summary>
        public IDictionary<string, string> ReadLanguagePack(string language)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var path in SearchDirectoryPaths)
            {
                List<LanguagePack> packs = GetPacks(path, SearchOption.TopDirectoryOnly).ToList();

                int targetIndex = packs.FindIndex(pack => pack.Tag == language);

                if (targetIndex < 0)
                    targetIndex = packs.FindIndex(pack => pack.Tag == DefaultLanguageTag);

                if (targetIndex >= 0)
                {
                    LanguagePack pack = packs[targetIndex];
                    Read(pack, dictionary);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 读取到语言包的所有文本条目,并且加入到 dictionary 内,若存在相同的 Key 则替换;
        /// </summary>
        void Read(LanguagePack pack, IDictionary<string, string> dictionary)
        {
            var texts = Read(pack.FilePath);
            dictionary.AddOrUpdate(texts);
        }

        /// <summary>
        /// 读取并返回所有文本条目;
        /// </summary>
        protected abstract IEnumerable<KeyValuePair<string, string>> Read(string filePath);

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
        public abstract void Create(LanguagePack pack, IEnumerable<KeyValuePair<string, string>> texts);

        /// <summary>
        /// 添加到Texts到文件内;
        /// </summary>
        public virtual void Append(string filePath, IDictionary<string, string> texts)
        {
            LanguagePack language;
            if (TryLoadFile(filePath, out language))
            {
                IEnumerable<KeyValuePair<string, string>> original = Read(filePath);
                var newTests = new Dictionary<string, string>(texts);
                newTests.AddOrUpdate(original);
                Create(language, newTests);
            }
            else
            {
                throw new InvalidOperationException("可能格式不符,无法读取文件内容;");
            }
        }


        /// <summary>
        /// 获取到目录下的所有语言包文件;
        /// </summary>
        public IEnumerable<LanguagePack> GetPacks(string directoryPath, SearchOption searchOption)
        {
            var paths = Directory.GetFiles(directoryPath, LanguagePackSearchPattern, searchOption);

            foreach (var path in paths)
            {
                LanguagePack pack;
                if (TryLoadFile(path, out pack))
                    yield return pack;
            }
        }

        /// <summary>
        /// 尝试获取到这个文件的语言信息,若无法获取到则返回false;
        /// </summary>
        public abstract bool TryLoadFile(string filePath, out LanguagePack pack);


        /// <summary>
        /// 语言包信息;
        /// </summary>
        public struct LanguagePack
        {
            public LanguagePack(string name, string tag, string path)
            {
                Name = name;
                Tag = tag;
                FilePath = path;
            }

            public string Name { get; private set; }
            public string Tag { get; private set; }
            public string FilePath { get; private set; }

            public bool Exists()
            {
                return File.Exists(FilePath);
            }

        }

    }

    /// <summary>
    /// Xml 格式的语言包;
    /// </summary>
    public class XmlTextPackFiler : TextPackFiler
    {

        const string fileExtension = ".xml";

        public override string FileExtension
        {
            get { return fileExtension; }
        }

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
        const string LanguageTagAttributeName = "languageTag";

        const string TextElementName = "Text";
        const string KeyAttributeName = "key";
        const string ValueAttributeName = "value";

        /// <summary>
        /// 读取并返回所有文本条目 延迟方法;
        /// </summary>
        protected override IEnumerable<KeyValuePair<string, string>> Read(string filePath)
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
        public override bool TryLoadFile(string filePath, out LanguagePack pack)
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
                    tag = reader.GetAttribute(LanguageTagAttributeName);

                    if (!string.IsNullOrEmpty(tag))
                    {
                        pack = new LanguagePack(name, tag, filePath);
                        return true;
                    }
                }
            }

            OnFail:
            pack = default(LanguagePack);
            return false;
        }


        /// <summary>
        /// 创建新的文件,并且写入所有文本条目;
        /// </summary>
        public override void Create(LanguagePack pack, IEnumerable<KeyValuePair<string, string>> texts)
        {
            using (XmlWriter writer = XmlWriter.Create(pack.FilePath, xmlWriterSettings))
            {
                WriteStartRoot(writer, pack);
                WriteTextElements(writer, texts);
                WriteEndRoot(writer);
            }
        }

        void WriteStartRoot(XmlWriter writer, LanguagePack pack)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(RootElementName);

            writer.WriteStartAttribute(LanguageNameAttributeName);
            writer.WriteString(pack.Name);

            writer.WriteStartAttribute(LanguageTagAttributeName);
            writer.WriteString(pack.Tag);
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
