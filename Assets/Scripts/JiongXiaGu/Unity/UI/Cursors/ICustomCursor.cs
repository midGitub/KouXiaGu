using System;

namespace JiongXiaGu.Unity.UI.Cursors
{
    /// <summary>
    /// 自定义鼠标样式;
    /// </summary>
    public interface ICustomCursor
    {
        /// <summary>
        /// 设置鼠标样式,并返回取消器;
        /// </summary>
        IDisposable Play();
    }
}
