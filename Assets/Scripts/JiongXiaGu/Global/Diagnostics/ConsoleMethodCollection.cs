using System;
using System.Collections.Generic;

namespace JiongXiaGu.Diagnostics
{

    /// <summary>
    /// 控制台命令合集;
    /// </summary>
    public class ConsoleMethodCollection
    {
        public ConsoleMethodCollection()
        {
            CommandDictionary = new Dictionary<string, ConsoleMethodGroup>();
        }

        public ConsoleMethodCollection(IDictionary<string, ConsoleMethodGroup> commandDictionary)
        {
            CommandDictionary = commandDictionary;
        }

        /// <summary>
        /// 命令合集;
        /// </summary>
        internal IDictionary<string, ConsoleMethodGroup> CommandDictionary { get; private set; }

        public int Count
        {
            get { return CommandDictionary.Count; }
        }

        /// <summary>
        /// 获取到对应的方法和应该传入的参数;
        /// </summary>
        public IConsoleMethod Find(string message, out string[] parameters)
        {
            string key;
            return Find(message, out key, out parameters);
        }

        static readonly char[] separator = new char[] { ' ' };

        /// <summary>
        /// 获取到对应的方法和应该传入的参数;
        /// </summary>
        public IConsoleMethod Find(string message, out string key, out string[] parameters)
        {
            string[] words = message.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            key = words[0];
            ConsoleMethodGroup commandGroup;

            if (CommandDictionary.TryGetValue(key, out commandGroup))
            {
                int parameterCount = words.Length - 1;
                parameters = new string[parameterCount];
                Array.Copy(words, 1, parameters, 0, parameterCount);
                return commandGroup.Find(key, parameters.Length);
            }
            parameters = default(string[]);
            return null;
        }

        /// <summary>
        /// 添加方法到,若已经存在则返回异常;
        /// </summary>
        public void Add(string key, IConsoleMethod commandItem)
        {
            if (commandItem == null)
                throw new ArgumentNullException("commandItem");

            ConsoleMethodGroup commandGroup;
            if (CommandDictionary.TryGetValue(key, out commandGroup))
            {
                commandGroup.Add(commandItem);
            }
            else
            {
                commandGroup = new ConsoleMethodGroup(key);
                commandGroup.Add(commandItem);
                CommandDictionary.Add(key, commandGroup);
            }
        }


        ///// <summary>
        ///// 若找不到对应命令,则返回false;
        ///// </summary>
        //public bool Operate(string message)
        //{
        //    string[] words = message.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        //    string key = words[0];
        //    CommandGroup commandGroup;

        //    if (CommandDictionary.TryGetValue(key, out commandGroup))
        //    {
        //        int parameterCount = words.Length - 1;
        //        string[] parameters = new string[parameterCount];
        //        Array.Copy(words, 1, parameters, 0, parameterCount);
        //        return commandGroup.Operate(key, parameters);
        //    }

        //    return false;
        //}
    }
}
