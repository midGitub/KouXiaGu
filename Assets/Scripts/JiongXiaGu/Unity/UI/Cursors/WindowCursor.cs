using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{
    /// <summary>
    /// 窗口光标;
    /// </summary>
    internal class WindowCursor : ICursor
    {
        public static readonly WindowCursor _default = new WindowCursor();
        public static WindowCursor Default => _default;

        private WindowCursor()
        {
        }

        public void SetCursor(CursorInfo cursor)
        {
            Cursor.SetCursor(cursor.Texture, cursor.Hotspot, cursor.CursorMode);
        }

        public void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
        {
            Cursor.SetCursor(texture, hotspot, cursorMode);
        }

        public void ResetCursor()
        {
            Cursor.SetCursor(null, default(Vector2), CursorMode.Auto);
        }
    }
}
