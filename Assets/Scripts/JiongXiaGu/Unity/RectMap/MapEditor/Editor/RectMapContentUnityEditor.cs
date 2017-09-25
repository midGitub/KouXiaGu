using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor;

namespace JiongXiaGu.World.RectMap.MapEditor
{


    [CustomEditor(typeof(RectMapContentEditor))]
    class RectMapContentUnityEditor : Editor
    {
        RectMapContentUnityEditor()
        {
        }

        RectMapContentEditor Target
        {
            get { return target as RectMapContentEditor; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Target.IsCompleted)
            {
            }
        }
    }
}
