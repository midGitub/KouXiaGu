using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{
    public interface IState
    {
        /// <summary>
        /// 是否已经取消?
        /// </summary>
        bool IsCanceled { get; }
    }
}
