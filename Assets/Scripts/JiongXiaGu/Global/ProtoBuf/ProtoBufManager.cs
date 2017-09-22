using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// 添加自定义序列化方式;
    /// </summary>
    [DisallowMultipleComponent]
    class ProtoBufManager : MonoBehaviour
    {
        static ProtoBufManager()
        {
            Init();
        }

        ProtoBufManager()
        {
        }

        static void Init()
        {
            //使用这个方法,或者下面的方法;
            //RuntimeTypeModel.Default.Add(typeof(Vector3), false).SetSurrogate(typeof(SerializableVector3));

            RuntimeTypeModel.Default.Add(typeof(Vector3), false).Add("x", "y", "z");
        }
    }

    [Obsolete]
    [ProtoContract]
    struct SerializableVector3
    {
        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;

        public static implicit operator Vector3(SerializableVector3 item)
        {
            return new Vector3(item.x, item.y, item.z);
        }

        public static implicit operator SerializableVector3(Vector3 item)
        {
            return new SerializableVector3(item.x, item.y, item.z);
        }
    }
}
