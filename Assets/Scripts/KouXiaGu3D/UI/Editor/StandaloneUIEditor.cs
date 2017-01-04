using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.UI
{

    [CustomEditor(typeof(StandaloneUI), true)]
    public class StandaloneUIEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            StandaloneUI target = (StandaloneUI)this.target;

            EditorGUILayout.Toggle("IsDisplay(readOnly)", target.IsDisplay());

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("显示"))
            {
                target.Display();
            }

            if (GUILayout.Button("隐藏"))
            {
                target.Conceal();
            }

            GUILayout.EndHorizontal();

        }

    }

}
