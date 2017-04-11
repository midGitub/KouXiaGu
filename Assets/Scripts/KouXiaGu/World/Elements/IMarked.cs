using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    /// <summary>
    /// 拥有一个名为 "ID" 的变量确定类型;
    /// </summary>
    public interface IMarked
    {
        int ID { get; }
    }
}
