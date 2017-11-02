using System;

namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台方法命令,需要挂在"静态方法"上,传入参数都为字符串;
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ConsoleMethodAttribute : Attribute
    {
        /// <summary>
        /// 前提方法名;
        /// </summary>
        public string PrerequisiteName { get; set; }

        /// <summary>
        /// 完整的方法名;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 方法描述;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 参数描述:[方法1类型, 方法1描述, 方法2类型, 方法2描述, ....]
        /// </summary>
        public string[] ParameterDes { get; set; }

        public ConsoleMethodAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 获取到
        /// </summary>
        public ConsoleMethodDesc GetDescription()
        {
            return new ConsoleMethodDesc()
            {
                Name = Name,
                Message = Message,
                Parameters = ParametersDesc.Convert(ParameterDes),
            };
        }
    }
}
