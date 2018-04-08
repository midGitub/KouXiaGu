using System;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{


    /// <summary>
    /// 动画鼠标的参数;
    /// </summary>
    [Serializable]
    public struct AnimatedCursorInfo
    {
        public bool IsLoop;
        public AnimatedCursorTime[] Animation;
    }
}
