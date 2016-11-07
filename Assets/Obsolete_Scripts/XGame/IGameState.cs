using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{


    /// <summary>
    /// 调用次序;
    /// </summary>
    public interface ICallOrder
    {

        /// <summary>
        /// 调用次序;
        /// </summary>
        CallOrder CallOrder { get; }

    }

    /// <summary>
    /// 游戏初始资源读取;
    /// </summary>
    public interface IModLoad : ICallOrder
    {

        /// <summary>
        /// 读取模组资源;
        /// </summary>
        /// <returns>Untiy协程;</returns>
        IEnumerator Load(ModInfo modInfo);

    }


}
