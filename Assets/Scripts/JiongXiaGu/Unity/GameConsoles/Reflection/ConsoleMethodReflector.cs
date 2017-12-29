using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 反射控制台方法反射器;
    /// </summary>
    public class ConsoleMethodReflector
    {
        /// <summary>
        /// 发射的方法类型;
        /// </summary>
        private static readonly BindingFlags MethodBindingAttrs = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.InvokeMethod;

        private List<ConsoleMethodReflected> tempConsoleMethods;
        private readonly List<_PrerequisiteInfo> tempPrerequisites;
        private readonly List<_ConsoleMethodInfo> tempMethods;

        public ConsoleMethodReflector()
        {
            tempPrerequisites = new List<_PrerequisiteInfo>();
            tempMethods = new List<_ConsoleMethodInfo>();
        }

        /// <summary>
        /// 获取所有控制台方法;
        /// </summary>
        public List<ConsoleMethodReflected> Search(params Assembly[] assemblys)
        {
            return Search(assemblys as IEnumerable<Assembly>);
        }

        /// <summary>
        /// 获取所有控制台方法;
        /// </summary>
        public List<ConsoleMethodReflected> Search(IEnumerable<Assembly> assemblys)
        {
            if (assemblys == null)
                throw new ArgumentNullException(nameof(assemblys));

            var consoleMethods = tempConsoleMethods = new List<ConsoleMethodReflected>();

            foreach (var assembly in assemblys)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (IsEffective(type))
                    {
                        SearchInternal(type);
                    }
                }
            }

            tempConsoleMethods = null;
            return consoleMethods;
        }

        /// <summary>
        /// 该Type类型是否需要被搜寻;
        /// </summary>
        public bool IsEffective(Type type)
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
        /// 搜索获取到所有控制台方法;
        /// </summary>
        public List<ConsoleMethodReflected> Search(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var consoleMethods = tempConsoleMethods = new List<ConsoleMethodReflected>();
            SearchInternal(type);
            tempConsoleMethods = null;
            return consoleMethods;
        }

        /// <summary>
        /// 搜索获取到所有控制台方法;
        /// </summary>
        private void SearchInternal(Type type)
        {
            var methodInfos = type.GetMethods(MethodBindingAttrs);

            //整理前提方法和控制台执行方法;
            foreach (MethodInfo methodInfo in methodInfos)
            {
                object[] attributes = methodInfo.GetCustomAttributes(false);

                if (!HaveObsoleteAttribute(attributes))
                {
                    foreach (var attribute in attributes)
                    {
                        var consoleMethodAttribute = attribute as ConsoleMethodAttribute;
                        if (consoleMethodAttribute != null)
                        {
                            _ConsoleMethodInfo info = new _ConsoleMethodInfo(methodInfo, consoleMethodAttribute);
                            tempMethods.Add(info);
                            continue;
                        }

                        var prerequisiteAttribute = attribute as ConsoleMethodPrerequisiteAttribute;
                        if (prerequisiteAttribute != null)
                        {
                            _PrerequisiteInfo info = new _PrerequisiteInfo(prerequisiteAttribute.Name, methodInfo);
                            tempPrerequisites.Add(info);
                            continue;
                        }
                    }
                }
            }

            //将可用的控制台方法加入到合集;
            foreach (var methodInfo in tempMethods)
            {
                try
                {
                    ConsoleMethod consoleMethod = CreateConsoleMethod(methodInfo);
                    ConsoleMethodReflected reflected = new ConsoleMethodReflected(consoleMethod, methodInfo.MethodInfo);
                    tempConsoleMethods.Add(reflected);
                }
                catch (Exception ex)
                {
                    ConsoleMethodReflected reflected = new ConsoleMethodReflected(ex, methodInfo.MethodInfo);
                    tempConsoleMethods.Add(reflected);
                }
            }

            tempPrerequisites.Clear();
            tempMethods.Clear();
        }

        /// <summary>
        /// 是否存在[(ObsoleteAttribute)过时特性]
        /// </summary>
        private bool HaveObsoleteAttribute(IEnumerable<object> attributes)
        {
            foreach (var attribute in attributes)
            {
                if (attribute is ObsoleteAttribute)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建控制台方法,若未能创建成功则返回异常;
        /// </summary>
        private ConsoleMethod CreateConsoleMethod(_ConsoleMethodInfo info)
        {
            Func<bool> prerequisite = null;
            string prerequisiteName = info.Attribute.PrerequisiteName;
            if (!string.IsNullOrWhiteSpace(prerequisiteName))
            {
                int index = tempPrerequisites.FindIndex(item => item.Name == prerequisiteName);
                if (index >= 0)
                {
                    var prerequisiteInfo = tempPrerequisites[index];
                    if (prerequisiteInfo.IsFaulted)
                    {
                        throw new MissingMethodException(string.Format("构建方法[{0}]时出错,因为前提方法[{1}]获取时出现异常[{2}];", info.MethodInfo.Name, prerequisiteName, prerequisiteInfo.Exception));
                    }
                    else
                    {
                        prerequisite = prerequisiteInfo.Prerequisite;
                    }
                }
                else
                {
                    throw new MissingMethodException(string.Format("构建方法[{0}]时出错,未找到提供前提的方法[{1}];", info.MethodInfo.Name, prerequisiteName));
                }
            }
            ConsoleMethod consoleMethod = ConsoleMethod.Create(info.MethodInfo, null, info.Attribute.GetDescription(), prerequisite);
            return consoleMethod;
        }

        /// <summary>
        /// 方法信息;
        /// </summary>
        private struct _ConsoleMethodInfo
        {
            public _ConsoleMethodInfo(MethodInfo methodInfo, ConsoleMethodAttribute attribute)
            {
                MethodInfo = methodInfo;
                Attribute = attribute;
            }

            public MethodInfo MethodInfo { get; private set; }
            public ConsoleMethodAttribute Attribute { get; private set; }
        }

        /// <summary>
        /// 参数信息;
        /// </summary>
        private struct _PrerequisiteInfo
        {
            public _PrerequisiteInfo(string name, MethodInfo methodInfo) : this()
            {
                Name = name;
                try
                {
                    Prerequisite = (Func<bool>)methodInfo.CreateDelegate(typeof(Func<bool>));
                    IsCompleted = true;
                    IsFaulted = false;
                    Exception = null;
                }
                catch (Exception ex)
                {
                    Prerequisite = null;
                    IsCompleted = true;
                    IsFaulted = true;
                    Exception = ex;
                }
            }

            /// <summary>
            /// 是否完成?
            /// </summary>
            public bool IsCompleted { get; private set; }

            /// <summary>
            /// 是否失败?
            /// </summary>
            public bool IsFaulted { get; private set; }

            /// <summary>
            /// 失败的原因;
            /// </summary>
            public Exception Exception { get; private set; }

            /// <summary>
            /// 前提名;
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// 前提委托;
            /// </summary>
            public Func<bool> Prerequisite { get; private set; }
        }
    }
}
