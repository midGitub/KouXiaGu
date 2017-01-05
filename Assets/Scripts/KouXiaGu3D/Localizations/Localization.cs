using System;
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
    /// 本地化文字字典;
    /// </summary>
    [DisallowMultipleComponent]
    public static class Localization
    {

        static readonly CustomDictionary<string , string> textDictionary =
            new CustomDictionary<string, string>();

        static readonly HashSet<ITextObserver> observers = new HashSet<ITextObserver>();


        public static IReadOnlyDictionary<string, string> TextDictionary
        {
            get { return textDictionary; }
        }

        public static IDisposable Subscribe(ITextObserver observer)
        {
            observers.Add(observer);
            return new CollectionUnsubscriber<ITextObserver>(observers, observer);
        }

    }

}
