
namespace KouXiaGu.World2D
{
    using Grids;

#if UNITY_EDITOR

    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(RectCoord))]
    public class ShortVector2Drawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var Point_x = new Rect(position.x, position.y, position.x / 2- 8, position.height);
            var Point_y = new Rect(position.x + position.x / 2 - 6, position.y, position.x / 2 - 8, position.height);

            
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(Point_x, property.FindPropertyRelative("x"), GUIContent.none);
            EditorGUI.PropertyField(Point_y, property.FindPropertyRelative("y"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();

        }

    }
#endif

}
