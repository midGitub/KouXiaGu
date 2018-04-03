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
        /// 名称;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 方法标签,默认为开发者标签;
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 描述;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 前提方法名,若不存在则为Null;
        /// </summary>
        public string PrerequisiteName { get; set; }

        /// <summary>
        /// 参数描述:[参数1类型, 参数1描述, 参数2类型, 参数2描述, ....]
        /// </summary>
        public string[] ParameterDes { get; set; }

        public ConsoleMethodAttribute(string name)
        {
            Name = name;
            Tag = GameConsole.DeveloperModeTag;
        }

        /// <summary>
        /// 获取到
        /// </summary>
        public MethodDescription GetDescription()
        {
            return new MethodDescription()
            {
                Name = Name,
                Tag = Tag,
                Message = Message,
                ParameterDescs = ParametersDesc.Convert(ParameterDes),
            };
        }
    }
}
