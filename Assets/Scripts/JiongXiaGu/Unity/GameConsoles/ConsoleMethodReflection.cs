using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 反射控制台方法;
    /// </summary>
    public class ConsoleMethodReflection : ReflectionHandler
    {
        /// <summary>
        /// 默认实参;
        /// </summary>
        public static ConsoleMethodReflection Default { get; private set; } = new ConsoleMethodReflection();

        private static readonly BindingAttrGroup BindingAttrs = new BindingAttrGroup()
        {
            Method = BindingFlags.Public | BindingFlags.Static,
        };

        private readonly List<PrerequisiteInfo> tempPrerequisites;
        private readonly List<ConsoleMethodInfo> tempMethods;
        private readonly List<Exception> exception;

        private ConsoleMethodSchema MethodSchema
        {
            get { return GameConsole.MethodSchema; }
        }

        /// <summary>
        /// 在读取时出现的异常;
        /// </summary>
        public IReadOnlyCollection<Exception> Exceptions
        {
            get { return exception; }
        }

        /// <summary>
        /// 反射控制台方法;
        /// </summary>
        public ConsoleMethodReflection() : base(BindingAttrs)
        {
            tempPrerequisites = new List<PrerequisiteInfo>();
            tempMethods = new List<ConsoleMethodInfo>();
            exception = new List<Exception>();
        }

        public override bool Do(Type type)
        {
            var attribute = type.GetCustomAttribute<ConsoleMethodClassAttribute>();
            return attribute != null;
        }

        public override void Do(IEnumerable<MethodInfo> methodInfos)
        {
            //整理前提方法和控制台执行方法;
            foreach (var methodInfo in methodInfos)
            {
                var attributes = methodInfo.GetCustomAttributes<ConsoleMethodBaseAttribute>();
                foreach (var attribute in attributes)
                {
                    try
                    {
                        var consoleMethod = attribute as ConsoleMethodAttribute;
                        if (consoleMethod != null)
                        {
                            ConsoleMethodInfo info = new ConsoleMethodInfo(methodInfo, consoleMethod);
                            tempMethods.Add(info);
                            continue;
                        }

                        var prerequisiteAttribute = attribute as ConsoleMethodPrerequisiteAttribute;
                        if (prerequisiteAttribute != null)
                        {
                            var prerequisite = CreatePrerequisite(methodInfo);
                            PrerequisiteInfo info = new PrerequisiteInfo(prerequisiteAttribute.Name, prerequisite);
                            tempPrerequisites.Add(info);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        exception.Add(ex);
                        continue;
                    }
                }
            }

            //将可用的控制台方法加入到合集;
            foreach (var methodInfo in tempMethods)
            {
                try
                {
                    ConsoleMethod consoleMethod = CreateConsoleMethod(methodInfo);
                    MethodSchema.Add(consoleMethod);
                }
                catch(Exception ex)
                {
                    exception.Add(ex);
                    continue;
                }
            }

            tempPrerequisites.Clear();
            tempMethods.Clear();
        }

        /// <summary>
        /// 创建前提的委托类型;
        /// </summary>
        private Func<bool> CreatePrerequisite(MethodInfo methodInfo)
        {
            return (Func<bool>)methodInfo.CreateDelegate(typeof(Func<bool>));
        }

        /// <summary>
        /// 创建控制台方法,若未能创建成功则返回异常;
        /// </summary>
        private ConsoleMethod CreateConsoleMethod(ConsoleMethodInfo info)
        {
            Func<bool> prerequisite = null;
            string prerequisiteName = info.Attribute.PrerequisiteName;
            if (!string.IsNullOrWhiteSpace(prerequisiteName))
            {
                int index = tempPrerequisites.FindIndex(item => item.Name == prerequisiteName);
                if (index >= 0)
                {
                    var prerequisiteInfo = tempPrerequisites[index];
                    prerequisite = prerequisiteInfo.Prerequisite;
                }
                else
                {
                    throw new MissingMethodException(string.Format("构建方法[{0}]时出错,未找到提供前提的方法[{1}];", info.MethodInfo.Name, prerequisiteName));
                }
            }
            ConsoleMethod consoleMethod = ConsoleMethod.Create(info.MethodInfo, null, info.Attribute.Description, prerequisite);
            return consoleMethod;
        }

        /// <summary>
        /// 方法信息;
        /// </summary>
        private struct ConsoleMethodInfo
        {
            public ConsoleMethodInfo(MethodInfo methodInfo, ConsoleMethodAttribute attribute)
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
        private struct PrerequisiteInfo
        {
            public PrerequisiteInfo(MethodInfo methodInfo, string name) : this()
            {
                Name = name;
                Prerequisite = (Func<bool>)methodInfo.CreateDelegate(typeof(Func<bool>));
            }

            public PrerequisiteInfo(string name, Func<bool> prerequisite)
            {
                Name = name;
                Prerequisite = prerequisite;
            }

            public string Name { get; private set; }
            public Func<bool> Prerequisite { get; private set; }
        }
    }

    /// <summary>
    /// 控制台方法类标记;
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ConsoleMethodClassAttribute : Attribute
    {
    }

    /// <summary>
    /// 用于限制方法挂载特性;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ConsoleMethodBaseAttribute : Attribute
    {
    }

    /// <summary>
    /// 控制台方法前提,需要挂在"公共静态方法"上,应该为 Func<bool> 类型;
    /// </summary>
    public sealed class ConsoleMethodPrerequisiteAttribute : ConsoleMethodBaseAttribute
    {
        public string Name { get; private set; }

        public ConsoleMethodPrerequisiteAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// 控制台方法命令,需要挂在"公共静态方法"上,传入参数都为字符串;
    /// </summary>
    public sealed class ConsoleMethodAttribute : ConsoleMethodBaseAttribute
    {
        private ConsoleMethodDesc description;

        public string PrerequisiteName { get; private set; }

        public ConsoleMethodDesc Description
        {
            get { return description; }
            private set { description = value; }
        }

        public string Name
        {
            get { return description.FullName; }
            set { description.FullName = value; }
        }

        public string Message
        {
            get { return description.Message; }
            set { description.Message = value; }
        }

        public string[] ParameterDes
        {
            get { return description.Parameters.Convert(); }
            set { description.Parameters = ParameterDescCollection.Convert(value); }
        }

        public ConsoleMethodAttribute(string name) : this(name, string.Empty, string.Empty, null)
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="message">方法描述</param>
        /// <param name="parameterDes">参数描述</param>
        public ConsoleMethodAttribute(string name, string message, string[] parameterDes = null)
        {
            Name = name;
            Message = message;
            ParameterDes = parameterDes;
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="message">方法描述</param>
        /// <param name="prerequisite">前提方法名</param>
        /// <param name="parameterDes">参数描述</param>
        public ConsoleMethodAttribute(string name, string prerequisite, string message, string[] parameterDes = null)
        {
            Name = name;
            PrerequisiteName = prerequisite;
            Message = message;
            ParameterDes = parameterDes;
        }
    }
}
