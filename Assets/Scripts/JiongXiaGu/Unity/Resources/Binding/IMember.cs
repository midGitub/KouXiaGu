using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources.Binding
{

    public interface IMember
    {
        string Name { get; }
        string RelativePath { get; }
        string Message { get; }
        string Tags { get; }
        ISerializer Serializer { get; }
        object GetValue(object instance);
        void SetValue(object instance, object value);
    }

    public struct Property : IMember
    {
        public AssetAttribute Info { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }
        public ISerializer Serializer { get; private set; }
        public string Name => PropertyInfo.Name;
        public string RelativePath => Info.RelativePath;
        public string Message => Info.Message;
        public string Tags => Info.Tags;

        public Property(AssetAttribute info, PropertyInfo propertyInfo)
        {
            Info = info;
            PropertyInfo = propertyInfo;
            Serializer = info.CreateSerializer(propertyInfo.PropertyType);
        }

        public object GetValue(object instance)
        {
            return PropertyInfo.GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            PropertyInfo.SetValue(instance, value);
        }
    }

    public struct Field : IMember
    {
        public AssetAttribute Info { get; private set; }
        public FieldInfo FieldInfo { get; private set; }
        public ISerializer Serializer { get; private set; }
        public string Name => FieldInfo.Name;
        public string RelativePath => Info.RelativePath;
        public string Message => Info.Message;
        public string Tags => Info.Tags;

        public Field(AssetAttribute assetAttribute, FieldInfo fieldInfo)
        {
            Info = assetAttribute;
            FieldInfo = fieldInfo;
            Serializer = assetAttribute.CreateSerializer(fieldInfo.FieldType);
        }

        public object GetValue(object instance)
        {
            return FieldInfo.GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            FieldInfo.SetValue(instance, value);
        }
    }

}
