using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{


    public interface IWalker
    {
        /// <summary>
        /// 允许行走的地形类型;
        /// </summary>
        int WalkableTagMask { get; }
    }
}
