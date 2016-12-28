using System;
using System.Linq;
using UnityEngine;

namespace KouXiaGu
{

#if UNITY_EDITOR

    using UnityEditor;

    /// <summary>
    /// 编辑器类,用于重写 MonoBehaviour 的编辑器(仅在存在 CustomEditorAttribute 特性的类上有效);
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : Editor
    {
        protected MonoBehaviourEditor() { }

        PropertyEditorAttribute.Property[] properties;

        public void OnEnable()
        {
            var editorClass = Attribute.GetCustomAttribute(target.GetType(), typeof(CustomEditorToolAttribute));

            if (editorClass != null)
            {
                properties = PropertyEditorAttribute.GetProperties(target).ToArray();
            }
            //Debug.Log(isEditorClass + " " + target.GetType().Name + " " + properties.Length);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PropertyGUI();
        }

        /// <summary>
        /// PropertyEditorAttribute
        /// </summary>
        public void PropertyGUI()
        {
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    property.OnGUI();
                }
            }
        }
    }
#endif


}
