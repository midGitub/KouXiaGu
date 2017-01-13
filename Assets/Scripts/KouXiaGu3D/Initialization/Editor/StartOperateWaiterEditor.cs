using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace KouXiaGu.Initialization
{

    [CustomEditor(typeof(StartOperateWaiter), true)]
    public class StartOperateWaiterEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            StartOperateWaiter target = (StartOperateWaiter)this.target;

            EditorGUILayout.LabelField("等待总数:" + target.Total);
            EditorGUILayout.LabelField("剩余总数:" + target.Remainder);
            EditorGUILayout.LabelField("等待中:", target.current != null ? target.current .GetType().Name : "Unknown");
            EditorGUILayout.LabelField("完成?", target.current != null ? target.current.IsCompleted.ToString() : "Unknown");
            EditorGUILayout.LabelField("失败?", target.current != null ? target.current.IsFaulted.ToString() : "Unknown");
        }

    }

}
