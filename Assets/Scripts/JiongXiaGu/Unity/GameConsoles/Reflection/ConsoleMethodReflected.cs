using System;
using System.Reflection;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 表示通过反射获得的控制台方法;
    /// </summary>
    public struct ConsoleMethodReflected
    {
        /// <summary>
        /// 是否完成?
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// 是否失败?
        /// </summary>
        public bool IsFaulted { get; private set; }

        /// <summary>
        /// 失败的原因;
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 对应的控制台方法,若失败则为null;
        /// </summary>
        public ConsoleMethod ConsoleMethod { get; private set; }

        /// <summary>
        /// 对应进行反射的方法;
        /// </summary>
        public MethodInfo MethodInfo { get; private set; }

        public ConsoleMethodReflected(ConsoleMethod consoleMethod, MethodInfo methodInfo)
        {
            if (consoleMethod == null)
                throw new ArgumentNullException(nameof(consoleMethod));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));

            ConsoleMethod = consoleMethod;
            MethodInfo = methodInfo;
            IsCompleted = true;
            IsFaulted = false;
            Exception = null;
        }

        public ConsoleMethodReflected(Exception exception, MethodInfo methodInfo)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));

            Exception = exception;
            MethodInfo = methodInfo;
            IsCompleted = true;
            IsFaulted = true;
            ConsoleMethod = null;
        }

        public override string ToString()
        {
            if (IsCompleted)
            {
                if (!IsFaulted)
                {
                    return string.Format("[Completed:Name:{0}]", ConsoleMethod.Name);
                }
                else
                {
                    return string.Format("[Faulted:DeclaringType:{0}, MethodInfo:{1}, Exception:{2}]", MethodInfo.DeclaringType.Name, MethodInfo.Name, Exception);
                }
            }
            return "[NotCompleted]";
        }
    }
}
