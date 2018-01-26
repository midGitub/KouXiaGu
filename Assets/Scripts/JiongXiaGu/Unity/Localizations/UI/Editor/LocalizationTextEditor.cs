using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace JiongXiaGu.Unity.Localizations.UI
{

    [CustomEditor(typeof(LocalizationText), true)]
    public class LocalizationTextEditor : Editor
    {
        private LocalizationText Target => target as LocalizationText;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Key : ", Target.Key);
            EditorGUILayout.TextField("Value : ", "");

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Refresh"))
            {
            }
            if (GUILayout.Button("Cancel"))
            {
            }
            if (GUILayout.Button("Apply"))
            {
            }

            EditorGUILayout.EndHorizontal();
        }

    }
}
