using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JiongXiaGu.Unity.Resources.Binding
{

    /// <summary>
    /// 提供反射成员方法;
    /// </summary>
    public class ReflectionImporter
    {
        public const BindingFlags FieldBindingFlags = BindingFlags.Instance | BindingFlags.Public;
        public const BindingFlags PropertyBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// 枚举所有成员;
        /// </summary>
        public static IEnumerable<IMember> EnumerateMembers(Type type, BindingFlags bindingAttr)
        {
            return EnumerateFields(type, bindingAttr).Concat(EnumerateProperties(type, bindingAttr));
        }

        private static IEnumerable<IMember> EnumerateFields(Type type, BindingFlags bindingAttr)
        {
            var fields = type.GetFields(bindingAttr);
            foreach (var field in fields)
            {
                if (!field.IsLiteral && !field.IsInitOnly)
                {
                    IMember member = null;
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
                                member = new Field(assetAttribute, field);
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

        private static IEnumerable<IMember> EnumerateProperties(Type type, BindingFlags bindingAttr)
        {
            var properties = type.GetProperties(bindingAttr);
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    IMember member = null;
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
                                member = new Property(assetAttribute, property);
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



        /// <summary>
        /// 获取该类型成员信息;
        /// </summary>
        public static List<IMember> BuildMembers(Type type)
        {
            var members = new List<IMember>();

            var fields = type.GetFields(FieldBindingFlags);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<AssetAttribute>();
                if (attribute != null)
                {
                    var member = new Field(attribute, field);
                    members.Add(member);
                }
            }

            var properties = type.GetProperties(PropertyBindingFlags);
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    var attribute = property.GetCustomAttribute<AssetAttribute>();
                    if (attribute != null)
                    {
                        var member = new Property(attribute, property);
                        members.Add(member);
                    }
                }
            }

            return members;
        }
    }
}
