using System.Reflection;

namespace JiongXiaGu.Unity.Resources.BindingSerialization
{

    public interface ISerializedMember
    {
        string Name { get; }
        string RelativePath { get; }
        string Message { get; }
        string Tags { get; }
        bool IsNecessaryAsset { get; }
        object GetValue(object instance);
        void SetValue(object instance, object value);
        ISerializer CreateSerializer();
    }
}
