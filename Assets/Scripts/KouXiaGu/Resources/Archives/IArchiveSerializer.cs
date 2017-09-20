namespace KouXiaGu.Resources.Archives
{
    /// <summary>
    /// 从存档和本地文件读取对应资源;
    /// </summary>
    public interface IArchiveSerializer<T>
    {
        void Serialize(Archive archive, T result);
        T Deserialize(Archive archive);
    }
}
