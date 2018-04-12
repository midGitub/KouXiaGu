using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    [Serializable]
    public struct StaticCursorConfig : IEquatable<StaticCursorConfig>
    {
        public Vector2 Hotspot;
        public CursorMode CursorMode;

        public override bool Equals(object obj)
        {
            return obj is StaticCursorConfig && Equals((StaticCursorConfig)obj);
        }

        public bool Equals(StaticCursorConfig other)
        {
            return EqualityComparer<Vector2>.Default.Equals(Hotspot, other.Hotspot) &&
                   CursorMode == other.CursorMode;
        }

        public override int GetHashCode()
        {
            var hashCode = 767310404;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Hotspot);
            hashCode = hashCode * -1521134295 + CursorMode.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(StaticCursorConfig config1, StaticCursorConfig config2)
        {
            return config1.Equals(config2);
        }

        public static bool operator !=(StaticCursorConfig config1, StaticCursorConfig config2)
        {
            return !(config1 == config2);
        }
    }
}
