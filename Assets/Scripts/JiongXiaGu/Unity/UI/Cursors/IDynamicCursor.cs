using System.Collections;

namespace JiongXiaGu.Unity.UI.Cursors
{


    /// <summary>
    /// 动态鼠标样式;
    /// </summary>
    public interface IDynamicCursor
    {
        /// <summary>
        /// 获取到鼠标样式执行的协程;
        /// </summary>
        IEnumerator GetCoroutine();
    }
}
