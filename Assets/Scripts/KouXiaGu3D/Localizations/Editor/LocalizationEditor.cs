using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Localizations
{

    [CustomEditor(typeof(Localization), true)]
    class LocalizationEditor : Editor
    {

        static readonly TextItem[] Templet = new TextItem[]
        {
            new TextItem("Test_1", "测试1", false),
            new TextItem("Test_2", "测试2", false),
            new TextItem("Test_3", "测试3", true),
            new TextItem("Test_4", "测试4", true),
        };

        string TempletFilePath
        {
            get { return Path.Combine(Resources.ResDirectoryPath, "Test.xml"); }
        }

        string LackingKeysFilePath
        {
            get { return Path.Combine(Resources.ResDirectoryPath, "LackingKeys.xml"); }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("SystemLanguage", LocalizationConfig.SysLanguage);
            EditorGUILayout.LabelField("文本量", Localization.TextDictionary.Count.ToString());
            EditorGUILayout.LabelField("缺失文本量", LackingTextCollecter.LackingKeyCount.ToString());

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("保存配置"))
            {
                LocalizationConfig.Write(Localization.Config);
            }

            if (GUILayout.Button("输出语言"))
            {
                Debug.Log(Resources.FindLanguageFiles(Resources.ResDirectoryPath).ToLog("目录下的语言文件"));
            }

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("测试输出"))
            {
                XmlFiler.CreateTexts(TempletFilePath, LocalizationConfig.SysLanguage, Templet);
            }

            if (GUILayout.Button("测试输入"))
            {
                var texts = XmlFiler.ReadTexts(TempletFilePath);
                Debug.Log(texts.ToLog());
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("测试添加"))
            {
                XmlFiler.AppendTexts(TempletFilePath, Templet);
            }

            if (GUILayout.Button("输出缺失"))
            {
                XmlFiler.CreateKeys(LackingKeysFilePath, LocalizationConfig.SysLanguage, LackingTextCollecter.LackingKeys);
            }

            EditorGUILayout.EndHorizontal();


        }

    }

}
