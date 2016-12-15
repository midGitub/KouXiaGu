using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 可以保存的游戏内容,在存档时调用;
    /// </summary>
    public interface IPreservable
    {
        IEnumerator OnSave(Archiver archive);
    }

}
