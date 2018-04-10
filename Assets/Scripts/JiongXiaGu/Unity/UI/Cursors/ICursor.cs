using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{
    /// <summary>
    /// 光标操作接口;
    /// </summary>
    public interface ICursor
    {
        void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode);
        void ResetCursor();
    }
}
