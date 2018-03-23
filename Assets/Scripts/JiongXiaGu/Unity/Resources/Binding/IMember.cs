using System.Reflection;

namespace JiongXiaGu.Unity.Resources.Binding
{

    public interface IMember
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

    public struct Property : IMember
    {
        public AssetAttribute Info { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }
        public string Name => PropertyInfo.Name;
        public string RelativePath => Info.RelativePath;
        public string Message => Info.Message;
        public string Tags => Info.Tags;
        public bool IsNecessaryAsset => Info.IsNecessaryAsset;

        public Property(AssetAttribute info, PropertyInfo propertyInfo)
        {
            Info = info;
            PropertyInfo = propertyInfo;
        }

        public object GetValue(object instance)
        {
            return PropertyInfo.GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            PropertyInfo.SetValue(instance, value);
        }

        public ISerializer CreateSerializer()
        {
            var serializer = Info.CreateSerializer(PropertyInfo.PropertyType);
            return serializer;
        }
    }

    public struct Field : IMember
    {
        public AssetAttribute Info { get; private set; }
        public FieldInfo FieldInfo { get; private set; }
        public string Name => FieldInfo.Name;
        public string RelativePath => Info.RelativePath;
        public string Message => Info.Message;
        public string Tags => Info.Tags;
        public bool IsNecessaryAsset => Info.IsNecessaryAsset;

        public Field(AssetAttribute assetAttribute, FieldInfo fieldInfo)
        {
            Info = assetAttribute;
            FieldInfo = fieldInfo;
        }

        public object GetValue(object instance)
        {
            return FieldInfo.GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            FieldInfo.SetValue(instance, value);
        }

        public ISerializer CreateSerializer()
        {
            var serializer = Info.CreateSerializer(FieldInfo.FieldType);
            return serializer;
        }
    }

}
