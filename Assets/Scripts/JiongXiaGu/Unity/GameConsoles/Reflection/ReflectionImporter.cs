using System;
using System.Collections.Generic;
using System.Reflection;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 反射控制台方法反射器;
    /// </summary>
    public static class ReflectionImporter
    {
        /// <summary>
        /// 发射的方法类型;
        /// </summary>
        private const BindingFlags DefaultMethodBindingAttrs = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.InvokeMethod;

        /// <summary>
        /// 枚举所有方法;
        /// </summary>
        public static IEnumerable<IMethod> EnumerateMethods(Assembly assembly, BindingFlags bindingFlags = DefaultMethodBindingAttrs)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (IsEffective(type))
                {
                    var methodInfos = type.GetMethods(bindingFlags | BindingFlags.Static);

                    foreach (var methodInfo in methodInfos)
                    {
                        IMethod method = GetMethod(methodInfo);
                        if (method != null)
                        {
                            yield return method;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 该Type类型是否需要被搜寻;
        /// </summary>
        public static bool IsEffective(Type type)
        {
            bool isEffective = false;
            if (type.IsClass)
            {
                var attributes = type.GetCustomAttributes();
                foreach (var attribute in attributes)
                {
                    if (attribute is ObsoleteAttribute)
                    {
                        return false;
                    }
                    if (attribute is ConsoleMethodClassAttribute)
                    {
                        isEffective = true;
                    }
                }
            }
            return isEffective;
        }


        /// <summary>
        /// 枚举所有控制台方法;
        /// </summary>
        public static IEnumerable<IMethod> EnumerateMethods(Type type, BindingFlags bindingFlags = DefaultMethodBindingAttrs)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var methodInfos = type.GetMethods(bindingFlags | BindingFlags.Static);

            foreach (var methodInfo in methodInfos)
            {
                IMethod method = GetMethod(methodInfo);
                if (method != null)
                {
                    yield return method;
                }
            }
        }

        private static IMethod GetMethod(MethodInfo methodInfo)
        {
            ConsoleMethodAttribute consoleMethodAttribute = null;
            object[] attributes = methodInfo.GetCustomAttributes(false);

            foreach (var attribute in attributes)
            {
                if (attribute is ObsoleteAttribute)
                {
                    return null;
                }

                consoleMethodAttribute = attribute as ConsoleMethodAttribute;
            }

            if (consoleMethodAttribute != null)
            {
                var method = Method.Create(methodInfo, null, consoleMethodAttribute.GetDescription());
                return method;
            }
            else
            {
                return null;
            }
        }
    }
}
