using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.Translates
{

    /// <summary>
    /// 本地化(大部分方法仅允许在Unity线程调用);
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// 语言包合集;
        /// </summary>
        public static LanguagePackGroup LanguagePackGroup { get; private set; }

        /// <summary>
        /// 观察者合集;
        /// </summary>
        static readonly List<ILanguageObserver> languageHandlers = new List<ILanguageObserver>();

        /// <summary>
        /// 当前语言类型;
        /// </summary>
        public static string Language
        {
            get { return LanguagePackGroup.Language; }
        }

        /// <summary>
        /// 设置新的语言,若需要移除语言包则设置为Null(仅在Unity线程调用);
        /// </summary>
        public static void SetLanguagePackGroup(LanguagePackGroup languageDictionary)
        {
            if (languageDictionary == LanguagePackGroup)
                return;

            LanguagePackGroup = languageDictionary;
            OnLanguageChanged();
        }

        /// <summary>
        /// 当语言发生变化时调用(仅在Unity线程调用);
        /// </summary>
        static void OnLanguageChanged()
        {
            foreach (var languageHandler in languageHandlers)
            {
                try
                {
                    languageHandler.OnLanguageChanged();
                }
                catch (Exception ex)
                {
                    Debug.LogError("在调用本地化观察者时出现异常: " + ex);
                }
            }
        }

        /// <summary>
        /// 订阅到观察者,并返回取消处置器;若已经加入了观察者合集,则返回Null(仅在Unity线程调用);
        /// </summary>
        public static IDisposable Subscribe(ILanguageObserver handler)
        {
            if (!languageHandlers.Contains(handler))
            {
                languageHandlers.Add(handler);
                var unsubscriber = new CollectionUnsubscriber<ILanguageObserver>(languageHandlers, handler);
                return unsubscriber;
            }
            return null;
        }

        /// <summary>
        /// 进行取消订阅(仅在Unity线程调用);
        /// </summary>
        public static bool Unsubscribe(ILanguageObserver handler)
        {
            return languageHandlers.Remove(handler);
        }

        /// <summary>
        /// 获取到对应文本,若未能获取到则返回 key;
        /// </summary>
        public static string Translate(string key)
        {
            if (LanguagePackGroup == null)
            {
                return key;
            }
            else
            {
                return LanguagePackGroup.Translate(key);
            }
        }
    }
}
