namespace JiongXiaGu.Resources
{
    /// <summary>
    /// 读取资源;
    /// </summary>
    /// <typeparam name="T">读取到T;</typeparam>
    public interface IResourceSerializer<T>
    {
        void Serialize(T result);
        T Deserialize();
    }
}
