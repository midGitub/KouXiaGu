using UnityEngine;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 提供序列化保存的二维向量;
    /// </summary>
    [ProtoContract]
    public struct ProtoVector2
    {
        public ProtoVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;

        public static implicit operator Vector2(ProtoVector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static implicit operator ProtoVector2(Vector2 v)
        {
            return new ProtoVector2(v.x, v.y);
        }

    }

}
