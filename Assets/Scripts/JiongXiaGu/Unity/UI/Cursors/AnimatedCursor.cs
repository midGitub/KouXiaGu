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
        private CursorInfo[] cursorInfos;
        [SerializeField]
        private bool isLoop;
        [SerializeField]
        private AnimatedCursorTime[] animations;

        public IReadOnlyList<CursorInfo> CursorInfos => cursorInfos;
        public bool IsLoop => isLoop;
        public IReadOnlyList<AnimatedCursorTime> Animations => animations;

        public AnimatedCursor(IEnumerable<CursorInfo> cursorInfos, IEnumerable<AnimatedCursorTime> animations, bool isLoop)
        {
            if (cursorInfos == null)
                throw new ArgumentNullException(nameof(cursorInfos));
            if (animations == null)
                throw new ArgumentNullException(nameof(animations));

            this.cursorInfos = cursorInfos.ToArray();
            this.animations = animations.ToArray();
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

            /// <summary>
            /// 指向下一个播放的光标下标;
            /// </summary>
            private int animationIndex;

            private IReadOnlyList<CursorInfo> cursorInfos => Parent.cursorInfos;
            private bool isLoop => Parent.isLoop;
            private IReadOnlyList<AnimatedCursorTime> animations => Parent.animations;

            public Coroutine(AnimatedCursor parent)
            {
                Parent = parent;
                Current = null;
                animationIndex = 0;
            }

            public bool MoveNext()
            {
                if (animationIndex < 0)
                {
                    return false;
                }
                else
                {
                    var animation = animations[animationIndex];

                    if (animation.Index < 0 || animation.Index > cursorInfos.Count)
                    {
                        CursorController.ResetCursor();
                    }
                    else
                    {
                        CursorInfo cursorInfo = cursorInfos[animation.Index];
                        CursorController.SetCursor(cursorInfo);
                    }

                    if (++animationIndex >= animations.Count)
                    {
                        if (isLoop)
                            animationIndex = 0;
                        else
                            animationIndex = -1;
                    }

                    Current = new WaitForSecondsRealtime(animation.SecondsToWait);
                    return true;
                }
            }

            public void Reset()
            {
                animationIndex = 0;
                Current = null;
            }
        }
    }
}
