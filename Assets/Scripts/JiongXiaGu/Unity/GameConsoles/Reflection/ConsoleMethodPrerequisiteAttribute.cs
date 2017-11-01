using System;

namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台方法前提,需要挂在"公共静态方法"上,应该为 Func<bool> 类型;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ConsoleMethodPrerequisiteAttribute : Attribute
    {
        public string Name { get; private set; }

        public ConsoleMethodPrerequisiteAttribute(string name)
        {
            Name = name;
        }
    }
}
