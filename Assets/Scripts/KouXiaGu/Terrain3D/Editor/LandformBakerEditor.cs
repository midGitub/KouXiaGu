using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(LandformBaker))]
    [CanEditMultipleObjects]
    public class LandformBakerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var target = (LandformBaker)this.target;

            EditorGUILayout.LabelField("RequestCount:" + target.RequestCount);
        }

    }

}
