using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 通过反射获取到控制台方法条目;
    /// </summary>
    public static class ReflectionConsoleMethods
    {

        /// <summary>
        /// 把程序集所有标记了特性的方法到合集;
        /// </summary>
        public static void SearchMethod(ConsoleMethodCollection commandDictionary)
        {
            Assembly assembly = typeof(ReflectionConsoleMethods).Assembly;
            SearchMethod(assembly, commandDictionary);
        }

        /// <summary>
        /// 把程序集所有标记了特性的方法到合集;
        /// </summary>
        public static void SearchMethod(Assembly assembly, ConsoleMethodCollection commandDictionary)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(ConsoleMethodsClassAttribute), false);
                if (attributes.Length > 0)
                {
                    SearchMethod(type, commandDictionary);
                }
            }
        }

        /// <summary>
        /// 把所有标记了的公共静态方法加入到合集;
        /// </summary>
        static void SearchMethod(Type classType, ConsoleMethodCollection commandDictionary)
        {
            MethodInfo[] methods = classType.GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ConsoleMethodAttribute), false);
                if (attributes.Length > 0)
                {
                    var attribute = (ConsoleMethodAttribute)attributes[0];
                    ReflectionConsoleMethod commandItem = new ReflectionConsoleMethod(method, attribute.Message, attribute.IsDeveloperMethod);
                    commandDictionary.Add(attribute.Key, commandItem);
                }
            }
        }
    }

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
            Message = XiaGuConsole.ConvertMassage(key, message, parameterTypes);
        }

        public string Key { get; private set; }
        public string Message { get; private set; }
        public bool IsDeveloperMethod { get; set; }
    }

    /// <summary>
    /// 反射获取的命令条目;
    /// </summary>
    public class ReflectionConsoleMethod : IConsoleMethod
    {
        public ReflectionConsoleMethod(MethodInfo methodInfo, string message, bool isDeveloperMethod)
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
}
