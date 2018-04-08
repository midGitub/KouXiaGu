using System;
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
}
