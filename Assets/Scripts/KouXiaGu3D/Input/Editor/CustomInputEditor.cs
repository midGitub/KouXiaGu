
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;

namespace KouXiaGu.EditorTool
{

    [CustomEditor(typeof(CustomInput), true)]
    public class CustomInputEditor : Editor
    {

        static readonly KeyFunction[] Functions = Enum.GetValues(typeof(KeyFunction)).Cast<KeyFunction>().ToArray();

        bool isLoad = false;

        private void OnEnable()
        {
            if (!isLoad)
            {
                CustomInput.Load();
            }
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            foreach(var item in Functions)
            {
                KeyCode keycode;
                try
                {
                    keycode = CustomInput.GetKey(item);
                }
                catch (KeyNotFoundException)
                {
                    keycode = KeyCode.None;
                }
                CustomInput.SetKey(item, (KeyCode)EditorGUILayout.EnumPopup(item.ToString(), keycode));
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
            {
                CustomInput.Save();
            }

            if (GUILayout.Button("Load"))
            {
                CustomInput.Load();
            }

            GUILayout.EndHorizontal();
        }

    }

}

