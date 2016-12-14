using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace KouXiaGu.EditorTool
{

    [CustomEditor(typeof(CustomInput), true)]
    public class CustomInputEditor : Editor
    {

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

            foreach(var item in CustomInput.Functions)
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
