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
        /// <summary>
        /// 信息;
        /// </summary>
        AssetAttribute Info { get; }

        /// <summary>
        /// 成员名称;
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 序列化接口;
        /// </summary>
        ISerializer Serializer { get; }

        /// <summary>
        /// 获取到值;
        /// </summary>
        object GetValue(object instance);

        /// <summary>
        /// 设置到值;
        /// </summary>
        void SetValue(object instance, object value);
    }

    public struct Property : IMember
    {
        public AssetAttribute Info { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }
        public ISerializer Serializer { get; private set; }
        public string Name => PropertyInfo.Name;

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
