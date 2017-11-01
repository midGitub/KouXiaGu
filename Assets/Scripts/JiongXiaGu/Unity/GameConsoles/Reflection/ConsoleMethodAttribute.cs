using System;

namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台方法命令,需要挂在"静态方法"上,传入参数都为字符串;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ConsoleMethodAttribute : Attribute
    {
        public string PrerequisiteName { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string[] ParameterDes { get; set; }

        public ConsoleMethodAttribute(string name)
        {
            Name = name;
        }

        public ConsoleMethodDesc GetDescription()
        {
            return new ConsoleMethodDesc()
            {
                FullName = Name,
                Message = Message,
                Parameters = ParametersDesc.Convert(ParameterDes),
            };
        }
    }
}
