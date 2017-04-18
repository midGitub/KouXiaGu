
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;
using KouXiaGu.KeyInput;

namespace KouXiaGu.XiaGuTools
{

    public sealed class CustomInputWindow : EditorWindow
    {

        CustomInputWindow()
        {
            titleContent = new GUIContent("CustomInput");
            minSize = new Vector2(280f, 380f);
        }

        [MenuItem(XiaGuTool.MenuName + "/CustomInput")]
        static void Init()
        {
            CustomInputWindow window = (CustomInputWindow)GetWindow(typeof(CustomInputWindow));
            window.Show();
        }

        void OnGUI()
        {
            const float interval = 5f;

            EditorGUILayout.BeginVertical();

            foreach (var functionKey in CustomInput.FunctionKeys)
            {
                EditorGUILayout.BeginHorizontal();

                KeyCode keycode;
                keycode = CustomInput.GetKey(functionKey);
                EditorGUILayout.LabelField(functionKey.ToString());
                KeyCode keycode2 = (KeyCode)EditorGUILayout.EnumPopup(keycode, GUILayout.MaxWidth(160f));
                if (keycode != keycode2)
                    CustomInput.UpdateKey(functionKey, keycode2);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(interval);

            const float btnHeight = 24f;

            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = XiaGuTool.green;
            if (GUILayout.Button("ReadFromFile", GUILayout.Height(btnHeight)))
            {
                ReadFromFile();
            }
            GUI.backgroundColor = XiaGuTool.red;
            if (GUILayout.Button("WirteToFile", GUILayout.Height(btnHeight)))
            {
                WirteToFile();
            }

            EditorGUILayout.EndHorizontal();

        }

        static void ReadFromFile()
        {
            CustomInput.ReadOrDefaultAsync();
        }

        static void WirteToFile()
        {
            CustomInput.WriteAsync();
        }

    }



    //[CustomEditor(typeof(CustomInput), true)]
    //public class CustomInputEditor : Editor
    //{

    //    static readonly KeyFunction[] Functions = Enum.GetValues(typeof(KeyFunction)).Cast<KeyFunction>().ToArray();

    //    bool isLoad = false;

    //    private void OnEnable()
    //    {
    //        if (!isLoad)
    //        {
    //            CustomInput.Load();
    //        }
    //    }

    //    public override void OnInspectorGUI()
    //    {
    //        //base.OnInspectorGUI();

    //        foreach (var item in Functions)
    //        {
    //            KeyCode keycode;
    //            try
    //            {
    //                keycode = CustomInput.GetKey(item);
    //            }
    //            catch (KeyNotFoundException)
    //            {
    //                keycode = KeyCode.None;
    //            }
    //            CustomInput.SetKeyNotSave(item, (KeyCode)EditorGUILayout.EnumPopup(item.ToString(), keycode));
    //        }

    //        GUILayout.BeginHorizontal();

    //        if (GUILayout.Button("Save"))
    //        {
    //            CustomInput.Save();
    //        }

    //        if (GUILayout.Button("Load"))
    //        {
    //            CustomInput.Load();
    //        }

    //        GUILayout.EndHorizontal();
    //    }

    //}

}

