using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Rx;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 本地化语言文本管理;
    /// </summary>
    public static class Localization
    {

        public static IDictionary<string, string> textDictionary { get; private set; }
        static readonly HashSet<ITextObserver> observers = new HashSet<ITextObserver>();
        static List<LanguagePack> languages;
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
        public static List<LanguagePack> Languages
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

            Localization.textDictionary = textDictionary;
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

        /// <summary>
        /// 异步读取语言文件;
        /// </summary>
        internal static IAsyncOperation ReadAsync()
        {
            return LanguagePackReader.ReadAsync(out languages, out languageIndex);
        }

        /// <summary>
        /// 设置到语言;
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

    }

}
