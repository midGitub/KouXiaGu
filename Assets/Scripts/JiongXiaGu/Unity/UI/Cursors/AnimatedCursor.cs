using JiongXiaGu.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    /// <summary>
    /// 动画鼠标样式;
    /// </summary>
    [Serializable]
    public class AnimatedCursor : ICustomCursor
    {
        [SerializeField]
        private CursorInfo[] animation;
        [SerializeField]
        private AnimatedCursorOptions options;

        public IReadOnlyList<CursorInfo> Animation => animation;
        public AnimatedCursorOptions Options => options;

        public AnimatedCursor(IEnumerable<CursorInfo> animation, AnimatedCursorOptions options)
        {
            if (animation == null)
                throw new ArgumentNullException(nameof(animation));

            this.animation = animation.ToArray();
            this.options = options;
        }

        public IDisposable Play()
        {
            IEnumerator coroutine = new Coroutine(this);
            var canceler = UnityCoroutine.Start(coroutine);
            return canceler;
        }

        private struct Coroutine : IEnumerator
        {
            public AnimatedCursor Parent { get; private set; }
            public object Current { get; private set; }
            private int index;

            private IReadOnlyList<CursorInfo> animation => Parent.animation;
            private AnimatedCursorOptions options => Parent.options;

            public Coroutine(AnimatedCursor parent)
            {
                Parent = parent;
                Current = null;
                index = 0;
            }

            public bool MoveNext()
            {
                if (index < animation.Count)
                {
                    CursorInfo style = animation[index];
                    CursorController.SetCursor(style);
                    Current = new WaitForSecondsRealtime(options.SwitchInterval);
                    return true;
                }
                else
                {
                    if (options.IsLoop)
                    {
                        index = 0;
                        Current = new WaitForSecondsRealtime(options.LoopInterval);
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
                CursorInfo style = Parent.animation[index];
                CursorController.SetCursor(style);
            }
        }
    }
}
