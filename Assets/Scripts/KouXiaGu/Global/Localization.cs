using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using KouXiaGu.Collections;
using KouXiaGu.Rx;
using UnityEngine;
using System.Xml.Serialization;

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
        static LocalManager()
        {
            Filer = new XmlTextPackFiler();
        }

        public static LanguagePackFiler Filer { get; private set; }
        public static IDictionary<string, string> textDictionary { get; private set; }
        static readonly HashSet<ITextObserver> observers = new HashSet<ITextObserver>();

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

        static void TrackObserver(ITextObserver observer)
        {
            observer.UpdateTexts(textDictionary);
        }

        /// <summary>
        /// 设置新的 文本字典, 需要手动 TrackAll() 所有订阅者;
        /// </summary>
        public static void SetTextDictionary(IDictionary<string, string> textDictionary)
        {
            if (textDictionary == null)
                throw new ArgumentNullException();

            LocalManager.textDictionary = textDictionary;
        }

        /// <summary>
        /// 需要在Unity线程内调用;
        /// </summary>
        public static void TrackAll()
        {
            ITextObserver[] observerArray = observers.ToArray();
            foreach (var observer in observerArray)
            {
                TrackObserver(observer);
            }
        }

        /// <summary>
        /// 需要在Unity线程内调用;
        /// </summary>
        public static IEnumerator TrackAllAsync()
        {
            ITextObserver[] observerArray = observers.ToArray();
            foreach (var observer in observerArray)
            {
                TrackObserver(observer);
                yield return null;
            }
        }

        public static bool Contains(ITextObserver observer)
        {
            return observers.Contains(observer);
        }

    }

    /// <summary>
    /// 负责读取本地化文本;
    /// </summary>
    public sealed class LanguagePackReader : MonoBehaviour
    {
        LanguagePackReader()
        {
            IsInitialized = false;
        }

        static List<string> searchDirectoryPaths = new List<string>()
        {
            Path.Combine(Application.streamingAssetsPath, "Localization"),
        };

        /// <summary>
        /// 在这些位置寻找到语言文件;
        /// </summary>
        public static List<string> SearchDirectoryPaths
        {
            get { return searchDirectoryPaths; }
        }

        /// <summary>
        /// 是否已经在读取中?
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// 已经 或 正在 读取的语言名;
        /// </summary>
        public static string LocName { get; private set; }

        /// <summary>
        /// 默认读取的语言;
        /// </summary>
        const string DefaultLocName = "zh-cn";

        /// <summary>
        /// 根据预留的语言信息,读取到语言文件;
        /// </summary>
        public static void Init()
        {
            try
            {
                Config config = Config.Read();
                Read(config.LocName);
            }
            catch (FileNotFoundException)
            {
                Read(DefaultLocName);
                WriteDefaultConfigFile();
            }
        }

        static void WriteDefaultConfigFile()
        {
            Config config = new Config()
            {
                LocName = DefaultLocName,
            };

            Config.Write(config);
        }

        /// <summary>
        /// 读取这个语言的语言包;
        /// </summary>
        public static void Read(string locName)
        {
            if (IsInitialized)
                throw new ArgumentException();

            LocName = locName;
            var instance = new GameObject("LanguagePackReader", typeof(LanguagePackReader));
        }


        Dictionary<string, string> textDictionary;

        void Start()
        {
            ReadLanguagePack();
        }

        void ReadLanguagePackAsync()
        {
            throw new NotImplementedException();
        }

        void ReadLanguagePack()
        {
            textDictionary = new Dictionary<string, string>();

            foreach (var searchDirectoryPath in SearchDirectoryPaths)
            {
                var packs = LocalManager.Filer.GetPacks(searchDirectoryPath);
                List<LanguagePack> packList = new List<LanguagePack>(packs);

                if (packList.Count == 0)
                {
                    Debug.LogWarning("Not Found Any LanguagePack In :" + searchDirectoryPath);
                    continue;
                }

                LanguagePack pack = GetPack(packList);
                Read(pack);
            }

            LocalManager.SetTextDictionary(textDictionary);
            LocalManager.TrackAll();
            IsInitialized = false;
            Destroy(gameObject);
        }

        LanguagePack GetPack(List<LanguagePack> packs)
        {
            var index = packs.FindIndex(pack => pack.LocName == LocName);
            if (index < 0)
            {
                index = 0;
            }
            return packs[index];
        }

        void Read(LanguagePack pack)
        {
            IEnumerable<KeyValuePair<string, string>> texts = LocalManager.Filer.Read(pack);
            textDictionary.AddOrUpdate(texts);
        }

        [XmlType("Config")]
        public struct Config
        {

            static readonly XmlSerializer serializer = new XmlSerializer(typeof(Config));

            public static string ConfigFilePath
            {
                get { return Path.Combine(Application.streamingAssetsPath, "Localization/Config.xml"); }
            }

            public static Config Read()
            {
                return (Config)serializer.DeserializeXiaGu(ConfigFilePath);
            }

            public static void Write(Config item)
            {
                string path = ConfigFilePath;
                string directoryPath = Path.GetDirectoryName(path);

                if (Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                serializer.SerializeXiaGu(ConfigFilePath, item);
            }

            [XmlElement("LocName")]
            public string LocName;

        }

    }

    /// <summary>
    /// 语言包读写基类;
    /// </summary>
    public abstract class LanguagePackFiler
    {
        public LanguagePackFiler()
        {
        }

        public abstract string FileExtension { get; }

        protected virtual string LanguagePackSearchPattern
        {
            get { return "*" + FileExtension; }
        }

        public IEnumerable<KeyValuePair<string, string>> Read(LanguagePack pack)
        {
            return Read(pack.FilePath);
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
        public virtual IEnumerable<LanguagePack> GetPacks(string directoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
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

    }

    /// <summary>
    /// 语言包信息;
    /// </summary>
    public struct LanguagePack
    {
        public LanguagePack(string name, string locName, string path)
        {
            Name = name;
            LocName = locName;
            FilePath = path;
        }

        public string Name { get; private set; }
        public string LocName { get; private set; }
        public string FilePath { get; private set; }

        public bool Exists()
        {
            return File.Exists(FilePath);
        }

    }

    /// <summary>
    /// 读取 Xml 格式的语言包;
    /// </summary>
    public class XmlTextPackFiler : LanguagePackFiler
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
        const string LanguageLocNameAttributeName = "languageTag";

        const string TextElementName = "Text";
        const string KeyAttributeName = "key";
        const string ValueAttributeName = "value";

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
                    tag = reader.GetAttribute(LanguageLocNameAttributeName);

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
