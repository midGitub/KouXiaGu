using JiongXiaGu.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    /// <summary>
    /// 动画鼠标样式,在Unity协程内执行;
    /// </summary>
    [Serializable]
    public class AnimatedCursor : ICustomCursor
    {
        [SerializeField]
        private AnimatedCursorConfig config;
        [SerializeField]
        private Texture2D[] textures;

        public AnimatedCursorConfig Config => config;
        public IReadOnlyList<Texture2D> Textures => textures;

        public AnimatedCursor(AnimatedCursorConfig config, IEnumerable<Texture2D> textures)
        {
            this.textures = textures.ToArray();
            this.config = config;
        }

        public IDisposable Play(ICursor cursor)
        {
            if (cursor == null)
                throw new NotImplementedException();

            IEnumerator coroutine = new Coroutine(this, cursor);
            var canceler = UnityCoroutine.Start(coroutine);
            return canceler;
        }

        private struct Coroutine : IEnumerator
        {
            public AnimatedCursor Parent { get; private set; }
            public ICursor Cursor { get; private set; }
            public object Current { get; private set; }

            /// <summary>
            /// 指向下一个播放的光标下标;
            /// </summary>
            public int TextureIndex { get; private set; }
            public float SecondsToWait { get; private set; }

            public IReadOnlyList<Texture2D> Textures => Parent.textures;
            public AnimatedCursorConfig Config => Parent.config;

            public Coroutine(AnimatedCursor parent, ICursor cursor)
            {
                Parent = parent;
                Cursor = cursor;
                Current = null;
                TextureIndex = 0;
                SecondsToWait = 1f / parent.config.Speed;
            }

            public bool MoveNext()
            {
                if (TextureIndex < 0)
                {
                    return false;
                }
                else
                {
                    var texture = Textures[TextureIndex];
                    Cursor.SetCursor(texture, Config.hotspot, Config.cursorMode);

                    if (++TextureIndex >= Textures.Count)
                    {
                        if (Config.IsLoop)
                        {
                            TextureIndex = 0;
                        }
                        else
                        {
                            TextureIndex = -1;
                            return false;
                        }
                    }

                    Current = new WaitForSecondsRealtime(SecondsToWait);
                    return true;
                }
            }

            public void Reset()
            {
                throw new InvalidOperationException();
            }
        }
    }
}
