using System;
using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 命令组;
    /// </summary>
    public class CommandGroup : ICollection<ICommandItem>
    {
        public CommandGroup(string key)
        {
            Key = key;
            commandItems = new List<ICommandItem>();
        }

        readonly List<ICommandItem> commandItems;
        public string Key { get; private set; }

        public int Count
        {
            get { return commandItems.Count; }
        }

        bool ICollection<ICommandItem>.IsReadOnly
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
        public ICommandItem Find(string key, int parameterCount)
        {
            int index = commandItems.FindIndex(item => item.ParameterNumber == parameterCount);
            if (index >= 0)
            {
                ICommandItem item = commandItems[index];
                return item;
            }
            return null;
        }

        public void Add(ICommandItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (Contains(item.ParameterNumber))
                throw new ArgumentException("已经存在相同的命令;" + item.ToString());

            commandItems.Add(item);
        }

        public bool Remove(ICommandItem item)
        {
            return commandItems.Remove(item);
        }

        public bool Contains(int contentNumber)
        {
            return commandItems.Contains(item => item.ParameterNumber == contentNumber);
        }

        public bool Contains(ICommandItem item)
        {
            return commandItems.Contains(item);
        }

        public void Clear()
        {
            commandItems.Clear();
        }

        void ICollection<ICommandItem>.CopyTo(ICommandItem[] array, int arrayIndex)
        {
            commandItems.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ICommandItem> GetEnumerator()
        {
            return commandItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
