using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.UI
{

    /// <summary>
    /// 鼠标样式;
    /// </summary>
    public enum CursorType
    {
        /// <summary>
        /// 默认;
        /// </summary>
        Default,

        /// <summary>
        /// 移动;
        /// </summary>
        Move,

        /// <summary>
        /// 垂直调整;
        /// </summary>
        S_resize,

        /// <summary>
        /// 水平调整;
        /// </summary>
        W_resize,

        /// <summary>
        /// 文本选择;
        /// </summary>
        Text,

        /// <summary>
        /// 忙,等待;
        /// </summary>
        Wait,
    }
}
