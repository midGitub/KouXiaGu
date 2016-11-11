using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public interface IGameCreater
    {

        ///// <summary>
        ///// 当保存或者读取结束时调用;
        ///// </summary>
        //event Action OnEventDone;

        ///// <summary>
        ///// 当前的存档文件
        ///// </summary>
        //ArchiveExpand Archive { get; }

        ///// <summary>
        ///// 无法开始游戏时调用;
        ///// </summary>
        //void OnError(Exception error);
    }

    /// <summary>
    /// 游戏资源接口;
    /// </summary>
    public interface ILoadRes
    {
        /// <summary>
        /// 当前的存档文件
        /// </summary>
        ArchiveExpand Archive { get; }

        /// <summary>
        /// 在初始化时,游戏需要加载的资源目录;
        /// </summary>
        IEnumerable<string> ResPath { get; }
    }

    /// <summary>
    /// 保存游戏状态;
    /// </summary>
    public interface ISaveRes
    {
        /// <summary>
        /// 当前的存档文件
        /// </summary>
        ArchiveExpand Archive { get; }

        /// <summary>
        /// 存档将会保存到的位置;
        /// </summary>
        string Path { get; }
    }

}
