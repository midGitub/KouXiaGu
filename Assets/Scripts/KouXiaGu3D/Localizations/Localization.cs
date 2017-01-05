using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    public static class Localization
    {

        static readonly Dictionary<string, string> textDictionary = new Dictionary<string, string>();

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();


        /// <summary>
        /// 当前系统的语言;
        /// </summary>
        public static SystemLanguage SystemLanguage
        {
            get { return Application.systemLanguage; }
        }


        /// <summary>
        /// 订阅到文本更新;
        /// </summary>
        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");
            if (!textObservers.Add(observer))
                throw new ArgumentException("重复订阅;");

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }

        /// <summary>
        /// 取消订阅文本更新;
        /// </summary>
        public static bool Unsubscribe(ITextObserver observer)
        {
            return textObservers.Remove(observer);
        }

        /// <summary>
        /// 更新所有文本观察者(应该在Unity线程中);
        /// </summary>
        public static void UpdateTextObservers()
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textObserver);
            }
        }

        /// <summary>
        /// 更新文本观察者内容;
        /// </summary>
        public static void UpdateTextObserver(ITextObserver textObserver)
        {
            string text;
            if (textDictionary.TryGetValue(textObserver.Key, out text))
            {
                textObserver.SetText(text);
            }
            Debug.LogWarning("[本地化]未知字符串:" + textObserver.Key);
        }

        /// <summary>
        /// 加入到文本字典;
        /// </summary>
        public static bool AddText(TextPack pack)
        {
            if (textDictionary.ContainsKey(pack.Key) && !pack.IsUpdate)
            {
                Debug.LogWarning("[本地化]存在相同的字符串:" + pack.ToString());
                return false;
            }

            textDictionary.AddOrUpdate(pack.Key, pack.Value);
            return true;
        }

    }

}
