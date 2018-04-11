using System;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    public class StaticCursor : ICustomCursor, IDisposable
    {
        [SerializeField]
        private Texture2D texture;
        [SerializeField]
        private StaticCursorConfig config;

        public Texture2D Texture => texture;
        public StaticCursorConfig Config => config;

        public StaticCursor(Texture2D texture, StaticCursorConfig config)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            this.texture = texture;
            this.config = config;
        }

        public IDisposable Play(ICursor cursor)
        {
            cursor.SetCursor(texture, config.Hotspot, config.CursorMode);
            return this;
        }

        void IDisposable.Dispose()
        {
            return;
        }
    }
}
