using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame.Running
{

    /// <summary>
    /// GameObject状态保存复原接口;
    /// </summary>
    public interface IGameObjectState
    {

        /// <summary>
        /// 保存当前状态;
        /// </summary>
        /// <param name="list"></param>
        void Save(GameObjectState state);

        /// <summary>
        /// 恢复到状态;
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        void Load(GameObjectState state);

    }


}
