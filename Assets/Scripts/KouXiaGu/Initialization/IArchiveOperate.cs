﻿using System;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏存档操作;
    /// </summary>
    public interface IArchiveOperate : IAsync
    {
        /// <summary>
        /// 进行保存;
        /// </summary>
        void SaveState(ArchiveFile archive);
    }

}
