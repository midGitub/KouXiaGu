using System;
using System.Collections.Generic;
using KouXiaGu.Collections;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 本地化文本订阅器;
    /// </summary>
    public static class LocalizationText
    {

        static IReadOnlyDictionary textDictionary;

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();


        public static IReadOnlyDictionary TextDictionary
        {
            get { return textDictionary; }
        }

        public static bool IsInitialized
        {
            get { return textDictionary != null; }
        }


        /// <summary>
        /// 是否已经订阅?O(1)
        /// </summary>
        public static bool IsSubecribe(ITextObserver observer)
        {
            return textObservers.Contains(observer);
        }

        /// <summary>
        /// 订阅到文本更新;
        /// </summary>
        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");

            textObservers.Add(observer);

            if (IsInitialized)
                UpdateTextObserver(observer);

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }

        /// <summary>
        /// 更新文本信息;
        /// </summary>
        public static void UpdateTextObserver(ITextObserver textObserver)
        {
            textObserver.UpdateTexts(textDictionary);
        }


        /// <summary>
        /// 更新文本字典,应在主线程内调用;
        /// </summary>
        public static void UpdateTextDictionary(IReadOnlyDictionary textDictionary)
        {
            if (textDictionary == null)
                throw new ArgumentNullException();

            LocalizationText.textDictionary = textDictionary;
            UpdateTextObservers();
        }

        /// <summary>
        /// 更新所有文本内容,在主线程内调用;
        /// </summary>
        static void UpdateTextObservers()
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textObserver);
            }
        }


    }

}
