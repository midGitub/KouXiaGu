using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    interface IObservableOperation
    {
        IDisposable Subscribe(IOperationObserver observer);
    }

    interface IObservableOperation<T>
    {
        IDisposable Subscribe(IOperationObserver<T> observer);
    }

    interface IOperationObserver
    {
        void OnCompleted();
        void OnFaulted(Exception ex);
        void OnCanceled();
    }

    interface IOperationObserver<T>
    {
        void OnCompleted(T item);
        void OnFaulted(Exception ex);
        void OnCanceled();
    }

}
