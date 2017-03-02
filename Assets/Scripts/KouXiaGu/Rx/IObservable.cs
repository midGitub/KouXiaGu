using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    public interface IObservable<T>
    {
        IDisposable Subscribe(IObserver<T> observer);
    }

}
