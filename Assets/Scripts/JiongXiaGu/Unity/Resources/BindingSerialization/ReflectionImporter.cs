using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JiongXiaGu.Unity.Resources.BindingSerialization
{

    /// <summary>
    /// 提供反射成员方法;
    /// </summary>
    public static class ReflectionImporter
    {
        public const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// 枚举所有成员;
        /// </summary>
        public static IEnumerable<ISerializedMember> EnumerateMembers(Type type, BindingFlags bindingAttr = DefaultBindingFlags)
        {
            return EnumerateFields(type, bindingAttr).Concat(EnumerateProperties(type, bindingAttr));
        }

        public static IEnumerable<ISerializedMember> EnumerateFields(Type type, BindingFlags bindingAttr)
        {
            var fields = type.GetFields(bindingAttr);
            foreach (var field in fields)
            {
                if (!field.IsLiteral && !field.IsInitOnly)
                {
                    ISerializedMember member = null;
                    var attributes = field.GetCustomAttributes();
                    foreach (var attribute in attributes)
                    {
                        if (attribute is ObsoleteAttribute)
                        {
                            break;
                        }
                        else
                        {
                            var assetAttribute = attribute as AssetAttribute;
                            if (attribute != null)
                            {
                                member = new SerializedField(assetAttribute, field);
                            }
                        }
                    }

                    if (member != null)
                    {
                        yield return member;
                    }
                }
            }
        }

        public static IEnumerable<ISerializedMember> EnumerateProperties(Type type, BindingFlags bindingAttr)
        {
            var properties = type.GetProperties(bindingAttr);
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    ISerializedMember member = null;
                    var attributes = property.GetCustomAttributes();
                    foreach (var attribute in attributes)
                    {
                        if (attribute is ObsoleteAttribute)
                        {
                            break;
                        }
                        else
                        {
                            var assetAttribute = attribute as AssetAttribute;
                            if (attribute != null)
                            {
                                member = new SerializedProperty(assetAttribute, property);
                            }
                        }
                    }

                    if (member != null)
                    {
                        yield return member;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 线程安全;
    /// </summary>
    public struct SerializedProperty : ISerializedMember
    {
        public AssetAttribute Info { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }
        public string Name => PropertyInfo.Name;
        public string RelativePath => Info.RelativePath;
        public string Message => Info.Message;
        public string Tags => Info.Tags;
        public bool IsNecessaryAsset => Info.IsNecessaryAsset;

        public SerializedProperty(AssetAttribute info, PropertyInfo propertyInfo)
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

    /// <summary>
    /// 线程安全;
    /// </summary>
    public struct SerializedField : ISerializedMember
    {
        public AssetAttribute Info { get; private set; }
        public FieldInfo FieldInfo { get; private set; }
        public string Name => FieldInfo.Name;
        public string RelativePath => Info.RelativePath;
        public string Message => Info.Message;
        public string Tags => Info.Tags;
        public bool IsNecessaryAsset => Info.IsNecessaryAsset;

        public SerializedField(AssetAttribute assetAttribute, FieldInfo fieldInfo)
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
