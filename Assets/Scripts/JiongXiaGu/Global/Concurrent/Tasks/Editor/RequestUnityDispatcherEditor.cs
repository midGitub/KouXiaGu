using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace JiongXiaGu.Concurrent
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(RequestUnityDispatcher), true)]
    public class RequestUnityDispatcherEditor : Editor
    {
        RequestUnityDispatcher Target
        {
            get { return target as RequestUnityDispatcher; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("RequestCount:" + Target.Count);
        }
    }
}
