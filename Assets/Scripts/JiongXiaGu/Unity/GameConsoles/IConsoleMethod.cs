namespace JiongXiaGu.Unity.GameConsoles
{
    public interface IConsoleMethod
    {
        /// <summary>
        /// 方法描述;
        /// </summary>
        ConsoleMethodDesc Description { get; }

        /// <summary>
        /// 参数数量;
        /// </summary>
        int ParameterCount { get; }

        /// <summary>
        /// 当前该方法是否可用?
        /// </summary>
        bool Prerequisite();

        /// <summary>
        /// 调用当前方法,若不存在参数,则传入null;
        /// </summary>
        void Invoke(string[] parameters);
    }
}
