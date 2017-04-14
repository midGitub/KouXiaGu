using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using KouXiaGu.Collections;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 本地化语言文本管理;
    /// </summary>
    public static class Localization
    {
        public static IDictionary<string, string> textDictionary { get; private set; }
        static readonly HashSet<ITextObserver> observers = new HashSet<ITextObserver>();
        static ReadOnlyCollection<LanguagePack> languages;
        static int languageIndex;

        /// <summary>
        /// 词条总数;
        /// </summary>
        public static int EntriesCount
        {
            get { return textDictionary != null ? textDictionary.Count : 0; }
        }

        /// <summary>
        /// 只读的语言文件合集;
        /// </summary>
        public static ReadOnlyCollection<LanguagePack> Languages
        {
            get { return languages; }
        }

        /// <summary>
        /// 当前读取的语言文件下标;
        /// </summary>
        public static int LanguageIndex
        {
            get { return languageIndex; }
        }

        /// <summary>
        /// 订阅到文本变化;
        /// </summary>
        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");
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

            Localization.textDictionary = textDictionary;
        }

        /// <summary>
        /// 需要在Unity线程内调用,通知到所有观察者,文本内容发生变化;
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
        /// 确认是否存在这个观察者;
        /// </summary>
        public static bool Contains(ITextObserver observer)
        {
            return observers.Contains(observer);
        }

        /// <summary>
        /// 设置到语言;传入语言下标;
        /// </summary>
        public static void SetLanguage(int languageIndex)
        {
            var pack = Languages[languageIndex];
            var temp = textDictionary;
            SetTextDictionaryToNull();
            LanguagePackReader.Read(pack, temp);
        }

        /// <summary>
        /// 设置语言字典到空;
        /// </summary>
        static void SetTextDictionaryToNull()
        {
            Localization.textDictionary = null;
        }


        /// <summary>
        /// 异步读取语言文件;
        /// </summary>
        internal static IAsyncOperation ReadAsync()
        {
            return LanguagePackReader.ReadAsync(out languages, out languageIndex);
        }

    }

    /// <summary>
    /// 语言文件读取方法;
    /// </summary>
    public abstract class LocalizationInfo
    {
        static readonly LanguagerReader languagerReader = new XmlLanguagerReader();
        static readonly XmlLocalizationConfigReader configReader = new XmlLocalizationConfigReader();

        const string LocalizationConfigFileName = "Localization/Config.xml";
        const string LanguagePackDirectoryName = "Localization";

        public static IAsyncOperation<Dictionary<string, string>> ReadAsync(
            out ReadOnlyCollection<LanguagePack> languages,
            out int languageIndex)
        {
            throw new NotImplementedException();
        }

        public LocalizationConfig Config { get; private set; }

        /// <summary>
        /// 获取到所有语言包文件;
        /// </summary>
        public ReadOnlyCollection<LanguagePack> SearchLanguagePacks()
        {
            return SearchLanguagePacks(LanguagePackDirectoryName);
        }

        /// <summary>
        /// 获取到所有语言包文件;
        /// </summary>
        public ReadOnlyCollection<LanguagePack> SearchLanguagePacks(string dirPath)
        {
            var packs = languagerReader.SearchLanguagePacks(dirPath);
            IList<LanguagePack> packList = packs as IList<LanguagePack> ?? packs.ToArray();
            var collection = new ReadOnlyCollection<LanguagePack>(packList);
            return collection;
        }



    }

    [XmlType("LocalizationConfig")]
    public struct LocalizationConfig
    {
        [XmlElement("LocName")]
        public string LocName;
    }


    public class XmlLocalizationConfigReader
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LocalizationConfig));

        public static LocalizationConfig Read(string filePath)
        {
            return (LocalizationConfig)serializer.DeserializeXiaGu(filePath);
        }

        public static void Write(LocalizationConfig item, string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);

            if (Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            serializer.SerializeXiaGu(filePath, item);
        }
    }

}
