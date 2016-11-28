using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public interface IConstruct1<T>
    {
        IEnumerator Construction(T item);
    }

    public interface IConstruct2<T>
    {
        IEnumerator Prepare(T item);
        IEnumerator Construction(T item);
    }

}
