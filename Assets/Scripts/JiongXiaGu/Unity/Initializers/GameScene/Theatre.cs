using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{

    public interface ITheatreHandle
    {
        /// <summary>
        /// 每次更新时调用;
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 游戏内时间,每天更新;
        /// </summary>
        void OnDayUpdate();
    }

    /// <summary>
    /// 游戏场景控制器;
    /// </summary>
    public class Theatre
    {

        /// <summary>
        /// 在Unity线程调用,用于游戏场景更新;
        /// </summary>
        internal void OnUpdate()
        {
            UnityThread.ThrowIfNotUnityThread();

            throw new NotImplementedException();
        }


    }
}
