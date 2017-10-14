using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 本地化组件(大部分方法仅允许在Unity线程调用);
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// 语言包合集;
        /// </summary>
        internal static LanguagePackGroup LanguagePackGroup { get; private set; }

        /// <summary>
        /// 观察者合集;
        /// </summary>
        static readonly IObserverCollection<ILanguageObserver> observers = new ObserverLinkedList<ILanguageObserver>();

        /// <summary>
        /// 是否不允许更改当前语言;
        /// </summary>
        public static bool IsLockLanguage { get; internal set; } = false;

        /// <summary>
        /// 组件是否准备完成?
        /// </summary>
        public static bool IsReady
        {
            get { return LanguagePackGroup != null; }
        }

        /// <summary>
        /// 当前语言类型,若未进行设置则返回 "Unknow";
        /// </summary>
        public static string Language
        {
            get { return IsReady ? LanguagePackGroup.Language : "Unknow"; }
        }

        /// <summary>
        /// 所有语言包;若未初始化则为Null(仅在Unity线程调用);
        /// </summary>
        public static IReadOnlyCollection<LanguagePack> LanguagePacks
        {
            get { return LanguagePackGroup; }
        }

        /// <summary>
        /// 获取到对应文本,若未能获取到则返回 key;
        /// </summary>
        public static string Translate(string key)
        {
            return LanguagePackGroup.Translate(key);
        }

        /// <summary>
        /// 设置新的语言(仅在Unity线程调用);
        /// </summary>
        public static void SetLanguage(LanguagePackFileInfo languagePackFileInfo, Action<Task> callback = null)
        {
            ThrowIfLanguageIsLock();
            LocalizationController.Instance.ReadLanguagePack(languagePackFileInfo, callback);
        }

        /// <summary>
        /// 设置新的语言(仅在Unity线程调用);
        /// </summary>
        public static void SetLanguage(LanguagePackGroup languagePackGroup)
        {
            if (languagePackGroup == null)
                throw new ArgumentNullException("languagePackGroup");
            if (languagePackGroup == LanguagePackGroup)
                return;
            ThrowIfLanguageIsLock();

            LanguagePackGroup = languagePackGroup;
            OnLanguageChanged();
        }

        /// <summary>
        /// 锁不允许更改语言则抛出异常;
        /// </summary>
        static void ThrowIfLanguageIsLock()
        {
            if(IsLockLanguage)
            {
                throw new InvalidOperationException("不允许更改语言;");
            }
        }

        /// <summary>
        /// 在语言发生变化时调用(仅在Unity线程调用);
        /// </summary>
        internal static void OnLanguageChanged()
        {
            foreach (var languageHandler in observers.EnumerateObserver())
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
            if (!observers.Contains(handler))
            {
                return observers.Subscribe(handler);
            }
            return null;
        }

        /// <summary>
        /// 进行取消订阅(仅在Unity线程调用);
        /// </summary>
        public static bool Unsubscribe(ILanguageObserver handler)
        {
            return observers.Unsubscribe(handler);
        }
    }
}
