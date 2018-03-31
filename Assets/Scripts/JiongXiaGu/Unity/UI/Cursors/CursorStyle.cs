using System.Collections;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    public interface ICursorStyle
    {
        /// <summary>
        /// 获取到鼠标样式执行的协程;
        /// </summary>
        IEnumerator GetCoroutine();
    }

    /// <summary>
    /// 光标样式;
    /// </summary>
    public struct CursorStyle
    {
        public Texture2D Texture { get; set; }
        public Vector2 Hotspot { get; set; }
        public CursorMode CursorMode { get; set; }

        public CursorStyle(Texture2D texture, Vector2 hotspot, CursorMode cursorMode) : this()
        {
            Texture = texture;
            Hotspot = hotspot;
            CursorMode = cursorMode;
        }
    }
}
