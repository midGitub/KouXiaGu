namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 读取资源;
    /// </summary>
    /// <typeparam name="T">读取到T;</typeparam>
    public interface IResourceReader<T>
    {
        void Write(T item);
        T Read();
    }
}
