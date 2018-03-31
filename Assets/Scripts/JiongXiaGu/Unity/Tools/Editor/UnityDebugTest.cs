using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    [TestFixture]
    public class UnityDebugTest
    {
        private const string Str = "";

        /// <summary>
        /// 检查 Debug.Log() 方法非主线程调用时是否会转给主线程;
        /// 结果:并不会;
        /// </summary>
        [Test]
        public void TestThread()
        {
            int currentThreadID = Thread.CurrentThread.ManagedThreadId;
            int testThreadId = -1;

            var logHandler = new LogHandler(() => testThreadId = Thread.CurrentThread.ManagedThreadId);
            using (UnityDebug.TemporaryLogHandler(logHandler))
            {
                Task.Run(() => Debug.Log(Str)).Wait();

                Assert.IsTrue(testThreadId != -1);
                Assert.AreNotEqual(currentThreadID, testThreadId);
            }
        }

        /// <summary>
        /// 检查在多个线程调用 Debug.Log() 方法时,是否阻塞的形式调用;
        /// 结果:是以阻塞的方式调用;
        /// </summary>
        [Test]
        public void TestThreadSafe()
        {
            var asyncLock = new object();

            var logHandler = new LogHandler(delegate ()
            {
                Monitor.Enter(asyncLock);
                Thread.Sleep(200);
                Monitor.Exit(asyncLock);
            });

            using (UnityDebug.TemporaryLogHandler(logHandler))
            {
                var task = Task.Run(() => Debug.Log(Str));

                logHandler.CallBack = delegate ()
                {
                    Assert.IsFalse(Monitor.IsEntered(asyncLock));
                };
                Debug.Log(Str);

                task.Wait();
            }
        }


        private class LogHandler : ILogHandler
        {
            public Action CallBack { get; set; }

            public LogHandler(Action callBack)
            {
                CallBack = callBack;
            }

            public void LogException(Exception exception, UnityEngine.Object context)
            {
                CallBack.Invoke();
            }

            public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                CallBack.Invoke();
            }
        }
    }
}
