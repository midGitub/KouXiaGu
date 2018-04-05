using JiongXiaGu.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    /// <summary>
    /// 动画鼠标的参数;
    /// </summary>
    [Serializable]
    public struct AnimatedCursorOptions
    {
        [SerializeField]
        private float switchInterval;
        [SerializeField]
        private bool isLoop;
        [SerializeField]
        private float loopInterval;

        /// <summary>
        /// 切换间隔,单位秒;
        /// </summary>
        public float SwitchInterval
        {
            get { return switchInterval; }
            set { switchInterval = value; }
        }

        /// <summary>
        /// 是否循环播放;
        /// </summary>
        public bool IsLoop
        {
            get { return isLoop; }
            set { isLoop = value; }
        }

        /// <summary>
        /// 循环间隔时间,单位秒;
        /// </summary>
        public float LoopInterval
        {
            get { return loopInterval; }
            set { loopInterval = value; }
        }

        public AnimatedCursorOptions(float switchInterval, bool isLoop, float loopInterval)
        {
            this.switchInterval = switchInterval;
            this.isLoop = isLoop;
            this.loopInterval = loopInterval;
        }
    }

    /// <summary>
    /// 动画鼠标样式;
    /// </summary>
    [Serializable]
    public class AnimatedCursor : IDynamicCursor
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

        public IEnumerator GetCoroutine()
        {
            return new Coroutine(this);
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
                    CustomCursor.SetCursor(style);
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
                CustomCursor.SetCursor(style);
            }
        }
    }
}
