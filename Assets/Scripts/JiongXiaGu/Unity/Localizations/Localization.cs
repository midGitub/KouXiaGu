using System;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 本地化信息(仅Unity线程调用);
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// 当前使用的语言包;(若不存在则为Null)
        /// </summary>
        internal static LanguagePack language;

        /// <summary>
        /// 所有可用的语言包(在进行初始化之后,仅提供Unity线程对此内容进行变更);
        /// </summary>
        internal static List<LanguagePackInfo> AvailableLanguagePacks { get; set; }

        /// <summary>
        /// 观察者合集;
        /// </summary>
        private static readonly ObserverCollection<LanguageChangedEvent> observers = new ObserverLinkedList<LanguageChangedEvent>();

        /// <summary>
        /// 当前使用的语言字典;(若不存在则为Null)
        /// </summary>
        public static IReadOnlyPack Language
        {
            get { return language; }
        }

        /// <summary>
        /// 设置新的语言(此方法不会通知观察者);
        /// </summary>
        public static void SetLanguage(LanguagePack pack)
        {
            language = pack;
        }

        /// <summary>
        /// 尝试获取到对应文本,若未能获取到则返回 false;
        /// </summary>
        public static bool TryTranslate(string key, out string value)
        {
            if (language != null)
            {
                return language.TryTranslate(key, out value);
            }
            value = default(string);
            return false;
        }

        /// <summary>
        /// 观察者订阅,并返回取消处置器;若已经加入了观察者合集,则返回Null;
        /// </summary>
        public static IDisposable Subscribe(IObserver<LanguageChangedEvent> observer)
        {
            if (!observers.Contains(observer))
            {
                return observers.Add(observer);
            }
            return null;
        }

        /// <summary>
        /// 移除订阅者;此方法不会调用观察者的 OnCompleted() 方法;
        /// </summary>
        public static bool Unsubscribe(IObserver<LanguageChangedEvent> observer)
        {
            return observers.Remove(observer);
        }

        /// <summary>
        /// 通知到观察者语言文本已经发生变化;
        /// </summary>
        public static void NotifyLanguageChanged()
        {
            LanguageChangedEvent changedEvent = new LanguageChangedEvent()
            {
                LanguageDictionary = language,
            };
            observers.NotifyNext(changedEvent);
        }
    }
}
