using UnityEditor;
using UnityEngine;

namespace JiongXiaGu.Grids
{

    [CustomPropertyDrawer(typeof(CubicHexCoord))]
    public class CubicHexCoordDrawer : PropertyDrawer
    {
        /// <summary>
        /// 显示的变量数目;
        /// </summary>
        const int FIELD_COUNT = 3;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * FIELD_COUNT;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty x = property.FindPropertyRelative("x");
            SerializedProperty y = property.FindPropertyRelative("y");
            SerializedProperty z = property.FindPropertyRelative("z");


            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float textOffset = 20;

            float pX = position.x + textOffset;
            float width = position.width - textOffset;
            float height = position.height / FIELD_COUNT - 1;

            // Calculate rects
            var Point_x = new Rect(pX, position.y, width, height);
            var Point_y = new Rect(pX, position.y + (position.height / FIELD_COUNT), width, height);
            var Point_z = new Rect(pX, position.y + (position.height / FIELD_COUNT) * 2, width, height);

            var text_x = new Rect(position.x, position.y, textOffset, height);
            var text_y = new Rect(position.x, position.y + (position.height / FIELD_COUNT), textOffset, height);
            var text_z = new Rect(position.x, position.y + (position.height / FIELD_COUNT) * 2, textOffset, height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.LabelField(text_x, "X");
            EditorGUI.PropertyField(Point_x, x, GUIContent.none);

            EditorGUI.LabelField(text_y, "Y");
            EditorGUI.PropertyField(Point_y, y, GUIContent.none);

            short ValueZ = (short)(-x.intValue - y.intValue);
            z.intValue = ValueZ;

            EditorGUI.LabelField(text_z, "Z");
            EditorGUI.LabelField(Point_z, ValueZ.ToString());

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

    }

}
