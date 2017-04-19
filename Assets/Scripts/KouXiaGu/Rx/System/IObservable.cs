using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    public interface IXiaGuObservable<T>
    {
        IDisposable Subscribe(IXiaGuObserver<T> observer);
    }

}
