using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using UniRx;

namespace KouXiaGu.Localizations
{

    public class ObservableText : IObservableText
    {
        public ObservableText()
        {
            textObservers = new HashSet<ITextObserver>();
        }

        readonly HashSet<ITextObserver> textObservers;

        /// <summary>
        /// 订阅到文本更新;
        /// </summary>
        public IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");
            if (!textObservers.Add(observer))
                throw new ArgumentException("重复订阅;");

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }

        /// <summary>
        /// 更新所有文本(应该在Unity线程中);
        /// </summary>
        public void UpdateTextObservers(IDictionary<string, string> textDictionary)
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textDictionary, textObserver);
            }
        }

        /// <summary>
        /// 更新文本观察者内容;
        /// </summary>
        public void UpdateTextObserver(IDictionary<string, string> textDictionary, ITextObserver textObserver)
        {
            string text;
            if (textDictionary.TryGetValue(textObserver.Key, out text))
            {
                textObserver.SetText(text);
            }
            textObserver.OnTextNotFound();
        }

    }

}
