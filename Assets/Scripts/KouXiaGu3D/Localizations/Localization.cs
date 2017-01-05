﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 文字字典;
    /// </summary>
    [DisallowMultipleComponent]
    public class Localization : UnitySington<Localization>
    {

        static readonly CustomDictionary<string , string> textDictionary =
            new CustomDictionary<string, string>();

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();

        /// <summary>
        /// 读取器们;
        /// </summary>
        static readonly LinkedList<ILocalizationReader> readers = new LinkedList<ILocalizationReader>();

        /// <summary>
        /// 当前读取到的读取器;
        /// </summary>
        static LinkedListNode<ILocalizationReader> currentReader;

        /// <summary>
        /// 是否已经读取完毕?
        /// </summary>
        public static bool Initialized { get; private set; }


        /// <summary>
        /// 订阅字段更新器;
        /// </summary>
        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");
            if (!textObservers.Add(observer))
                throw new ArgumentException("重复订阅;");

            if (Initialized)
                UpdateTextObserver(observer);

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }

        /// <summary>
        /// 订阅到读取任务;
        /// </summary>
        public static void Subscribe(ILocalizationReader observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");
            if (readers.Contains(observer))
                throw new ArgumentException("重复订阅;");

            var node = readers.AddLast(observer);
        }

        /// <summary>
        /// 更新所有字符串订阅器(应该在Unity线程中);
        /// </summary>
        static void UpdateTextObservers()
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textObserver);
            }
        }

        /// <summary>
        /// 更新字符串信息;
        /// </summary>
        static void UpdateTextObserver(ITextObserver textObserver)
        {
            string text;
            if (textDictionary.TryGetValue(textObserver.Key, out text))
            {
                textObserver.SetText(text);
            }
            Debug.LogWarning("[本地化]未知字符串:" + textObserver.Key);
        }



        static void Read(ILocalizationReader reader)
        {
            foreach (var pair in reader.ReadTexts())
            {

            }
        }


        static void Add(string key, string value)
        {
            if (textDictionary.ContainsKey(key))
                Debug.LogWarning("[本地化]存在相同的字符串:" + key);

            textDictionary.Add(key, value);
        }



        void Update()
        {
            if (Initialized)
            {
                UpdateTextObservers();
                enabled = false;
            }
        }

    }

}
