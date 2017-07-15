using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace KouXiaGu.UI
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(SelectablePanel), true)]
    public class SelectablePanelEditor : Editor
    {
        SelectablePanelEditor()
        {
        }

        SelectablePanel ins
        {
            get { return target as SelectablePanel; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            string log = SelectablePanel.Stack.ToLog();
            EditorGUILayout.LabelField(log);
        }
    }
}
