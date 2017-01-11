using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace KouXiaGu.Initialization
{

    [CustomEditor(typeof(Scheduler), true)]
    public class SchedulerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Scheduler target = (Scheduler)this.target;

            EditorGUILayout.LabelField("等待总数:" + target.Total);
            EditorGUILayout.LabelField("剩余总数:" + target.Remainder);
        }

    }

}
