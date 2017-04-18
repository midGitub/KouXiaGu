using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace KouXiaGu.Rx
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityThreadDispatcher), true)]
    public class UnityThreadDispatcherEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (UnityThreadDispatcher.IsInitialized)
            {
                UnityThreadDispatcher target = (UnityThreadDispatcher)this.target;
                EditorGUILayout.LabelField("Update:" + target.UpdateObserverCount);
                EditorGUILayout.LabelField("FixedUpdate:" + target.FixedUpdateObserverCount);
            }
        }

    }

}
