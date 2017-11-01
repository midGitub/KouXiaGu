using System;

namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台方法类标记;
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ConsoleMethodClassAttribute : Attribute
    {
    }
}
