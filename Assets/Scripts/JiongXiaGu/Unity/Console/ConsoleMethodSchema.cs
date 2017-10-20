using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 控制台方法合集;
    /// </summary>
    public class ConsoleMethodSchema
    {
        private readonly Dictionary<string, ConsoleMethodGroup> consoleMethods;

        /// <summary>
        /// 方法合集数目,不包括重载的方法;
        /// </summary>
        public int Count
        {
            get { return consoleMethods.Count; }
        }

        public ConsoleMethodSchema()
        {
            consoleMethods = new Dictionary<string, ConsoleMethodGroup>();
        }

        /// <summary>
        /// 添加控制台方法,若已经存在则返回异常;
        /// </summary>
        public void Add(ConsoleMethodInfo consoleMethod)
        {
            if (consoleMethod == null)
                throw new ArgumentNullException(nameof(consoleMethod));

            ConsoleMethodGroup group;
            if (consoleMethods.TryGetValue(consoleMethod.FullName, out group))
            {
                group.Add(consoleMethod);
            }
            else
            {
                group = new ConsoleMethodGroup(consoleMethod.FullName);
                group.Add(consoleMethod);
            }
        }

        /// <summary>
        /// 从和合集中移除指定方法;
        /// </summary>
        public bool Remove(ConsoleMethodInfo consoleMethod)
        {
            if (consoleMethod == null)
                throw new ArgumentNullException(nameof(consoleMethod));

            ConsoleMethodGroup group;
            if (consoleMethods.TryGetValue(consoleMethod.FullName, out group))
            {
                return group.Remove(consoleMethod);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 确认是否存在此方法;
        /// </summary>
        public bool Contains(ConsoleMethodInfo consoleMethod)
        {
            if (consoleMethod == null)
                throw new ArgumentNullException(nameof(consoleMethod));

            ConsoleMethodGroup group;
            if (consoleMethods.TryGetValue(consoleMethod.FullName, out group))
            {
                return group.Contains(consoleMethod);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取到指定方法,若未能找到则返回异常;
        /// </summary>
        public ConsoleMethodInfo GetMethod(string name, int parameterNumber = 0)
        {
            ConsoleMethodGroup group;
            if (consoleMethods.TryGetValue(name, out group))
            {
                return group.Get(parameterNumber);
            }
            else
            {
                throw new KeyNotFoundException(string.Format("未能找到名为[{0}]的方法;", name));
            }
        }

        /// <summary>
        /// 清除所有方法;
        /// </summary>
        public void Clear()
        {
            consoleMethods.Clear();
        }

        /// <summary>
        /// 枚举所有方法合集;
        /// </summary>
        public IEnumerable<IReadOnlyCollection<ConsoleMethodInfo>> EnumerateMethodGroup()
        {
            return consoleMethods.Values.OfType<IReadOnlyCollection<ConsoleMethodInfo>>();
        }

        /// <summary>
        /// 获取到方法总数;
        /// </summary>
        public int GetMethodCount()
        {
            return consoleMethods.Values.Sum(group => group.Count);
        }

        /// <summary>
        /// 相同名的方法组,按参数个数区分;
        /// </summary>
        private class ConsoleMethodGroup : IReadOnlyCollection<ConsoleMethodInfo>
        {
            public string FullName { get; private set; }
            private readonly List<ConsoleMethodInfo> consoleMethods;

            public ConsoleMethodGroup(string fullName)
            {
                FullName = fullName;
                consoleMethods = new List<ConsoleMethodInfo>();
            }

            public ConsoleMethodGroup(string fullName, IEnumerable<ConsoleMethodInfo> consoleMethod)
            {
                FullName = fullName;
                consoleMethods = new List<ConsoleMethodInfo>(consoleMethod);
            }

            public int Count
            {
                get { return consoleMethods.Count; }
            }

            /// <summary>
            /// 添加控制台方法;
            /// </summary>
            public void Add(ConsoleMethodInfo consoleMethod)
            {
                if (Contains(consoleMethod.ParameterCount))
                {
                    throw new ArgumentException(string.Format("已经存在相同参数数量为[{0}]的方法;", consoleMethod.ParameterCount));
                }
                consoleMethods.Add(consoleMethod);
            }

            /// <summary>
            /// 移除指定元素;
            /// </summary>
            public bool Remove(ConsoleMethodInfo consoleMethod)
            {
                return consoleMethods.Remove(consoleMethod);
            }

            /// <summary>
            /// 确认是否存在此方法;
            /// </summary>
            public bool Contains(ConsoleMethodInfo consoleMethod)
            {
                return consoleMethods.Contains(consoleMethod);
            }

            /// <summary>
            /// 确认是否存在相同参数数目的方法;
            /// </summary>
            public bool Contains(int parameterNumber)
            {
                return consoleMethods.Find(item => item.ParameterCount == parameterNumber) != null;
            }

            /// <summary>
            /// 获取到指定方法,若未能找到则返回异常 KeyNotFoundException;
            /// </summary>
            public ConsoleMethodInfo Get(int parameterNumber)
            {
                ConsoleMethodInfo method = consoleMethods.Find(item => item.ParameterCount == parameterNumber);
                if (method == null)
                {
                    throw new KeyNotFoundException(string.Format("未能找到名[{0}]参数为[{1}]的方法", FullName, parameterNumber));
                }
                else
                {
                    return method;
                }
            }

            public IEnumerator<ConsoleMethodInfo> GetEnumerator()
            {
                return consoleMethods.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
