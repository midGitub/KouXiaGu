using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace KouXiaGu.World
{


    [CustomEditor(typeof(TimeManager)), CanEditMultipleObjects]
    public class WorldTimerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targt = (TimeManager)this.target;

            EditorGUILayout.LabelField(targt.CurrentTime.ToString());
            EditorGUILayout.LabelField("Hour:" + targt.CurrentTime.Hour);

            Time.timeScale = EditorGUILayout.FloatField("TimeScale", Time.timeScale);

        }


    }

}
