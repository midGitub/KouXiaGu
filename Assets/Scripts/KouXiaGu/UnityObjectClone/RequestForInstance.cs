using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    public interface IRequestForInstance<T>
    {
        T Original { get; }
        Transform Parent { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        bool WorldPositionStays { get; }

        T Instantiate();
        void SetResult(T clone);
    }


    /// <summary>
    /// 实例化方法枚举;
    /// </summary>
    public enum InstanceMethod
    {
        None,
        Original,
        Original_Position_Rotation,
        Original_Parent_WorldPositionStays,
        Original_Position_Rotation_Parent,
    }


    public struct RequestForInstance<T> : IRequestForInstance<T>
        where T : UnityEngine.Component
    {
        public RequestForInstance(T original) : this()
        {
            NulloriginalException(original);
            this.Original = original;
            this.methodType = InstanceMethod.Original;
        }
        public RequestForInstance(T original, Vector3 position, Quaternion rotation) : this()
        {
            NulloriginalException(original);
            this.Original = original;
            this.Position = position;
            this.Rotation = rotation;
            this.methodType = InstanceMethod.Original_Position_Rotation;
        }
        public RequestForInstance(T original, Transform parent, bool worldPositionStays) : this()
        {
            NulloriginalException(original);
            this.Original = original;
            this.Parent = parent;
            this.WorldPositionStays = worldPositionStays;
            this.methodType = InstanceMethod.Original_Parent_WorldPositionStays;
        }
        public RequestForInstance(T original, Vector3 position, Quaternion rotation, Transform parent) : this()
        {
            NulloriginalException(original);
            this.Original = original;
            this.Position = position;
            this.Rotation = rotation;
            this.Parent = parent;
            this.methodType = InstanceMethod.Original_Parent_WorldPositionStays;
        }

        internal InstanceMethod methodType { get; private set; }

        public T Original { get; private set; }
        public Transform Parent { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public bool WorldPositionStays { get; private set; }

        /// <summary>
        /// 若传入参数为null;
        /// </summary>
        private void NulloriginalException(T original)
        {
            if (original == null)
                throw new NullReferenceException();
        }

        /// <summary>
        /// 主线程调用 根据原型克隆一个物体到结果,并且返回;
        /// </summary>
        public T Instantiate()
        {
            if (Original == null)
                throw new NullReferenceException();

            switch (this.methodType)
            {
                case InstanceMethod.None:
                    return null;

                case InstanceMethod.Original:
                    return Instantiate(this.Original);

                case InstanceMethod.Original_Parent_WorldPositionStays:
                    return Instantiate(this.Original, this.Parent, this.WorldPositionStays);

                case InstanceMethod.Original_Position_Rotation:
                    return Instantiate(this.Original, this.Position, this.Rotation);

                case InstanceMethod.Original_Position_Rotation_Parent:
                    return Instantiate(this.Original, this.Position, this.Rotation, this.Parent);

                default:
                    throw new Exception();
            }
        }

        void IRequestForInstance<T>.SetResult(T clone)
        {
            return;
        }

        /// <summary>
        /// Unity方法实例化到游戏中;
        /// </summary>
        private T Instantiate(T original)
        {
            return UnityEngine.Object.Instantiate(original);
        }
        /// <summary>
        /// Unity方法实例化到游戏中;
        /// </summary>
        private T Instantiate(T original, Vector3 position, Quaternion rotation)
        {
            return UnityEngine.Object.Instantiate(original, position, rotation) as T;
        }
        /// <summary>
        /// Unity方法实例化到游戏中;
        /// </summary>
        private T Instantiate(T original, Transform parent, bool worldPositionStays)
        {
            return UnityEngine.Object.Instantiate(original, parent, worldPositionStays) as T;
        }
        /// <summary>
        /// Unity方法实例化到游戏中;
        /// </summary>
        private T Instantiate(T original, Vector3 position, Quaternion rotation, Transform parent)
        {
            return UnityEngine.Object.Instantiate(original, position, rotation, parent) as T;
        }

    }


    /// <summary>
    /// 异步执行的状态;
    /// </summary>
    public interface IAsyncState<T>
    {
        /// <summary>
        /// 是否已经完成了读取?
        /// </summary>
        bool IsDone { get; }
        /// <summary>
        /// 完整后的结果;
        /// </summary>
        T Result { get; }
    }


    /// <summary>
    /// 将实例化方法打包;
    /// </summary>
    public class RequestForInstanceAsync<T> : IAsyncState<T>, IRequestForInstance<T>
       where T : UnityEngine.Component
    {
        private RequestForInstanceAsync()
        {
            Clear();
        }
        public RequestForInstanceAsync(RequestForInstance<T> requestForInstance) : this()
        {
            this.requestForInstance = requestForInstance;
        }
        public RequestForInstanceAsync(T original) : this()
        {
            this.requestForInstance = new RequestForInstance<T>(original);
        }
        public RequestForInstanceAsync(T original, Vector3 position, Quaternion rotation) : this()
        {
            this.requestForInstance = new RequestForInstance<T>(original, position, rotation);
        }
        public RequestForInstanceAsync(T original, Transform parent, bool worldPositionStays) : this()
        {
            this.requestForInstance = new RequestForInstance<T>(original, parent, worldPositionStays);
        }
        public RequestForInstanceAsync(T original, Vector3 position, Quaternion rotation, Transform parent)
        {
            this.requestForInstance = new RequestForInstance<T>(original, position, rotation, parent);
        }

        private RequestForInstance<T> requestForInstance;


        public bool IsDone { get; private set; }
        public T Result { get; private set; }

        public T Original { get { return requestForInstance.Original; } }
        public Transform Parent { get { return requestForInstance.Parent; } }
        public Vector3 Position { get { return requestForInstance.Position; } }
        public Quaternion Rotation { get { return requestForInstance.Rotation; } }
        public bool WorldPositionStays { get { return requestForInstance.Original; } }

        /// <summary>
        /// 主线程调用 根据原型克隆一个物体到结果,并且返回;
        /// </summary>
        public T Instantiate()
        {
            this.Result = requestForInstance.Instantiate();
            IsDone = true;
            return this.Result;
        }

        /// <summary>
        /// 清除关键信息;
        /// </summary>
        public void Clear()
        {
            IsDone = false;
            Result = default(T);
        }

       void IRequestForInstance<T>.SetResult(T clone)
        {
            IsDone = true;
            Result = clone;
        }

        /// <summary>
        /// 若传入参数为null;
        /// </summary>
        private void NulloriginalException(T original)
        {
            if (original == null)
                throw new NullReferenceException();
        }

    }


}
