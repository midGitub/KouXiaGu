using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 反射控制台方法;
    /// </summary>
    public class ConsoleMethodReflection : ReflectionHandler
    {
        private static readonly BindingAttrGroup BindingAttrs = new BindingAttrGroup()
        {
            Field = BindingFlags.Public | BindingFlags.Static,
            Method = BindingFlags.Public | BindingFlags.Static,
            Property = BindingFlags.Default,
        };

        public ConsoleMethodReflection(BindingAttrGroup bindingAttrs) : base(bindingAttrs)
        {
        }

        public override bool Do(Type type)
        {
            throw new NotImplementedException();
        }

        public override void Do(IEnumerable<MethodInfo> methodInfos)
        {
            base.Do(methodInfos);
        }
    }

    /// <summary>
    /// 控制台方法类标记;
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ConsoleMethodsClassAttribute : Attribute
    {
    }


    /// <summary>
    /// 控制台方法命令,需要挂在"公共静态方法"上,传入参数都为字符串;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class ConsoleMethodAttribute : Attribute
    {
        private ConsoleMethodDescription description;

        public ConsoleMethodAttribute(string key)
            : this(key, string.Empty)
        {
        }

        public ConsoleMethodAttribute(string key, string message)
            : this(key, message, null)
        {
        }
        
        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="message">方法描述</param>
        /// <param name="parameterDes">参数描述</param>
        public ConsoleMethodAttribute(string name, string message, params string[] parameterDes)
        {
            Name = name;
            //Message = XiaGuConsole.ConvertMassage(key, message, parameterTypes);
        }

        public ConsoleMethodDescription Description
        {
            get { return description; }
            private set { description = value; }
        }

        public string Name
        {
            get { return description.FullName; }
            private set { description.FullName = value; }
        }

        public string Message
        {
            get { return description.Message; }
            private set { description.Message = value; }
        }

        public string[] ParameterDes { get; private set; }

    }
}
