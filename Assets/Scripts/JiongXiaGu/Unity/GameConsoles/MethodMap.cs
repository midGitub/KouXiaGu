using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 控制台方法合集;
    /// </summary>
    public class MethodMap
    {
        /// <summary>
        /// 方法字典;
        /// </summary>
        private readonly Dictionary<string, IMethod> methods;

        /// <summary>
        /// 方法数目;
        /// </summary>
        public int Count
        {
            get { return methods.Count; }
        }

        public ICollection<IMethod> Methods
        {
            get { return methods.Values; }
        }

        public MethodMap()
        {
            methods = new Dictionary<string, IMethod>();
        }

        /// <summary>
        /// 获取到用于保存到字典的关键词;
        /// </summary>
        private string GetKey(IMethod method)
        {
            return GetKey(method.Description.Name, method.ParameterCount);
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
        public void Add(IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            string key = GetKey(method);
            methods.Add(key, method);
        }

        public bool TryAdd(IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            string key = GetKey(method);
            return methods.TryAdd(key, method);
        }

        /// <summary>
        /// 尝试从和合集中移除指定方法,移除成功返回 true;
        /// </summary>
        public bool Remove(string methodName, int parameterCount)
        {
            string key = GetKey(methodName, parameterCount);
            return methods.Remove(key);
        }

        /// <summary>
        /// 确认是否存在此方法;
        /// </summary>
        public bool Contains(string methodName, int parameterCount)
        {
            string key = GetKey(methodName, parameterCount);
            return methods.ContainsKey(key);
        }

        /// <summary>
        /// 尝试获取到指定方法,若未能找到则返回false;
        /// </summary>
        public bool TryGetMethod(string methodName, int parameterCount, out IMethod method)
        {
            string key = GetKey(methodName, parameterCount);
            return methods.TryGetValue(key, out method);
        }

        /// <summary>
        /// 清除所有方法;
        /// </summary>
        public void Clear()
        {
            methods.Clear();
        }

        /// <summary>
        /// 输入的方法命令间隔符;
        /// </summary>
        private static readonly char[] methodSeparator = new char[] { ' ' };

        /// <summary>
        /// 尝试获取到指定方法;
        /// </summary>
        public bool TryGetMethod(string message, out IMethod method, out string[] parameters)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(nameof(message));

            if (string.IsNullOrWhiteSpace(message))
            {
                method = default(IMethod);
                parameters = default(string[]);
                return false;
            }

            string[] valueArray = message.Split(methodSeparator, StringSplitOptions.RemoveEmptyEntries);
            var methodName = valueArray[0];

            if (valueArray.Length == 1)
            {
                parameters = null;
                return TryGetMethod(methodName, 0, out method);
            }
            else
            {
                int parameterCount = valueArray.Length - 1;
                parameters = new string[parameterCount];
                Array.Copy(valueArray, 1, parameters, 0, parameterCount);
                return TryGetMethod(methodName, parameterCount, out method);
            }
        }
    }
}
