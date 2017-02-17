using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 可观察变量;
    /// </summary>
    public interface IObservable
    {

        /// <summary>
        /// 观察者订阅;
        /// </summary>
        IDisposable Subscribe(IObserver observer);
    }

}
