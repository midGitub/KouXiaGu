using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏资源接口;
    /// </summary>
    public interface IResLoad
    {
        /// <summary>
        /// 需要从这个存档信息初始化;
        /// </summary>
        ArchiveExpand Archive { get;}

        /// <summary>
        /// 在初始化时,游戏需要加载的资源目录;
        /// </summary>
        IEnumerable<string> ResPath { get; }

        /// <summary>
        /// 无法开始游戏时调用;
        /// </summary>
        void OnError(Exception error);

    }

    /// <summary>
    /// 保存游戏状态;
    /// </summary>
    public interface IResSave
    {
        /// <summary>
        /// 需要从这个存档信息初始化;
        /// </summary>
        ArchiveExpand Archive { get; }

        /// <summary>
        /// 无法保存游戏时调用;
        /// </summary>
        void OnError(Exception error);
    }

    /// <summary>
    /// 在协程中准备游戏资源;
    /// </summary>
    public interface ILoadGame
    {
        /// <summary>
        /// 是否终止这个协程;
        /// </summary>
        bool IsStop { get; set; }

        /// <summary>
        /// 开始读取游戏;若在读取的过程中 IsStop 置为true,则停止加载,并且卸载已加载的资源;
        /// 当 IsStop 为false时开始加载游戏, 为true时直接返回;
        /// </summary>
        /// <param name="resLoad"></param>
        /// <returns>任意返回值</returns>
        IEnumerator Load(IResLoad resLoad);

        /// <summary>
        /// 卸载已经加载的游戏资源;
        /// </summary>
        void Unload();
    }

    /// <summary>
    /// 多线程准备游戏资源(在线程池内初始化);
    /// </summary>
    public interface ILoadGameThread
    {
        /// <summary>
        /// 是否终止这个线程;
        /// </summary>
        bool IsStop { get; set; }

        /// <summary>
        /// 开始读取游戏;若在读取的过程中 IsStop 置为true,则停止加载,并且卸载已加载的资源;
        /// 当 IsStop 为false时开始加载游戏, 为true时直接返回;
        /// </summary>
        /// <param name="resLoad"></param>
        void Load(IResLoad resLoad);

        /// <summary>
        /// 卸载已经加载的游戏资源;
        /// </summary>
        void Unload();
    }

    /// <summary>
    /// 在协程中保存游戏状态;
    /// </summary>
    public interface ISaveGame
    {
        /// <summary>
        /// 是否终止这个线程;
        /// </summary>
        bool IsStop { get; set; }

        /// <summary>
        /// 保存游戏状态;若在读取的过程中 IsStop 置为true,则停止保存;
        /// </summary>
        /// <param name="resSave"></param>
        /// <returns></returns>
        IEnumerator Save(IResSave resSave);
    }

    /// <summary>
    /// 在线程中保存游戏状态;
    /// </summary>
    public interface ISaveGameThread
    {
        /// <summary>
        /// 是否终止这个线程;
        /// </summary>
        bool IsStop { get; set; }

        /// <summary>
        /// 保存游戏状态;若在读取的过程中 IsStop 置为true,则停止保存;
        /// </summary>
        /// <param name="resSave"></param>
        /// <returns></returns>
        void Save(IResSave resSave);
    }

}
