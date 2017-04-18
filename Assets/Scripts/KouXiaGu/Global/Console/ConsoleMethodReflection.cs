using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 标记;
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    sealed class ConsoleClassAttribute : Attribute
    {
    }

    /// <summary>
    /// 控制台方法命令,需要挂在公共静态方法上,传入参数都为字符串;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    class ConsoleMethodAttribute : Attribute
    {
        public ConsoleMethodAttribute(string keyword)
        {
            Keyword = keyword;
        }

        public string Keyword { get; private set; }
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
                UnityEngine.Debug.LogError("存在相同的控制台命令!" + ex);
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
