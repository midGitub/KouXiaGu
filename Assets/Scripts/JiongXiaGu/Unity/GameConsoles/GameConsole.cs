using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 游戏控制台(方法线程安全);
    /// </summary>
    public static class GameConsole
    {
        /// <summary>
        /// 控制台方法合集;
        /// </summary>
        public static ConsoleMethodSchema MethodSchema { get; internal set; }
        public static ConsoleItemRecorder Recorder { get; internal set; }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Write(string message)
        {
            Recorder.Write(message);
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Write(object message)
        {
            Write(message.ToString());
        }

        /// <summary>
        /// 记录标准条目;
        /// </summary>
        public static void Write(string format, params object[] args)
        {
            string message = string.Format(format, args);
            Write(message);
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void WriteSuccessful(string message)
        {
            Recorder.WriteSuccessful(message);
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void WriteSuccessful(object message)
        {
            WriteSuccessful(message.ToString());
        }

        /// <summary>
        /// 记录成功条目;
        /// </summary>
        public static void WriteSuccessful(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteSuccessful(message);
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void WriteWarning(string message)
        {
            Recorder.WriteWarning(message);
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void WriteWarning(object message)
        {
            WriteWarning(message.ToString());
        }

        /// <summary>
        /// 记录警告条目;
        /// </summary>
        public static void WriteWarning(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteWarning(message);
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void WriteError(string message)
        {
            Recorder.WriteError(message);
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void WriteError(object message)
        {
            WriteError(message.ToString());
        }

        /// <summary>
        /// 记录异常条目;
        /// </summary>
        public static void WriteError(string format, params object[] args)
        {
            string message = string.Format(format, args);
            WriteError(message);
        }

        /// <summary>
        /// 输入的方法命令间隔符;
        /// </summary>
        private static readonly char[] methodSeparator = new char[] { ' ' };

        /// <summary>
        /// 执行指定控制台方法;
        /// </summary>
        public static void Do(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowMethodStringIncorrect(message);
            }

            string[] valueArray = message.Split(methodSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (valueArray.Length == 0)
            {
                ThrowMethodStringIncorrect(message);
            }
            else if (valueArray.Length == 1)
            {
                var methodName = valueArray[0];
                ConsoleMethod consoleMethod;
                if (MethodSchema.TryGetMethod(methodName, 0, out consoleMethod))
                {
                    Recorder.WriteMethod(message);
                    consoleMethod.Invoke(null);
                }
                else
                {
                    ThrowMethodNotFound(methodName, 0);
                }
            }
            else if (valueArray.Length > 1)
            {
                var methodName = valueArray[0];
                int parameterCount = valueArray.Length - 1;
                ConsoleMethod consoleMethod;
                if (MethodSchema.TryGetMethod(methodName, parameterCount, out consoleMethod))
                {
                    string[] parameters = new string[parameterCount];
                    Array.Copy(valueArray, 1, parameters, 0, parameterCount);
                    Recorder.WriteMethod(message);
                    consoleMethod.Invoke(parameters);
                }
                else
                {
                    ThrowMethodNotFound(methodName, parameterCount);
                }
            }
        }

        /// <summary>
        /// 抛出传入方法命令不正确异常;
        /// </summary>
        private static void ThrowMethodStringIncorrect(string methodName)
        {
            throw new ArgumentException(string.Format("不合法的命令[{0}];", methodName));
        }

        private static void ThrowMethodNotFound(string methodName, int parameterCount)
        {
            throw new KeyNotFoundException(string.Format("未找到方法:[{0}]参数总数:[{1}]", methodName, parameterCount));
        }
    }
}
