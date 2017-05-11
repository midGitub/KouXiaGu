using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityStandardAssets.Water;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(WaterManager))]
    [CanEditMultipleObjects]
    class WaterManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var target = (WaterManager)this.target;

            target.IsDisplay = EditorGUILayout.Toggle("IsDisplay", target.IsDisplay);
            target.WaterMode = (Water.WaterMode)EditorGUILayout.EnumPopup("WaterMode", target.WaterMode);
            target.Size = EditorGUILayout.FloatField("Size", target.Size);

            //if (Application.isPlaying)
            //{
            //    target.WaterMode = (Water.WaterMode)EditorGUILayout.EnumPopup("WaterMode", target.WaterMode);
            //}
        }

    }

}
