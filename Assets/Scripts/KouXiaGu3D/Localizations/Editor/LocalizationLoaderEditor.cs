using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    [CustomEditor(typeof(LocalizationLoader), true)]
    class LocalizationLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("SystemLanguage", Localization.SystemLanguage.ToString());
        }
    }

}
