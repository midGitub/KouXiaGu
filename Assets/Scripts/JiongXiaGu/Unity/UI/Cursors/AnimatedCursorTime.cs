using System;

namespace JiongXiaGu.Unity.UI.Cursors
{
    [Serializable]
    public struct AnimatedCursorTime
    {
        /// <summary>
        /// 目标图片的下标;
        /// </summary>
        public int Index;

        /// <summary>
        /// 等待秒数;
        /// </summary>
        public float SecondsToWait;
    }
}
