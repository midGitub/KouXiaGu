﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 通过存档对游戏进行恢复状态;
    /// </summary>
    public interface IRecoveryOperate : IAsyncOperation
    {
        /// <summary>
        /// 初始化内容;
        /// </summary>
        Action Initialize(ArchiveFile archive);
    }

}
