using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 控制台命令合集;
    /// </summary>
    public class ConsoleCommand
    {
        public ConsoleCommand()
        {
            CommandDictionary = new Dictionary<string, CommandGroup>();
        }

        public ConsoleCommand(IDictionary<string, CommandGroup> commandDictionary)
        {
            CommandDictionary = commandDictionary;
        }

        /// <summary>
        /// 命令合集;
        /// </summary>
        internal IDictionary<string, CommandGroup> CommandDictionary { get; private set; }

        static readonly char[] separator = new char[] { ' ' };

        /// <summary>
        /// 若找不到对应命令,则返回false;
        /// </summary>
        public bool Operate(string message)
        {
            string[] words = message.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string key = words[0];
            CommandGroup commandGroup;

            if (CommandDictionary.TryGetValue(key, out commandGroup))
            {
                int parameterCount = words.Length - 1;
                string[] parameters = new string[parameterCount];
                Array.Copy(words, 1, parameters, 0, parameterCount);
                return commandGroup.Operate(key, parameters);
            }

            return false;
        }

        /// <summary>
        /// 添加方法到,若已经存在则返回异常;
        /// </summary>
        public void Add(string key, ICommandItem commandItem)
        {
            if (commandItem == null)
                throw new ArgumentNullException("commandItem");

            CommandGroup commandGroup;
            if (CommandDictionary.TryGetValue(key, out commandGroup))
            {
                commandGroup.Add(commandItem);
            }
            else
            {
                commandGroup = new CommandGroup(key);
                commandGroup.Add(commandItem);
                CommandDictionary.Add(key, commandGroup);
            }
        }
    }

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

        public bool Operate(string key, string[] parameters)
        {
            if (Key != key)
                throw new ArgumentException("key");

            int index = commandItems.FindIndex(item => item.ParameterNumber == parameters.Length);
            if (index >= 0)
            {
                ICommandItem commandItem = commandItems[index];

                if (commandItem.IsDeveloperMethod && !XiaGu.IsDeveloperMode)
                {
                    return false;
                }

                commandItem.Operate(parameters);
                return true;
            }
            return false;
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

    /// <summary>
    /// 命令条目接口;
    /// </summary>
    public interface ICommandItem
    {
        /// <summary>
        /// 是否为开发模式方法?
        /// </summary>
        bool IsDeveloperMethod { get; }

        /// <summary>
        /// 参数数量;
        /// </summary>
        int ParameterNumber { get; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 对应的操作;
        /// </summary>
        void Operate(string[] parameters);
    }

    /// <summary>
    /// 委托类型的条目;
    /// </summary>
    public class CommandItem : ICommandItem
    {
        public CommandItem(string key, Action<string[]> action, string message, bool isDeveloperMethod, params string[] parameterTypes)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            Action = action;
            ParameterNumber = parameterTypes.Length;
            Message = GameConsole.ConvertMassage(key, message, parameterTypes);
            IsDeveloperMethod = isDeveloperMethod;
        }

        public int ParameterNumber { get; private set; }
        public Action<string[]> Action { get; private set; }
        public string Message { get; private set; }
        public bool IsDeveloperMethod { get; private set; }

        public void Operate(string[] parameters)
        {
            Action(parameters);
        }
    }


    //反射获取控制台方法;
    #region Reflection

    /// <summary>
    /// 控制台方法类标记;
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class ConsoleMethodsClassAttribute : Attribute
    {
    }

    /// <summary>
    /// 控制台方法命令,需要挂在"公共静态方法"上,传入参数都为字符串;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class ConsoleMethodAttribute : Attribute
    {
        public ConsoleMethodAttribute(string key)
            : this(key, string.Empty)
        {
        }

        public ConsoleMethodAttribute(string key, string message)
            : this(key, message, null)
        {
        }

        public ConsoleMethodAttribute(string key, string message, params string[] parameterTypes)
        {
            Key = key;
            Message = GameConsole.ConvertMassage(key, message, parameterTypes);
        }

        public string Key { get; private set; }
        public string Message { get; private set; }
        public bool IsDeveloperMethod { get; set; }
    }


    public static class ConsoleMethodsReflection
    {

        /// <summary>
        /// 把程序集所有标记了特性的方法到合集;
        /// </summary>
        public static void SearchMethod(ConsoleCommand commandDictionary)
        {
            Assembly assembly = typeof(ConsoleMethodsReflection).Assembly;
            SearchMethod(assembly, commandDictionary);
        }

        /// <summary>
        /// 把程序集所有标记了特性的方法到合集;
        /// </summary>
        public static void SearchMethod(Assembly assembly, ConsoleCommand commandDictionary)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(KouXiaGu.ConsoleMethodsClassAttribute), false);
                if (attributes.Length > 0)
                {
                    SearchMethod(type, commandDictionary);
                }
            }
        }

        /// <summary>
        /// 把所有标记了的公共静态方法加入到合集;
        /// </summary>
        static void SearchMethod(Type classType, ConsoleCommand commandDictionary)
        {
            MethodInfo[] methods = classType.GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ConsoleMethodAttribute), false);
                if (attributes.Length > 0)
                {
                    var attribute = (ConsoleMethodAttribute)attributes[0];
                    ReflectionCommandItem commandItem = new ReflectionCommandItem(method, attribute.Message, attribute.IsDeveloperMethod);
                    commandDictionary.Add(attribute.Key, commandItem);
                }
            }
        }
    }

    public class ReflectionCommandItem : ICommandItem
    {
        public ReflectionCommandItem(MethodInfo methodInfo, string message, bool isDeveloperMethod)
        {
            MethodInfo = methodInfo;
            ParameterNumber = methodInfo.GetParameters().Length;
            Message = message;
            IsDeveloperMethod = isDeveloperMethod;
        }

        public int ParameterNumber { get; private set; }
        public string Message { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public bool IsDeveloperMethod { get; private set; }

        public void Operate(string[] parameters)
        {
            MethodInfo.Invoke(null, parameters);
        }
    }

    #endregion
}
