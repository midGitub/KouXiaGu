using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{

#if UNITY_EDITOR
    using UnityEditor;

    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    internal class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }
    }
#endif

    /// <summary>
    /// 将枚举变量置为多选窗口;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal class EnumFlagsAttribute : PropertyAttribute
    {
    }
}
