using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace XGame
{

    /// <summary>
    /// 使用ProtoBuf保存Vector3结构的类;
    /// </summary>
    [ProtoContract]
    public struct ProtoBuf_Vector3
    {

        public ProtoBuf_Vector3(Vector3 vector3)
        {
            this.x = vector3.x;
            this.y = vector3.y;
            this.z = vector3.z;
        }

        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;

        /// <summary>
        /// 根据四舍五入进行转换;
        /// </summary>
        public static implicit operator ProtoBuf_Vector3(Vector3 vector3)
        {
            return new ProtoBuf_Vector3(vector3);
        }

        /// <summary>
        /// 根据四舍五入进行转换;
        /// </summary>
        public static implicit operator Vector3(ProtoBuf_Vector3 vector3)
        {
            return new Vector3(vector3.x, vector3.y, vector3.z);
        }

    }

}
