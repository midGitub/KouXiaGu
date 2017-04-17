using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu
{


    public interface IConsoleInput
    {
        void Operate(string message);
    }


    /// <summary>
    /// 待搜索特定方法的类;
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class ConsoleClassAttribute : Attribute
    {
    }

    /// <summary>
    /// 控制台方法命令,需要挂在公共静态方法上,传入参数都为字符串;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ConsoleMethodAttribute : Attribute
    {
        public ConsoleMethodAttribute(string keyword)
        {
            Keyword = keyword;
        }

        public string Keyword { get; private set; }
    }


    /// <summary>
    /// 控制台输入控制,通过反射获得所有 关键词 和其 重载的方法;
    /// </summary>
    [ConsoleClass]
    public class ConsoleInput : IConsoleInput
    {
        public ConsoleInput()
        {
            methodMap = new ConsoleMethodReflection();
        }

        ConsoleMethodReflection methodMap;

        public void Operate(string message)
        {
            object[] parameters;
            string keyword = GetKeywrod(message, out parameters);
            MethodItem method;

            if (methodMap.TryGetMethod(keyword, parameters,out method))
            {
                method.Invoke(parameters);
            }
            else
            {
                throw new ArgumentException("未知命令:" + message);
            }
        }

        string GetKeywrod(string message, out object[] parameters)
        {
            const char separator = ' ';

            string[] wordArray = message.Split(separator);
            string keyWord = wordArray[0];

            int parameterCount = wordArray.Length - 1;
            parameters = new string[parameterCount];
            Array.Copy(wordArray, 1, parameters, 0, parameterCount);

            return keyWord;
        }

        const string testPrefix = "Test:";

        [ConsoleMethod("test")]
        public static void Test()
        {
            Debug.Log(testPrefix + "Null");
        }

        [ConsoleMethod("test")]
        public static void Test(string str)
        {
            Debug.Log(testPrefix + str);
        }

        [ConsoleMethod("test")]
        public static void Test(string str0, string str1)
        {
            Debug.Log(testPrefix + str0 + "," + str1);
        }

        [ConsoleMethod("test")]
        public static void Test(object str)
        {
            Debug.Log(testPrefix + str);
        }

    }


    class ConsoleMethodReflection : Dictionary<string, MethodGroup>
    {
        public ConsoleMethodReflection()
            : this(typeof(ConsoleMethodReflection).Assembly)
        {
        }

        public ConsoleMethodReflection(Assembly assembly)
        {
            SearchClass(assembly);
        }

        void SearchClass(Assembly assembly)
        {
            var types = assembly.GetTypes();
            SearchClass(types);
        }

        void SearchClass(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(ConsoleClassAttribute), false);
                if (attributes.Length > 0)
                {
                    SearchMethod(type);
                }
            }
        }

        void SearchMethod(Type classType)
        {
            MethodInfo[] methods = classType.GetMethods(BindingFlags.Static | BindingFlags.Public);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ConsoleMethodAttribute), false);
                foreach (ConsoleMethodAttribute attribute in attributes)
                {
                    Add(attribute.Keyword, method);
                }
            }
        }

        void Add(string keyWord, MethodInfo methodInfo)
        {
            MethodGroup group;
            if (TryGetValue(keyWord, out group))
            {
                AddMethod(group, methodInfo);
            }
            else
            {
                group = new MethodGroup();
                AddMethod(group, methodInfo);
                Add(keyWord, group);
            }
        }

        void AddMethod(MethodGroup group, MethodInfo methodInfo)
        {
            try
            {
                group.Add(methodInfo);
            }
            catch (ArgumentException ex)
            {
                Debug.LogError("存在相同的控制台命令!" + ex);
            }
        }

        public bool TryGetMethod(string keyword, object[] parameters, out MethodItem method)
        {
            MethodGroup group;

            if (TryGetValue(keyword, out group))
            {
                if (group.TryGetMethod(out method, parameters))
                {
                    return true;
                }
            }

            method = null;
            return false;
        }
    }

    class MethodGroup : IEqualityComparer<MethodItem>
    {
        public MethodGroup()
        {
            methods = new List<MethodItem>();
        }

        List<MethodItem> methods;

        public IEnumerable<MethodItem> Methods
        {
            get { return methods; }
        }

        public void Add(MethodInfo methodInfo)
        {
            MethodItem item = new MethodItem(methodInfo);

            if (Contains(item))
                throw new ArgumentException();

            methods.Add(item);
        }

        bool Contains(MethodItem item)
        {
            return methods.Contains(item, this);
        }

        public bool TryGetMethod(out MethodItem method, params object[] parameters)
        {
            int parameterCount = parameters.Length;
            method = Find(parameterCount);

            if (method == null)
                return false;

            return true;
        }

        /// <summary>
        /// 若为寻找到则返回 null;
        /// </summary>
        public MethodItem Find(int parameterCount)
        {
            var method = methods.Find(item => item.ParameterCount == parameterCount);
            return method;
        }

        public object Invoke(params object[] parameters)
        {
            MethodItem method;

            if (TryGetMethod(out method, parameters))
            {
                return method.Invoke(parameters);
            }
            else
            {
                throw new KeyNotFoundException("Method not Found,ParameterCount:" + parameters.Length);
            }
        }

        public bool Equals(MethodItem x, MethodItem y)
        {
            return x.ParameterCount == y.ParameterCount;
        }

        public int GetHashCode(MethodItem obj)
        {
            return obj.ParameterCount;
        }

    }

    class MethodItem
    {
        public MethodItem(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;

            var parameterInfos = methodInfo.GetParameters();
            ParameterCount = parameterInfos.Length;
        }

        public int ParameterCount { get; private set; }
        public MethodInfo MethodInfo { get; private set; }

        public object Invoke(object[] parameters)
        {
            return MethodInfo.Invoke(null, parameters);
        }
    }

}
