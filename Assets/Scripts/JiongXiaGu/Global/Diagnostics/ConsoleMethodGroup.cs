using System;
using System.Linq;
using System.Collections;
using JiongXiaGu.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Diagnostics
{

    /// <summary>
    /// 命令组;
    /// </summary>
    public class ConsoleMethodGroup : ICollection<IConsoleMethod>
    {
        public ConsoleMethodGroup(string key)
        {
            Key = key;
            commandItems = new List<IConsoleMethod>();
        }

        readonly List<IConsoleMethod> commandItems;
        public string Key { get; private set; }

        public int Count
        {
            get { return commandItems.Count; }
        }

        bool ICollection<IConsoleMethod>.IsReadOnly
        {
            get { return false; }
        }

        //public bool Operate(string key, string[] parameters)
        //{
        //    if (Key != key)
        //        throw new ArgumentException("key");

        //    int index = commandItems.FindIndex(item => item.ParameterNumber == parameters.Length);
        //    if (index >= 0)
        //    {
        //        ICommandItem commandItem = commandItems[index];

        //        if (commandItem.IsDeveloperMethod && !XiaGu.IsDeveloperMode)
        //        {
        //            return false;
        //        }

        //        commandItem.Operate(parameters);
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// 获取到对应的方法;
        /// </summary>
        public IConsoleMethod Find(string key, int parameterCount)
        {
            int index = commandItems.FindIndex(item => item.ParameterNumber == parameterCount);
            if (index >= 0)
            {
                IConsoleMethod item = commandItems[index];
                return item;
            }
            return null;
        }

        public void Add(IConsoleMethod item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (Contains(item.ParameterNumber))
                throw new ArgumentException("已经存在相同的命令;" + item.ToString());

            commandItems.Add(item);
        }

        public bool Remove(IConsoleMethod item)
        {
            return commandItems.Remove(item);
        }

        public bool Contains(int contentNumber)
        {
            return commandItems.Contains(item => item.ParameterNumber == contentNumber);
        }

        public bool Contains(IConsoleMethod item)
        {
            return commandItems.Contains(item);
        }

        public void Clear()
        {
            commandItems.Clear();
        }

        void ICollection<IConsoleMethod>.CopyTo(IConsoleMethod[] array, int arrayIndex)
        {
            commandItems.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IConsoleMethod> GetEnumerator()
        {
            return commandItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
