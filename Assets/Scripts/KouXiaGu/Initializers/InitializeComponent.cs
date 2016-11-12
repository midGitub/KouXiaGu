using System.Collections.Generic;
using UnityEngine;
using System;

namespace KouXiaGu
{

    public interface IAppendComponent<FromCoroutine, FromThread>
    //where FromCoroutine : ICoroutineInitialize<T>
    //where FromThread : IThreadInitialize<T>
    {
        bool Add(FromThread item);
        bool Add(FromCoroutine item);
        bool Contains(FromThread item);
        bool Contains(FromCoroutine item);
        bool Remove(FromThread item);
        bool Remove(FromCoroutine item);
    }


    public class InitializeComponent<FromCoroutine, FromThread, T> : InitializeHelper<FromCoroutine, FromThread, T>,
        IAppendComponent<FromCoroutine, FromThread>
        where FromCoroutine : ICoroutineInitialize<T>
        where FromThread : IThreadInitialize<T>
    {
        protected InitializeComponent()
        {
            coroutineComponents = new HashSet<FromCoroutine>();
            threadComponents = new HashSet<FromThread>();
        }

        [Header("组件获取")]
        [SerializeField]
        private bool findFromGameObject = true;
        [SerializeField]
        private GameObject baseGameObject;

        private HashSet<FromCoroutine> coroutineComponents;
        private HashSet<FromThread> threadComponents;

        public bool FindFromGameObject
        {
            get { return findFromGameObject; }
            set { findFromGameObject = value; }
        }
        public GameObject BaseComponents
        {
            get { return baseGameObject; }
            set { baseGameObject = value; }
        }
        protected override IEnumerable<FromCoroutine> LoadInCoroutineComponents
        {
            get { return coroutineComponents; }
        }

        protected override IEnumerable<FromThread> LoadInThreadComponents
        {
            get { return threadComponents; }
        }


        public void Awake()
        {
            if (findFromGameObject)
            {
                coroutineComponents.UnionWith(BaseComponents.GetComponentsInChildren<FromCoroutine>());
                threadComponents.UnionWith(BaseComponents.GetComponentsInChildren<FromThread>());
            }
        }


        public bool Add(FromCoroutine item)
        {
            return coroutineComponents.Add(item);
        }
        public bool Remove(FromCoroutine item)
        {
            return coroutineComponents.Remove(item);
        }
        public bool Contains(FromCoroutine item)
        {
            return coroutineComponents.Contains(item);
        }

        public bool Add(FromThread item)
        {
            return threadComponents.Add(item);
        }
        public bool Remove(FromThread item)
        {
            return threadComponents.Remove(item);
        }
        public bool Contains(FromThread item)
        {
            return threadComponents.Contains(item);
        }

    }


}
