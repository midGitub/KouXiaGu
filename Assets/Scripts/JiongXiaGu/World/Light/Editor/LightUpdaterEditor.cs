using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JiongXiaGu.World
{

    [CustomEditor(typeof(LightingUpdater))]
    [CanEditMultipleObjects]
    public class LightUpdaterEditor : Editor
    {

        static int selectLightStyleIndex;
        SerializedProperty lightStylesProp;

        LightingUpdater Target
        {
            get { return (LightingUpdater)target; }
        }

        void OnEnable()
        {
            if (Target.LightStyles == null)
            {
                Target.LightStyles = new LightOfTime[24];
            }
            lightStylesProp = serializedObject.FindProperty("lightStyles");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            selectLightStyleIndex = EditorGUILayout.IntField("0~23", selectLightStyleIndex);
            if (GUILayout.Button("设置当前状态到.."))
            {
                LightOfTime current = Target.GetCurrentState();
                var item = lightStylesProp.GetArrayElementAtIndex(selectLightStyleIndex);
                item.FindPropertyRelative("SunIntensity").floatValue = current.SunIntensity;
                item.FindPropertyRelative("SunRotation").vector3Value = current.SunRotation;
                item.FindPropertyRelative("GlobalIntensity").floatValue = current.GlobalIntensity;
                item.FindPropertyRelative("Skybox").objectReferenceValue = current.Skybox;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
