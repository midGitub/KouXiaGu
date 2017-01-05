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

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("SystemLanguage", Localization.SystemLanguage.ToString());
            EditorGUILayout.LabelField("文本量", Localization.TextDictionary.Count.ToString());

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("输出测试"))
            {
                Output();
            }

            if (GUILayout.Button("输入测试"))
            {
                Input();
            }

            EditorGUILayout.EndHorizontal();
        }


        static readonly TextPack[] Templet = new TextPack[]
        {
            new TextPack("Test_1", "测试1", false),
            new TextPack("Test_2", "测试2", false),
            new TextPack("Test_3", "测试3", true),
            new TextPack("Test_4", "测试4", true),
        };

        string TempletFilePath
        {
            get { return Path.Combine(Localization.ResPath, "Test.xml"); }
        }

        void Output()
        {
            XmlFile writer = new XmlFile(TempletFilePath);
            writer.WriteTexts(SystemLanguage.ChineseSimplified.ToString(), Templet);
        }

        void Input()
        {
            XmlFile writer = new XmlFile(TempletFilePath);
            var texts = writer.ReadTexts().ToArray();
            Debug.Log(texts.ToLog());
        }

    }

}
