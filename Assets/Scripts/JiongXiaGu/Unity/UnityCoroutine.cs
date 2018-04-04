using System;
using System.Collections;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 提供Unity协程执行方法;
    /// </summary>
    public class UnityCoroutine
    {
        private static UnityCoroutineDispatcher dispatcher;

        /// <summary>
        /// 开始协程;
        /// </summary>
        public static IDisposable Start(IEnumerator routine)
        {
            if (routine == null)
                throw new ArgumentNullException(nameof(routine));

            var coroutine = GetDispatcher().StartCoroutine(routine);
            return new CoroutineCanceler(coroutine);
        }

        /// <summary>
        /// 停止所有协程;
        /// </summary>
        public static void StopAll()
        {
            GetDispatcher().StopAllCoroutines();
        }

        /// <summary>
        /// 获取到协程处理类;
        /// </summary>
        private static UnityCoroutineDispatcher GetDispatcher()
        {
            if (dispatcher == null)
            {
                var dispatcherGameObject = new GameObject(nameof(UnityCoroutineDispatcher), typeof(UnityCoroutineDispatcher));
                dispatcherGameObject.hideFlags = HideFlags.HideAndDontSave;
                GameObject.DontDestroyOnLoad(dispatcherGameObject);
                UnityCoroutineDispatcher dispatcher = dispatcherGameObject.GetComponent<UnityCoroutineDispatcher>();
                UnityCoroutine.dispatcher = dispatcher;
                return dispatcher;
            }
            else
            {
                return dispatcher;
            }
        }

        private class UnityCoroutineDispatcher : MonoBehaviour
        {
            private UnityCoroutineDispatcher()
            {
            }
        }

        private class CoroutineCanceler : IDisposable
        {
            private Coroutine coroutine;

            public CoroutineCanceler(Coroutine coroutine)
            {
                this.coroutine = coroutine;
            }

            public void Dispose()
            {
                if (coroutine != null)
                {
                    dispatcher.StopCoroutine(coroutine);
                    coroutine = null;
                }
            }
        }
    }
}
