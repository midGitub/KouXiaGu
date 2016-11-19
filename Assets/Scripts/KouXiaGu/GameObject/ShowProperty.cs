using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu
{

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public abstract class PropertyEditorAttribute : Attribute
    {
        public PropertyEditorAttribute()
        {
            this.Label = string.Empty;
        }
        public PropertyEditorAttribute(string label)
        {
            this.Label = Label;
        }

        private const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// 显示的标签;
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// 渲染到检视面板的方法;
        /// </summary>
        public abstract void OnGUI(Property property);

        /// <summary>
        /// 获取到所有添加此特性的可读或可写的属性;
        /// </summary>
        public static IEnumerable<Property> GetProperties(object instance, BindingFlags bindingAttr = DefaultBindingFlags)
        {
            PropertyInfo[] propertyInfos = instance.GetType().GetProperties(bindingAttr);

            foreach (var propertyInfo in propertyInfos)
            {
                if (!propertyInfo.CanRead && !propertyInfo.CanWrite)
                    continue;

                Attribute[] attributes = GetCustomAttributes(propertyInfo, typeof(PropertyEditorAttribute));

                foreach (PropertyEditorAttribute attribute in attributes)
                {
                    Property property = new Property(instance, propertyInfo, attribute);
                    yield return property;
                }
            }
        }


        public class Property
        {
            public Property(object instance, PropertyInfo propertyInfo, PropertyEditorAttribute attribute)
            {
                this.Instance = instance;
                this.Attribute = attribute;

                this.Getter = propertyInfo.GetGetMethod(true);
                this.Setter = propertyInfo.GetSetMethod(true);
                this.PropertyType = EditorReflection.GetPropertyType(propertyInfo.PropertyType);
                this.Label = GetLabel(propertyInfo);
            }
            
            public PropertyType PropertyType { get; private set; }
            public object Instance { get; private set; }
            public string Label { get; private set; }
            private PropertyEditorAttribute Attribute { get; set; }
            private MethodInfo Getter { get; set; }
            private MethodInfo Setter { get; set; }

            public object Value
            {
                get { return Getter.Invoke(Instance, null); }
                set { Setter.Invoke(Instance, new object[] { value }); }
            }

            /// <summary>
            /// 渲染到检视面板的方法;
            /// </summary>
            public void OnGUI()
            {
                Attribute.OnGUI(this);
            }

            private string GetLabel(PropertyInfo propertyInfo)
            {
                return Attribute.Label == string.Empty ?
                    propertyInfo.Name :
                    Attribute.Label;
            }
        }
    }

    /// <summary>
    /// 在检视面板显示这个属性;
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ShowOnlyPropertyAttribute : PropertyEditorAttribute
    {
        public ShowOnlyPropertyAttribute() : base() { }
        public ShowOnlyPropertyAttribute(string label) : base(label) { }

        public override void OnGUI(Property property)
        {
            EditorGUILayout.LabelField(property.Label, property.Value.ToString());
        }
    }

    /// <summary>
    /// 在检视面板暴露这个属性;
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ExposePropertyAttribute : PropertyEditorAttribute
    {
        public ExposePropertyAttribute() : base() { }
        public ExposePropertyAttribute(string label) : base(label) { }

        public override void OnGUI(Property property)
        {
            switch (property.PropertyType)
            {
                case PropertyType.Integer:
                    property.Value = EditorGUILayout.IntField(property.Label, (int)property.Value);
                    break;
                case PropertyType.Float:
                    property.Value = EditorGUILayout.FloatField(property.Label, (float)property.Value);
                    break;
                default:
                    EditorGUILayout.LabelField("未知的类型");
                    break;
            }
        }
    }

    [CustomEditor(typeof(ComponentClone), true)]
    public class ShowPropertyEditor : Editor
    {

        private PropertyEditorAttribute.Property[] properties;

        public virtual void OnEnable()
        {
            properties = PropertyEditorAttribute.GetProperties(target).ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PropertyGUI();
        }

        protected virtual void PropertyGUI()
        {
            foreach (var property in properties)
            {
                property.OnGUI();
            }
        }

    }

    public enum PropertyType
    {
        Generic = -1,
        Integer = 0,
        Boolean = 1,
        Float = 2,
        String = 3,
        Color = 4,
        ObjectReference = 5,
        //LayerMask = 6,
        //Enum = 7,
        //Vector2 = 8,
        //Vector3 = 9,
        //Vector4 = 10,
        //Rect = 11,
        //ArraySize = 12,
        //Character = 13,
        //AnimationCurve = 14,
        //Bounds = 15,
        //Gradient = 16,
        //Quaternion = 17
    }

    public static class EditorReflection
    {

        /// <summary>
        /// 获取到物体类型;
        /// </summary>
        public static PropertyType GetPropertyType(Type type)
        {
            if (type == typeof(int))
                return PropertyType.Integer;

            if (type == typeof(bool))
                return PropertyType.Boolean;

            if (type == typeof(float))
                return PropertyType.Float;

            if (type == typeof(string))
                return PropertyType.String;

            if (type == typeof(Color))
                return PropertyType.Color;

            else
                return PropertyType.Generic;
        }
    }


}
