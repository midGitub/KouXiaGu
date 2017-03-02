using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace KouXiaGu.World
{


    [CustomEditor(typeof(WorldTimer)), CanEditMultipleObjects]
    public class WorldTimerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targt = (WorldTimer)this.target;

            EditorGUILayout.LabelField(targt.CurrentSimplifiedDateTime.ToString());
            EditorGUILayout.LabelField("Hour:" + targt.CurrentDateTime.Hour);
            EditorGUILayout.LabelField("Minute:" + targt.CurrentDateTime.Minute);

        }


    }

}
