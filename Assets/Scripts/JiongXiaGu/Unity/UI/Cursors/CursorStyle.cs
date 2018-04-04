using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.UI.Cursors
{

    /// <summary>
    /// 鼠标样式;
    /// </summary>
    public interface IDynamicCursorStyle
    {
        /// <summary>
        /// 获取到鼠标样式执行的协程;
        /// </summary>
        IEnumerator GetCoroutine();
    }

    /// <summary>
    /// 动画鼠标样式;
    /// </summary>
    [Serializable]
    public struct AnimatedCursor : IDynamicCursorStyle
    {
        [SerializeField]
        private CursorStyle[] styles;

        /// <summary>
        /// 切换间隔,单位秒;
        /// </summary>
        [SerializeField]
        private float switchInterval;

        /// <summary>
        /// 是否循环播放;
        /// </summary>
        [SerializeField]
        private bool isLoop;

        /// <summary>
        /// 循环间隔时间,单位秒;
        /// </summary>
        [SerializeField]
        private float loopInterval;

        public IReadOnlyList<CursorStyle> Styles => styles;
        public float SwitchInterval => switchInterval;
        public bool IsLoop => isLoop;
        public float LoopInterval => loopInterval;

        public AnimatedCursor(IEnumerable<CursorStyle> styles, float switchInterval, bool isLoop, float loopInterval)
        {
            if (styles == null)
                throw new ArgumentNullException(nameof(styles));

            this.styles = styles.ToArray();
            this.switchInterval = switchInterval;
            this.isLoop = isLoop;
            this.loopInterval = loopInterval;
        }

        public IEnumerator GetCoroutine()
        {
            if (styles == null)
            {
                return EmptyCollection<object>.Default.GetEnumerator();
            }
            else
            {
                return new Coroutine(this);
            }
        }

        private struct Coroutine : IEnumerator
        {
            public AnimatedCursor Parent { get; private set; }
            public object Current { get; private set; }
            private int index;

            public Coroutine(AnimatedCursor parent)
            {
                Parent = parent;
                Current = null;
                index = 0;
            }

            public bool MoveNext()
            {
                var styles = Parent.styles;

                if (index < styles.Length)
                {
                    CursorStyle style = styles[index];
                    CustomCursor.SetCursor(style);
                    Current = new WaitForSecondsRealtime(Parent.switchInterval);
                    return true;
                }
                else
                {
                    if (Parent.isLoop)
                    {
                        index = 0;
                        Current = new WaitForSecondsRealtime(Parent.loopInterval);
                        return true;
                    }
                    else
                    {
                        Current = null;
                        return false;
                    }
                }
            }

            public void Reset()
            {
                index = 0;
                Current = null;
                CursorStyle style = Parent.styles[index];
                CustomCursor.SetCursor(style);
            }
        }
    }


    /// <summary>
    /// 光标样式;
    /// </summary>
    [Serializable]
    public struct CursorStyle
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

        public CursorStyle(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
        {
            this.texture = texture;
            this.hotspot = hotspot;
            this.cursorMode = cursorMode;
        }
    }
}
