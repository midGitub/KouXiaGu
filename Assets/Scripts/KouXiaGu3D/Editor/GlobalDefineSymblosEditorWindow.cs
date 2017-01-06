using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using KouXiaGu.Collections;

namespace KouXiaGu
{


    public class GlobalDefineSymblosEditorWindow : EditorWindow
    {

        GlobalDefineSymblosEditorWindow()
        {
            titleContent = new GUIContent("DefineSymblos");
            minSize = new Vector2(420f, 300f);
        }

        public const string DirName = "Editor\\DefineSymblos";
        public const string FileName = "DefineSymblos.xml";

        static readonly Color red = new Color(200f / 255f, 100f / 255f, 100f / 255f);
        static readonly Color green = new Color(150f / 255f, 210f / 255f, 110f / 255f);

        public const BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone;


        static readonly LinkedList<DefineSymblo> defineSymblos = new LinkedList<DefineSymblo>();


        public static string DirPath
        {
            get { return Path.Combine(Application.dataPath, DirName); }
        }

        public static string FilePath
        {
            get { return Path.Combine(DirPath, FileName); }
        }


        [MenuItem("Tools/DefineSymblos")]
        static void Init()
        {
            GlobalDefineSymblosEditorWindow window = (GlobalDefineSymblosEditorWindow)GetWindow(typeof(GlobalDefineSymblosEditorWindow));
            window.Show();
        }

        void OnEnable()
        {
            ReadFromFile();
        }

        void OnDisable()
        {
            WirteToFile();
        }

        void OnGUI()
        {
            const float interval = 5f;

            #region 调整;

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Original:", GUILayout.Width(65f));
            string original = GetDefinesFromPlayerSettings();
            EditorGUILayout.LabelField(original);

            EditorGUILayout.EndHorizontal();

            #endregion

            GUILayout.Space(interval);

            #region 自定义展示区;

            EditorGUILayout.BeginVertical();
            AutoAddOne();

            LinkedListNode<DefineSymblo> pointer = defineSymblos.First;
            while (pointer != null)
            {
                EditorGUILayout.BeginHorizontal();

                DefineSymblo defineSymblo = pointer.Value;

                GUI.backgroundColor = red;
                if (GUILayout.Button("Remove", GUILayout.Width(65f)))
                {
                    pointer = defineSymblos.RemoveAndReturnNext(pointer);
                }
                else
                {
                    pointer = pointer.Next;
                }
                GUI.backgroundColor = Color.white;


                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Tag:", GUILayout.MaxWidth(28f));
                defineSymblo.Tag = EditorGUILayout.TextField(defineSymblo.Tag, GUILayout.MaxWidth(222f));

                EditorGUILayout.LabelField("DefineSymblos:", GUILayout.MaxWidth(92f));
                string result = EditorGUILayout.TextField(defineSymblo.Define);

                EditorGUILayout.EndHorizontal();

                if (result != defineSymblo.Define)
                    defineSymblo.Define = Standardize(result);
                EditorGUILayout.EndHorizontal();
            }

            AutoRemoveEmpty();
            EditorGUILayout.EndVertical();

            #endregion

            GUILayout.Space(interval);

            #region 按钮区

            const float btnHeight = 24f;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Read", GUILayout.Height(btnHeight)))
            {
                ReadFromFile();
            }
            if (GUILayout.Button("Wirte", GUILayout.Height(btnHeight)))
            {
                WirteToFile();
            }

            GUI.backgroundColor = green;
            if (GUILayout.Button("SetTo", GUILayout.Height(btnHeight)))
            {
                SetDefinesToPlayerSettings();
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            #endregion

        }

        static void AutoAddOne()
        {
            if (defineSymblos.Count == 0)
                defineSymblos.AddLast(new DefineSymblo());
            else if (!defineSymblos.Last.Value.IsEmpty)
                defineSymblos.AddLast(new DefineSymblo());
        }

        static void AutoRemoveEmpty()
        {
            LinkedListNode<DefineSymblo> pointer = defineSymblos.First;
            while (pointer != null)
            {
                while (pointer.Value.IsEmpty &&
                    pointer.Next != null &&
                    pointer.Next.Value.IsEmpty)
                {
                    defineSymblos.Remove(pointer.Next);
                }

                pointer = pointer.Next;
            }
        }


        string GetDefinesFromPlayerSettings()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        }

        void SetDefinesToPlayerSettings()
        {
            string defines = defineSymblos.
                Select(item => item.Define).
                Aggregate((workingSentence, next) => workingSentence + next);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }


        static readonly List<char> newStr = new List<char>();

        static string Standardize(string defineSymblo)
        {
            if (string.IsNullOrEmpty(defineSymblo))
                return defineSymblo;

            const char endChar = ';';

            char[] charArray = defineSymblo.ToCharArray();
            newStr.Clear();

            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];

                if (i != 0 && c == endChar && charArray[i - 1] == endChar)
                    continue;

                newStr.Add(c);
            }

            if (newStr[newStr.Count - 1] != endChar)
                newStr.Add(endChar);

            return new string(newStr.ToArray());
        }


        static void ReadFromFile()
        {
            try
            {
                DefineSymblo[] defines = (DefineSymblo[])DefineSymblo.ArraySerializer.DeserializeXiaGu(FilePath);
                defineSymblos.Clear();
                defineSymblos.Add(defines);
            }
            catch (FileNotFoundException)
            {
                return;
            }
        }

        static void WirteToFile()
        {
            Directory.CreateDirectory(DirPath);

            DefineSymblo[] array = defineSymblos.Where(item => !item.IsEmpty).ToArray();
            DefineSymblo.ArraySerializer.SerializeXiaGu(FilePath, array);
        }

    }

    [XmlType("DefineSymblo")]
    public class DefineSymblo
    {

        static readonly XmlSerializer arraySerializer = new XmlSerializer(typeof(DefineSymblo[]));

        public static XmlSerializer ArraySerializer
        {
            get { return arraySerializer; }
        }

        public DefineSymblo()
        {
        }

        public DefineSymblo(string define)
        {
            this.Define = define;
        }

        [XmlAttribute("Tag")]
        public string Tag { get; set; }

        [XmlAttribute("defineSymblo")]
        public string Define { get; set; }

        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(Define) && string.IsNullOrEmpty(Tag); }
        }

    }


}
