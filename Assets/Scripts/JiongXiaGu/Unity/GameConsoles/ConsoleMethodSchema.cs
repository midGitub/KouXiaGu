﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 控制台方法合集(线程安全);
    /// </summary>
    public class ConsoleMethodSchema
    {
        /// <summary>
        /// 方法字典;
        /// </summary>
        private readonly ConcurrentDictionary<string, IConsoleMethod> consoleMethods;

        /// <summary>
        /// 方法数目;
        /// </summary>
        public int Count
        {
            get { return consoleMethods.Count; }
        }

        public ICollection<IConsoleMethod> ConsoleMethods
        {
            get { return consoleMethods.Values; }
        }

        public ConsoleMethodSchema()
        {
            consoleMethods = new ConcurrentDictionary<string, IConsoleMethod>();
        }

        /// <summary>
        /// 获取到用于保存到字典的关键词;
        /// </summary>
        private string GetKey(ConsoleMethod method)
        {
            return GetKey(method.Name, method.ParameterCount);
        }

        /// <summary>
        /// 获取到用于保存到字典的关键词;
        /// </summary>
        private string GetKey(string methodName, int parameterCount)
        {
            return string.Format("{0}[{1}]", methodName.ToLower(), parameterCount);
        }

        /// <summary>
        /// 尝试添加控制台方法;
        /// </summary>
        public bool TryAdd(ConsoleMethod consoleMethod)
        {
            if (consoleMethod == null)
                throw new ArgumentNullException(nameof(consoleMethod));

            string key = GetKey(consoleMethod);
            return consoleMethods.TryAdd(key, consoleMethod);
        }

        /// <summary>
        /// 尝试从和合集中移除指定方法,移除成功返回 true;
        /// </summary>
        public bool TryRemove(string methodName, int parameterCount, out IConsoleMethod consoleMethod)
        {
            string key = GetKey(methodName, parameterCount);
            return consoleMethods.TryRemove(key, out consoleMethod);
        }

        /// <summary>
        /// 确认是否存在此方法;
        /// </summary>
        public bool Contains(string methodName, int parameterCount)
        {
            string key = GetKey(methodName, parameterCount);
            return consoleMethods.ContainsKey(key);
        }

        /// <summary>
        /// 尝试获取到指定方法,若未能找到则返回false;
        /// </summary>
        public bool TryGetMethod(string methodName, int parameterCount, out IConsoleMethod consoleMethod)
        {
            string key = GetKey(methodName, parameterCount);
            return consoleMethods.TryGetValue(key, out consoleMethod);
        }

        /// <summary>
        /// 清除所有方法;
        /// </summary>
        public void Clear()
        {
            consoleMethods.Clear();
        }
    }
}