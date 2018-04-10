using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{


    /// <summary>
    /// 动画鼠标的参数;
    /// </summary>
    [Serializable]
    public struct AnimatedCursorConfig : IEquatable<AnimatedCursorConfig>
    {
        /// <summary>
        /// 是否循环;
        /// </summary>
        public bool IsLoop;

        /// <summary>
        /// 速度,每秒播放的帧数;若为1则每秒一张,0.5则每两秒一张,2则每秒一秒两张;
        /// </summary>
        public float Speed;

        /// <summary>
        /// 统一的锚点;
        /// </summary>
        public Vector2 hotspot;

        /// <summary>
        /// 光标模式;
        /// </summary>
        public CursorMode cursorMode;

        public override bool Equals(object obj)
        {
            return obj is AnimatedCursorConfig && Equals((AnimatedCursorConfig)obj);
        }

        public bool Equals(AnimatedCursorConfig other)
        {
            return IsLoop == other.IsLoop &&
                   Speed == other.Speed &&
                   EqualityComparer<Vector2>.Default.Equals(hotspot, other.hotspot) &&
                   cursorMode == other.cursorMode;
        }

        public override int GetHashCode()
        {
            var hashCode = 1115475111;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + IsLoop.GetHashCode();
            hashCode = hashCode * -1521134295 + Speed.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(hotspot);
            hashCode = hashCode * -1521134295 + cursorMode.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(AnimatedCursorConfig config1, AnimatedCursorConfig config2)
        {
            return config1.Equals(config2);
        }

        public static bool operator !=(AnimatedCursorConfig config1, AnimatedCursorConfig config2)
        {
            return !(config1 == config2);
        }
    }
}
