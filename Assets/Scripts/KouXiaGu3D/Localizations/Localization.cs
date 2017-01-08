using System;
using KouXiaGu.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 文本内容;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Localization : UnitySington<Localization>
    {
        Localization() { }

        /// <summary>
        /// 配置信息;
        /// </summary>
        [SerializeField]
        LocalizationConfig config = new LocalizationConfig()
        {
            IsFollowSystemLanguage = true,
            LanguagePrioritys = new string[]
            {
                "English",
                "ChineseSimplified",
                "ChineseS",
            },
        };


        static readonly TextDictionary textDictionary = new TextDictionary();

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();


        public static IReadOnlyDictionary<string, string> TextDictionary
        {
            get { return textDictionary; }
        }

        /// <summary>
        /// 指定的配置;
        /// </summary>
        public static LocalizationConfig Config
        {
            get { return GetInstance.config; }
            private set { GetInstance.config = value; }
        }


        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");
            if (!textObservers.Add(observer))
                throw new ArgumentException("重复订阅;");

            UpdateTextObserver(observer);

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }


        /// <summary>
        /// 更新所有文本,在主线程内调用;
        /// </summary>
        public static void UpdateTextObservers()
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textObserver);
            }
        }

        static void UpdateTextObserver(ITextObserver textObserver)
        {
            string text;
            if (textDictionary.TryGetValue(textObserver.Key, out text))
            {
                textObserver.SetText(text);
                return;
            }
            textObserver.OnTextNotFound();
        }


        /// <summary>
        /// 读取\重新读取 主语言包;
        /// </summary>
        public static void ReadMain()
        {
            Config = LocalizationConfig.Read();
            ITextReader reader = Resources.GetReader(Config.GetLanguages().ToArray());

            ClearTexts();
            ReadTexts(reader);
            UpdateTextObservers();
        }

        /// <summary>
        /// 重新读取到所有文本,读取完毕后通知到监视者;
        /// </summary>
        public static void ReadTexts(IEnumerable<ITextReader> readers)
        {
            ClearTexts();

            foreach (var reader in readers)
            {
                ReadTexts(reader);
            }

            UpdateTextObservers();
        }

        /// <summary>
        /// 添加文本到现有字典内;
        /// </summary>
        public static void AddTexts(IEnumerable<ITextReader> readers)
        {
            foreach (var reader in readers)
            {
                ReadTexts(reader);
            }

            UpdateTextObservers();
        }

        static void ReadTexts(ITextReader reader)
        {
            foreach (var item in reader.ReadTexts())
            {
                if (!textDictionary.Add(item))
                    Debug.LogWarning("重复加入的文本条目:" + item);
            }

            Debug.Log("语言读取完毕:" + reader.ToString());
        }

        /// <summary>
        /// 清除所有文本内容;
        /// </summary>
        public static void ClearTexts()
        {
            textDictionary.Clear();
        }


        protected override void Awake()
        {
            base.Awake();
            ReadMain();
        }

    }

}
