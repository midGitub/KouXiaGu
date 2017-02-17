using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 变量观察者;
    /// </summary>
    public interface IObserver
    {

        /// <summary>
        /// 当变量发生变化时调用;
        /// </summary>
        void OnNext(IObservable provider);
    }

}
