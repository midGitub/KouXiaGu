using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Concurrent
{

    public static partial class Request
    {

        public static void Instantiate(this ThreadRequest instance, UnityEngine.Object original)
        {
            RequestForInstance instantiate = new RequestForInstance(original);
            instance.AddQueue(instantiate);
        }

        public static void Instantiate(this ThreadRequest instance, UnityEngine.Object original, Vector3 position, Quaternion rotation)
        {
            RequestForInstance instantiate = new RequestForInstance(original, position, rotation);
            instance.AddQueue(instantiate);
        }

        public static void Instantiate(this ThreadRequest instance, UnityEngine.Object original, Transform parent, bool worldPositionStays)
        {
            RequestForInstance instantiate = new RequestForInstance(original, parent, worldPositionStays);
            instance.AddQueue(instantiate);
        }

        public static void Instantiate<T>(this ThreadRequest instance, UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent)
        {
            RequestForInstance instantiate = new RequestForInstance(original, position, rotation, parent);
            instance.AddQueue(instantiate);
        }

    }



    public struct RequestForInstance : IRequest
    {
        public RequestForInstance(UnityEngine.Object original) : this()
        {
            NulloriginalException(original);
            this.original = original;
            this.methodType = InstanceMethod.Original;
        }
        public RequestForInstance(UnityEngine.Object original, Vector3 position, Quaternion rotation) : this()
        {
            NulloriginalException(original);
            this.original = original;
            this.position = position;
            this.rotation = rotation;
            this.methodType = InstanceMethod.Original_Position_Rotation;
        }
        public RequestForInstance(UnityEngine.Object original, Transform parent, bool worldPositionStays) : this()
        {
            NulloriginalException(original);
            this.original = original;
            this.parent = parent;
            this.worldPositionStays = worldPositionStays;
            this.methodType = InstanceMethod.Original_Parent_WorldPositionStays;
        }
        public RequestForInstance(UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent) : this()
        {
            NulloriginalException(original);
            this.original = original;
            this.position = position;
            this.rotation = rotation;
            this.parent = parent;
            this.methodType = InstanceMethod.Original_Parent_WorldPositionStays;
        }

        InstanceMethod methodType;
        UnityEngine.Object original;
        Transform parent;
        Vector3 position;
        Quaternion rotation;
        bool worldPositionStays;

        bool IRequest.OnQueue { get; set; }

        void IRequest.Execute()
        {
            Instantiate();
        }

        /// <summary>
        /// 若传入参数为null;
        /// </summary>
        void NulloriginalException(UnityEngine.Object original)
        {
            if (original == null)
                throw new NullReferenceException();
        }

        /// <summary>
        /// 主线程调用 根据原型克隆一个物体到结果,并且返回;
        /// </summary>
        UnityEngine.Object Instantiate()
        {
            if (original == null)
                throw new NullReferenceException();

            switch (this.methodType)
            {
                case InstanceMethod.None:
                    return null;

                case InstanceMethod.Original:
                    return UnityEngine.Object.Instantiate(this.original);

                case InstanceMethod.Original_Parent_WorldPositionStays:
                    return UnityEngine.Object.Instantiate(this.original, this.parent, this.worldPositionStays);

                case InstanceMethod.Original_Position_Rotation:
                    return UnityEngine.Object.Instantiate(this.original, this.position, this.rotation);

                case InstanceMethod.Original_Position_Rotation_Parent:
                    return UnityEngine.Object.Instantiate(this.original, this.position, this.rotation, this.parent);

                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// 实例化方法枚举;
        /// </summary>
        enum InstanceMethod
        {
            None,
            Original,
            Original_Position_Rotation,
            Original_Parent_WorldPositionStays,
            Original_Position_Rotation_Parent,
        }

    }


}
