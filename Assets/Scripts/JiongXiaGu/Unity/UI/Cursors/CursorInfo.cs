using System;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{


    /// <summary>
    /// 光标样式;
    /// </summary>
    [Serializable]
    public struct CursorInfo
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

        public CursorInfo(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
        {
            this.texture = texture;
            this.hotspot = hotspot;
            this.cursorMode = cursorMode;
        }
    }
}
