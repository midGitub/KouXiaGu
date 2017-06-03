using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(LandformUnityDispatcher))]
    [CanEditMultipleObjects]
    public class LandformUnityDispatcherEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var target = (LandformUnityDispatcher)this.target;

            EditorGUILayout.LabelField("RequestCount:" + target.RequestCount);
        }

    }

}
