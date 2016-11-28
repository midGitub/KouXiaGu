using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏构建调用;
    /// </summary>
    public interface IConstruct<T>
    {
        IEnumerator Prepare(T item);
        IEnumerator Construction(T item);
    }

}
