using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Globalization
{

    [CustomEditor(typeof(LocalizationInitializer), true)]
    class LocalizationEditor : Editor
    {

        static readonly TextItem[] Templet = new TextItem[]
        {
            new TextItem("Test_1", "测试1", false),
            new TextItem("Test_2", "测试2", false),
            new TextItem("Test_3", "测试3", true),
            new TextItem("Test_4", "测试4", true),
        };

        static readonly Culture language = new Culture("简中", "zh-CN");

        string TempletFilePath
        {
            get { return Path.Combine(Resources.DirectoryPath, "Test.xml"); }
        }

        string LackingKeysFilePath
        {
            get { return Path.Combine(Resources.DirectoryPath, "LackingKeys.xml"); }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (LocalizationText.IsInitialized)
            {
                EditorGUILayout.LabelField("文本量", LocalizationText.TextDictionary.Count.ToString());
                EditorGUILayout.LabelField("缺失文本量", LackingTextCollecter.LackingKeyCount.ToString());
            }

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("保存配置"))
            {
                Localization.SetLanguage(new Localization("简体中文"));
            }

            if (GUILayout.Button("输出语言"))
            {
                Debug.Log(Resources.FindLanguageFiles(Resources.DirectoryPath).ToLog("目录下的语言文件"));
            }

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("测试输出"))
            {
                XmlFile.CreateTexts(TempletFilePath, language, Templet);
            }

            if (GUILayout.Button("测试输入"))
            {
                var texts = XmlFile.ReadTexts(TempletFilePath);
                Debug.Log(texts.ToLog());
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("测试添加"))
            {
                XmlFile.AppendTexts(TempletFilePath, Templet);
            }

            if (GUILayout.Button("输出缺失"))
            {
                XmlFile.CreateKeys(LackingKeysFilePath, language, LackingTextCollecter.LackingKeys);
            }

            EditorGUILayout.EndHorizontal();


        }

    }

}
