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
        public static LanguagePack LanguagePack { get; internal set; }
        public static ILanguageDictionary LanguageDictionary => LanguagePack != null ? LanguagePack.LanguageDictionary : null;

        /// <summary>
        /// 所有可用的语言包(在进行初始化之后,仅提供Unity线程对此内容进行变更);
        /// </summary>
        public static List<LanguagePackInfo> AvailableLanguagePacks { get; set; }

        /// <summary>
        /// 观察者合集;
        /// </summary>
        private static readonly ObserverCollection<LanguageChangedEvent> observers = new ObserverLinkedList<LanguageChangedEvent>();

        /// <summary>
        /// 观察者订阅,并返回取消处置器;若已经加入了观察者合集,则返回异常;
        /// </summary>
        public static IDisposable Subscribe(IObserver<LanguageChangedEvent> observer)
        {
            return observers.Subscribe(observer);
        }

        /// <summary>
        /// 通知到观察者语言文本已经发生变化;
        /// </summary>
        public static void NotifyLanguageChanged()
        {
            LanguageChangedEvent changedEvent = new LanguageChangedEvent()
            {
                LanguagePack = LanguagePack,
            };
            observers.NotifyNext(changedEvent);
        }
    }
}
