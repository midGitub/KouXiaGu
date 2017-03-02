using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    public interface IObserver<T>
    {
        void OnError(Exception error);
        void OnCompleted();
        void OnNext(T item);
    }

}
