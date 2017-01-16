using UnityEngine;
using ProtoBuf;
using System.Xml.Serialization;

namespace KouXiaGu.Serialization
{

    /// <summary>
    /// 提供序列化保存的二维向量;
    /// </summary>
    [ProtoContract, XmlType("Vector2")]
    public struct ProtoVector2
    {
        public ProtoVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        [ProtoMember(1), XmlAttribute("x")]
        public float x;
        [ProtoMember(2), XmlAttribute("y")]
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
