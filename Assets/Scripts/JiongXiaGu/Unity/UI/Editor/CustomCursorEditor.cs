//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEditor;

//namespace JiongXiaGu.UI
//{

//    [CanEditMultipleObjects]
//    [CustomEditor(typeof(CustomCursor), true)]
//    public class CustomCursorEditor : Editor
//    {
//        CustomCursorEditor()
//        {
//        }

//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            CustomCursor ins = target as CustomCursor;
//            EditorGUILayout.LabelField("订户总数:", ins.SubscriberCount.ToString());
//        }
//    }
//}
