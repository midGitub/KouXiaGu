using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 负责异步加载语言包;
    /// </summary>
    [DisallowMultipleComponent]
    public class LocalizationLoader : UnitySington<LocalizationLoader>
    {
        LocalizationLoader() { }

        /// <summary>
        /// 等待加载的语言包;
        /// </summary>
        static readonly Queue<IFile> waitLoads = new Queue<IFile>();

        static bool isEmpty
        {
            get { return waitLoads.Count == 0; }
        }

        /// <summary>
        /// 异步的读取语言包;
        /// </summary>
        public static void LoadAsync(IFile item)
        {
            waitLoads.Enqueue(item);
        }

        /// <summary>
        /// 读取语言包;
        /// </summary>
        static void Load(IFile item)
        {
            foreach (var text in item.ReadTexts())
            {
                Localization.Add(text);
            }
        }

        void Update()
        {
            if (!isEmpty)
            {
                Load(waitLoads.Dequeue());
            }
        }

    }

}
