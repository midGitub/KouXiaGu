using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace KouXiaGu.Rx
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityThreadDispatcher), true)]
    public class UnityThreadDispatcherEditor : Editor
    {

        public bool IsInitialized
        {
            get { return UnityThreadDispatcher.IsInitialized; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnityThreadDispatcher target = (UnityThreadDispatcher)this.target;
            if (IsInitialized)
            {
                Runtime(target);
            }
            else
            {
                DefaultOnInspectorGUI(target);
            }
        }

        void DefaultOnInspectorGUI(UnityThreadDispatcher target)
        {
            //EditorGUILayout.LabelField("Update:" + "0");
            //EditorGUILayout.LabelField("FixedUpdate:" + "0");
        }

        bool isShowUpdateInfo;
        bool isShowFixedUpdateInfo;

        void Runtime(UnityThreadDispatcher target)
        {
            const float width = 120f;
            const float interval = 8f;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Update:" + target.UpdateObserverCount, GUILayout.Width(width));
            Display(target.UpdateObservers, "ShowMore", ref isShowUpdateInfo);
            EditorGUILayout.EndVertical();

            GUILayout.Space(interval);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("FixedUpdate:" + target.FixedUpdateObserverCount, GUILayout.Width(width));
            Display(target.FixedUpdateObservers, "ShowMore", ref isShowFixedUpdateInfo);
            EditorGUILayout.EndVertical();
        }

        void Display<T>(IEnumerable<T> items, string label, ref bool toggle)
        {
            toggle = EditorGUILayout.BeginToggleGroup(label, toggle);
            if (toggle)
            {
                int i = 0;
                foreach (var item in items)
                {
                    string text = string.Format("[{0}]{1}", i++, item.GetType().FullName);
                    EditorGUILayout.LabelField(text);
                }
            }
            EditorGUILayout.EndVertical();
        }

    }

}
