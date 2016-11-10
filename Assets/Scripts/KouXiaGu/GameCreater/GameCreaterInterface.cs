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

    public interface IUnloadInCoroutine
    {
        /// <summary>
        /// 卸载已经加载的游戏资源;
        /// </summary>
        IEnumerator Unload();
    }

    /// <summary>
    /// 在协程中准备游戏资源;
    /// </summary>
    public interface ILoadInCoroutine : IUnloadInCoroutine
    {
        /// <summary>
        /// 开始读取游戏;
        /// 若在读取的过程中 IsStop 置为true,则停止加载,并且卸载已加载的资源;
        /// 若在进入读取时 IsLock 为true则直接返回;
        /// </summary>
        /// <param name="resLoad"></param>
        /// <returns>任意返回值</returns>
        IEnumerator Load(ILoadRes resLoad, Action<Exception> onError, Action loadingDoneCallBreak);

    }

    /// <summary>
    /// 多线程准备游戏资源;
    /// </summary>
    public interface ILoadInThread : IUnloadInCoroutine
    {
        /// <summary>
        /// 开始读取游戏;
        /// 若在读取的过程中 IsStop 置为true,则停止加载,并且卸载已加载的资源;
        /// 若在进入读取时 IsLock 为true则直接返回;
        /// </summary>
        /// <param name="resLoad"></param>
        void Load(ILoadRes resLoad, Action<Exception> onError, Action loadingDoneCallBreak);

    }

    /// <summary>
    /// 在协程中保存游戏状态;
    /// </summary>
    public interface ISaveInCoroutine
    {
        /// <summary>
        /// 保存游戏状态;
        /// 若在读取的过程中 IsStop 置为true,则停止保存;
        /// 若在进入读取时 IsLock 为true则直接返回;
        /// </summary>
        /// <param name="resSave"></param>
        /// <returns></returns>
        IEnumerator Save(ISaveRes resSave, Action<Exception> onError, Action loadingDoneCallBreak);
    }

    /// <summary>
    /// 在线程中保存游戏状态;
    /// </summary>
    public interface ISaveInThread
    {
        /// <summary>
        /// 保存游戏状态;
        /// 若在读取的过程中 IsStop 置为true,则停止保存;
        /// 若在进入读取时 IsLock 为true则直接返回;
        /// </summary>
        /// <param name="resSave"></param>
        /// <returns></returns>
        void Save(ISaveRes resSave, Action<Exception> onError, Action loadingDoneCallBreak);

    }

}
