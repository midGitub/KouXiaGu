using System;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    public class StaticCursor : ICustomCursor, IDisposable
    {
        [SerializeField]
        private Texture2D texture;
        [SerializeField]
        private Vector2 hotspot;
        [SerializeField]
        private CursorMode cursorMode;

        public Texture2D Texture => texture;
        public Vector2 Hotspot => hotspot;
        public CursorMode CursorMode => cursorMode;

        public StaticCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
        {
            this.texture = texture;
            this.hotspot = hotspot;
            this.cursorMode = cursorMode;
        }

        public IDisposable Play(ICursor cursor)
        {
            cursor.SetCursor(texture, hotspot, cursorMode);
            return this;
        }

        void IDisposable.Dispose()
        {
            return;
        }
    }
}
