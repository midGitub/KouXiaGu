using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 支持多线程的实例化物体类;
    /// 在异步实例化并非即时的,在Unity生命周期的FixUpdate内实例化物体;
    /// </summary>
    [Serializable]
    public class GameObjectConstructor
    {
        public void _GameObjectConstructor()
        {
            waitMainThreadCreateQueue = new ConcurrentQueue<AsyncGameObject>();
        }

        [SerializeField]
        private int maxEmptyClassCount;

        private ConcurrentQueue<AsyncGameObject> waitMainThreadCreateQueue;


        public void MainThreadFixedUpdate(int dequeueCount)
        {
            while (!waitMainThreadCreateQueue.IsEmpty && dequeueCount-- > 0)
            {
                AsyncGameObject asyncGameObject;
                if (waitMainThreadCreateQueue.TryDequeue(out asyncGameObject))
                {
                    asyncGameObject.Instantiate();
                }
            }
        }


        private UnityEngine.Object WaitInstantiate(AsyncGameObject asyncGameObject)
        {
            waitMainThreadCreateQueue.Enqueue(asyncGameObject);

            while (!asyncGameObject.IsDone)
            {
            }
            UnityEngine.Object gameObject = asyncGameObject.Result;
            AsyncGameObjectPool.GiveBack(asyncGameObject);
            return gameObject;
        }

        public UnityEngine.Object InstantiateWait(UnityEngine.Object original)
        {
            AsyncGameObject asyncGameObject = AsyncGameObjectPool.Give();
            asyncGameObject.Set(original);
            return WaitInstantiate(asyncGameObject);
        }

        public UnityEngine.Object InstantiateWait(UnityEngine.Object original, Transform parent)
        {
            AsyncGameObject asyncGameObject = AsyncGameObjectPool.Give();
            asyncGameObject.Set(original, parent);
            return WaitInstantiate(asyncGameObject);
        }

        public UnityEngine.Object InstantiateWait(UnityEngine.Object original, Vector3 position, Quaternion rotation)
        {
            AsyncGameObject asyncGameObject = AsyncGameObjectPool.Give();
            asyncGameObject.Set(original, position, rotation);
            return WaitInstantiate(asyncGameObject);
        }

        public UnityEngine.Object InstantiateWait(UnityEngine.Object original, Transform parent, bool worldPositionStays)
        {
            AsyncGameObject asyncGameObject = AsyncGameObjectPool.Give();
            asyncGameObject.Set(original, parent, worldPositionStays);
            return WaitInstantiate(asyncGameObject);
        }

        public UnityEngine.Object InstantiateWait(UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent)
        {
            AsyncGameObject asyncGameObject = AsyncGameObjectPool.Give();
            asyncGameObject.Set(original, position, rotation, parent);
            return WaitInstantiate(asyncGameObject);
        }

    }


    public class AsyncGameObject
    {
        public AsyncGameObject()
        {
            Clear();
        }
        public AsyncGameObject(UnityEngine.Object original) : this()
        {
            Set(original);
        }
        public AsyncGameObject(UnityEngine.Object original, Transform parent) : this()
        {
            Set(original, parent);
        }
        public AsyncGameObject(UnityEngine.Object original, Vector3 position, Quaternion rotation)
        {
            Set(original, position, rotation);
        }
        public AsyncGameObject(UnityEngine.Object original, Transform parent, bool worldPositionStays)
        {
            Set(original, parent, worldPositionStays);
        }
        public AsyncGameObject(UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent)
        {
            Set(original, position, rotation, parent);
        }

        public MethodType methodType { get; private set; }
        public bool IsDone { get; private set; }
        public UnityEngine.Object Result { get; private set; }

        public UnityEngine.Object Original { get; private set; }
        public Transform Parent { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public bool WorldPositionStays { get; private set; }

        public void Clear()
        {
            methodType = MethodType.None;
            IsDone = false;
            Result = null;
        }

        public void Set(UnityEngine.Object original)
        {
            this.methodType = MethodType.Original;
            this.Original = original;
        }

        public void Set(UnityEngine.Object original, Transform parent)
        {
            this.methodType = MethodType.Original_Parent;
            this.Original = original;
            this.Parent = parent;
        }

        public void Set(UnityEngine.Object original, Vector3 position, Quaternion rotation)
        {
            this.methodType = MethodType.Original_Position_Rotation;
            this.Original = original;
            this.Position = position;
            this.Rotation = rotation;
        }

        public void Set(UnityEngine.Object original, Transform parent, bool worldPositionStays)
        {
            this.methodType = MethodType.Original_Parent_WorldPositionStays;
            this.Original = original;
            this.Parent = parent;
            this.WorldPositionStays = worldPositionStays;
        }

        public void Set(UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent)
        {
            this.methodType = MethodType.Original_Parent_WorldPositionStays;
            this.Original = original;
            this.Position = position;
            this.Rotation = rotation;
            this.Parent = parent;
        }

        public void Instantiate()
        {
            this.Result = InstantiateThis();
            IsDone = true;
        }

        private UnityEngine.Object InstantiateThis()
        {
            switch (this.methodType)
            {
                case MethodType.None:
                    return null;

                case MethodType.Original:
                    return Instantiate(this.Original);

                case MethodType.Original_Parent:
                    return Instantiate(this.Original, this.Parent);

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

        public UnityEngine.Object Instantiate(UnityEngine.Object original)
        {
            return UnityEngine.Object.Instantiate(original);
        }

        public UnityEngine.Object Instantiate(UnityEngine.Object original, Transform parent)
        {
            return UnityEngine.Object.Instantiate(original, parent);
        }

        public UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation)
        {
            return UnityEngine.Object.Instantiate(original, position, rotation);
        }

        public UnityEngine.Object Instantiate(UnityEngine.Object original, Transform parent, bool worldPositionStays)
        {
            return UnityEngine.Object.Instantiate(original, parent, worldPositionStays);
        }

        public UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent)
        {
            return UnityEngine.Object.Instantiate(original, position, rotation, parent);
        }

        public enum MethodType
        {
            None,
            Original,
            Original_Parent,
            Original_Position_Rotation,
            Original_Parent_WorldPositionStays,
            Original_Position_Rotation_Parent,
        }
    }


    public static class AsyncGameObjectPool
    {
        static AsyncGameObjectPool()
        {
            emptyClassQueue = new Queue<AsyncGameObject>(defaultMaxCount);
            MaxCount = defaultMaxCount;
        }

        private const int defaultMaxCount = 100;
        private static readonly Queue<AsyncGameObject> emptyClassQueue;

        public static int MaxCount { get; set; }


        public static AsyncGameObject Give()
        {
            lock (emptyClassQueue)
            {
                if (emptyClassQueue.Count > 0)
                {
                    return emptyClassQueue.Dequeue();
                }
                else
                {
                    return new AsyncGameObject();
                }
            }
        }

        public static void GiveBack(AsyncGameObject asyncGameObject)
        {
            lock (emptyClassQueue)
            {
                asyncGameObject.Clear();
                if (emptyClassQueue.Count <= MaxCount)
                {
                    emptyClassQueue.Enqueue(asyncGameObject);
                }
            }
        }

    }


}
