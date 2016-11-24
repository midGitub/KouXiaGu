using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    /// <summary>
    /// Object 实际的类型;
    /// </summary>
    public enum ObjectType
    {
        Unbeknown = -1,
        [ObjectType(typeof(int))]
        Integer = 0,
        [ObjectType(typeof(bool))]
        Boolean = 1,
        [ObjectType(typeof(float))]
        Float = 2,
        [ObjectType(typeof(string))]
        String = 3,
        [ObjectType(typeof(Color))]
        Color = 4,
        [ObjectType(typeof(UnityEngine.Object))]
        ObjectReference = 5,
        [ObjectType(typeof(LayerMask))]
        LayerMask = 6,
        [ObjectType(typeof(Enum))]
        Enum = 7,
        [ObjectType(typeof(Vector2))]
        Vector2 = 8,
        [ObjectType(typeof(Vector3))]
        Vector3 = 9,
        [ObjectType(typeof(Vector4))]
        Vector4 = 10,
        [ObjectType(typeof(Rect))]
        Rect = 11,
        ArraySize = 12,
        Character = 13,
        [ObjectType(typeof(AnimationEvent))]
        AnimationCurve = 14,
        [ObjectType(typeof(Bounds))]
        Bounds = 15,
        Gradient = 16,
        [ObjectType(typeof(Quaternion))]
        Quaternion = 17
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ObjectTypeAttribute : Attribute
    {
        public ObjectTypeAttribute(Type type)
        {
            this.FieldType = type;
        }

        public Type FieldType { get; private set; }
    }

    /// <summary>
    /// 反射拓展;
    /// </summary>
    public static class Reflection
    {

        private static readonly Dictionary<Type, ObjectType> ObjectTypeDictionary = GetObjectTypeDictionary();

        /// <summary>
        /// 反射获取到对应类型;
        /// </summary>
        private static Dictionary<Type, ObjectType> GetObjectTypeDictionary()
        {
            Type dictionaryKey;
            ObjectType dictionaryValue;
            FieldInfo[] fieldInfos = typeof(ObjectType).GetFields(BindingFlags.Public | BindingFlags.Static);
            Dictionary<Type, ObjectType> dictionary = new Dictionary<Type, ObjectType>(fieldInfos.Length);

            foreach (var fieldInfo in fieldInfos)
            {
                var attribute = (ObjectTypeAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(ObjectTypeAttribute));
                if (attribute != null)
                {
                    dictionaryKey = attribute.FieldType;
                    if (!dictionary.ContainsKey(dictionaryKey))
                    {
                        dictionaryValue = (ObjectType)fieldInfo.GetValue(null);
                        dictionary.Add(dictionaryKey, dictionaryValue);
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 获取到物体类型(不完全的);
        /// </summary>
        public static ObjectType GetObjectType(this Type type)
        {
            //ObjectType objectType;
            //if (ObjectTypeDictionary.TryGetValue(type, out objectType))
            //{
            //    return objectType;
            //}
            foreach (var pair in ObjectTypeDictionary)
            {
                if (type.IsSubclassOf(pair.Key) || type == pair.Key)
                {
                    return pair.Value;
                }
            }

            return ObjectType.Unbeknown;
        }

    }
}
