using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{

    /// <summary>
    /// 预览启用关闭接口;游戏物体加入游戏和从游戏中移除的接口;
    /// </summary>
    public interface IXBehaviour
    {

        /// <summary>
        /// 设置为预览状态;
        /// </summary>
        void XOnEnable();

        /// <summary>
        /// 在实例化后取消设置为预览模式调用;
        /// </summary>
        void XOnDisable();

    }


}
