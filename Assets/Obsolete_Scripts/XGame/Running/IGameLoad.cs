using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame.Running
{


    /// <summary>
    /// 游戏读取销毁接口;
    /// </summary>
    public interface IGameLoad : ICallOrder
    {
        /// <summary>
        /// 开始游戏初始化(仅在开始游戏之前调用;);
        /// 若存档开始时存在IGameArchive接口,则调用OnLoad,不调用这个方法;
        /// </summary>
        /// <returns>返回值为Unity协程类</returns>
        IEnumerator OnStart();

        /// <summary>
        /// 退出游戏初始化(仅开始新游戏,或者退回主菜单调用),不在完全退出游戏调用;
        /// </summary>
        /// <returns>Unity协程类</returns>
        IEnumerator OnClear();

    }

    /// <summary>
    /// 保存和恢复状态(存档,读档)接口;
    /// </summary>
    public interface IGameArchive : IGameLoad
    {

        /// <summary>
        /// 读取游戏存档时调用(仅静态/单例类使用);通过协程初始化;
        /// </summary>
        /// <param name="info">存档的信息;</param>
        /// <param name="data">存档数据;</param>
        /// <returns>返回值为Unity协程类;</returns>
        IEnumerator OnLoad(GameSaveInfo info, GameSaveData data);

        /// <summary>
        /// 保存游戏存档时调用;通过协程保存;
        /// </summary>
        /// <param name="info">存档的信息;</param>
        /// <param name="data">存档数据;</param>
        /// <returns>返回值为Unity协程类;</returns>
        IEnumerator OnSave(GameSaveInfo info, GameSaveData data);

    }

}
