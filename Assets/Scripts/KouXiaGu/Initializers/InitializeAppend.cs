using System.Collections.Generic;
using UnityEngine;
using System;

namespace KouXiaGu
{

    public interface IAppendInitialize<FromCoroutine, FromThread>
    {
        bool Add(FromThread item);
        bool Add(FromCoroutine item);
        bool Contains(FromThread item);
        bool Contains(FromCoroutine item);
        bool Remove(FromThread item);
        bool Remove(FromCoroutine item);
    }

    [Serializable]
    public class InitializeAppend<FromCoroutine, FromThread, T> : InitializeHelper<FromCoroutine, FromThread, T>,
        IAppendInitialize<FromCoroutine, FromThread>
        where FromCoroutine : ICoroutineInitialize<T>
        where FromThread : IThreadInitialize<T>
    {
        protected InitializeAppend()
        {
            coroutineComponents = new HashSet<FromCoroutine>();
            threadComponents = new HashSet<FromThread>();
        }

        private HashSet<FromCoroutine> coroutineComponents;
        private HashSet<FromThread> threadComponents;

        protected override IEnumerable<FromCoroutine> LoadInCoroutineComponents
        {
            get { return coroutineComponents; }
        }

        protected override IEnumerable<FromThread> LoadInThreadComponents
        {
            get { return threadComponents; }
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
