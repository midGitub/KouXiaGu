using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{

    public interface IStageObserver<T> : IStageEnter<T>, IStageLeave<T>
    {

    }

    public interface IStageEnter<T>
    {
        IEnumerator OnEnter(T item);
    }

    public interface IStageLeave<T>
    {
        IEnumerator OnLeave(T item);
    }

}
