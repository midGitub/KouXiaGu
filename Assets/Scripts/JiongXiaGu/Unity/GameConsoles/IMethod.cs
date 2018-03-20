namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 方法抽象类;
    /// </summary>
    public interface IMethod
    {
        /// <summary>
        /// 方法描述;
        /// </summary>
        MethodDescription Description { get; }

        /// <summary>
        /// 参数数量;
        /// </summary>
        int ParameterCount { get; }

        /// <summary>
        /// 调用当前方法,若不存在参数,则传入null;
        /// </summary>
        void Invoke(string[] parameters);
    }
}
