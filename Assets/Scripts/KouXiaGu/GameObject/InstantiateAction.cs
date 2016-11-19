using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 异步执行的状态;
    /// </summary>
    public interface IAsyncState<T>
    {
        bool IsDone { get; }
        T Result { get; }
    }


    /// <summary>
    /// 将实例化方法打包;
    /// </summary>
    public sealed class InstantiateAction<T> : IAsyncState<T>
        where T : UnityEngine.Component
    {
        private InstantiateAction()
        {
            Clear();
        }
        public InstantiateAction(T original) : this()
        {
            Set(original);
        }
        public InstantiateAction(T original, Vector3 position, Quaternion rotation) : this()
        {
            Set(original, position, rotation);
        }
        public InstantiateAction(T original, Transform parent, bool worldPositionStays) : this()
        {
            Set(original, parent, worldPositionStays);
        }
        public InstantiateAction(T original, Vector3 position, Quaternion rotation, Transform parent) : this()
        {
            Set(original, position, rotation, parent);
        }

        /// <summary>
        /// 实例化方法枚举;
        /// </summary>
        public enum MethodType
        {
            None,
            Original,
            Original_Position_Rotation,
            Original_Parent_WorldPositionStays,
            Original_Position_Rotation_Parent,
        }

        internal MethodType methodType { get; private set; }
        public bool IsDone { get; private set; }
        public T Result { get; private set; }

        public T Original { get; private set; }
        public Transform Parent { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public bool WorldPositionStays { get; private set; }

        /// <summary>
        /// 清除关键信息;
        /// </summary>
        public void Clear()
        {
            methodType = MethodType.None;
            IsDone = false;
            Result = default(T);
        }

        /// <summary>
        /// 设置到实例化方式;
        /// </summary>
        internal void Set(T original)
        {
            NotClearException();
            this.Original = original;
            this.methodType = MethodType.Original;
        }
        /// <summary>
        /// 设置到实例化方式;
        /// </summary>
        internal void Set(T original, Vector3 position, Quaternion rotation)
        {
            NotClearException();
            this.Original = original;
            this.Position = position;
            this.Rotation = rotation;
            this.methodType = MethodType.Original_Position_Rotation;
        }
        /// <summary>
        /// 设置到实例化方式;
        /// </summary>
        internal void Set(T original, Transform parent, bool worldPositionStays)
        {
            NotClearException();
            this.Original = original;
            this.Parent = parent;
            this.WorldPositionStays = worldPositionStays;
            this.methodType = MethodType.Original_Parent_WorldPositionStays;
        }
        /// <summary>
        /// 设置到实例化方式;
        /// </summary>
        internal void Set(T original, Vector3 position, Quaternion rotation, Transform parent)
        {
            NotClearException();
            this.Original = original;
            this.Position = position;
            this.Rotation = rotation;
            this.Parent = parent;
            this.methodType = MethodType.Original_Parent_WorldPositionStays;
        }
        /// <summary>
        /// 当完成时实例化后若不清除信息重复使用后返回错误;
        /// </summary>
        private void NotClearException()
        {
            if (this.IsDone)
                throw new AccessViolationException("重复使用!");
        }

        /// <summary>
        /// 设置到结果,并且根据结果设置到对应信息;
        /// </summary>
        public void SetResult(T Result)
        {
            this.methodType = MethodType.Original;
            this.Result = Result;
            SetResultThis();
        }

        /// <summary>
        /// 主线程调用 根据原型克隆一个物体到结果,并且返回;
        /// </summary>
        public T Instantiate()
        {
            this.Result = InstantiateThis();
            IsDone = true;
            return this.Result;
        }

        /// <summary>
        /// 将结果设置到对应格式;
        /// </summary>
        private void SetResultThis()
        {
            if (this.Result != null)
            {
                switch (this.methodType)
                {
                    case MethodType.None:
                    case MethodType.Original:
                        break;

                    case MethodType.Original_Parent_WorldPositionStays:
                        this.Result.Set(this.Parent, this.WorldPositionStays);
                        break;

                    case MethodType.Original_Position_Rotation:
                        this.Result.Set(this.Position, this.Rotation);
                        break;

                    case MethodType.Original_Position_Rotation_Parent:
                        this.Result.Set(this.Position, this.Rotation, this.Parent);
                        break;

                    default:
                        throw new Exception();
                }
            }
        }

        /// <summary>
        /// 根据类信息克隆物体并且返回;
        /// </summary>
        /// <returns></returns>
        private T InstantiateThis()
        {
            switch (this.methodType)
            {
                case MethodType.None:
                    return null;

                case MethodType.Original:
                    return Instantiate(this.Original);

                case MethodType.Original_Parent_WorldPositionStays:
                    return Instantiate(this.Original, this.Parent, this.WorldPositionStays);

                case MethodType.Original_Position_Rotation:
                    return Instantiate(this.Original, this.Position, this.Rotation);

                case MethodType.Original_Position_Rotation_Parent:
                    return Instantiate(this.Original, this.Position, this.Rotation, this.Parent);

                default:
                    throw new Exception();
            }
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

}
