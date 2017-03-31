using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.XiaGuTools
{

    public sealed class GlobalDefineSymblosEditorWindow : EditorWindow
    {

        GlobalDefineSymblosEditorWindow()
        {
            titleContent = new GUIContent("DefineSymblos");
            minSize = new Vector2(420f, 300f);
        }

        public const string DirName = "Editor\\XiaGuTools";
        public const string FileName = "DefineSymblos.xml";

        static readonly Color red = new Color(200f / 255f, 100f / 255f, 100f / 255f);
        static readonly Color green = new Color(150f / 255f, 210f / 255f, 110f / 255f);

        public const BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone;


        static readonly LinkedList<DefineAndTag> defineSymblos = new LinkedList<DefineAndTag>();


        public static string DirPath
        {
            get { return Path.Combine(Application.dataPath, DirName); }
        }

        public static string FilePath
        {
            get { return Path.Combine(DirPath, FileName); }
        }


        [MenuItem(XiaGuTool.MenuName + "/DefineSymblos")]
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

            LinkedListNode<DefineAndTag> pointer = defineSymblos.First;
            while (pointer != null)
            {
                DefineAndTag defineSymblo = pointer.Value;

                EditorGUILayout.BeginHorizontal();


                GUI.backgroundColor = red;
                if (GUILayout.Button("Remove", GUILayout.Width(65f)))
                {
                    LinkedListNode<DefineAndTag> next = pointer.Next;
                    defineSymblos.Remove(pointer);
                    pointer = next;
                }
                else
                {
                    pointer = pointer.Next;
                }
                GUI.backgroundColor = Color.white;


                EditorGUILayout.LabelField("Tag:", GUILayout.MaxWidth(28f));
                defineSymblo.Tag = EditorGUILayout.TextField(defineSymblo.Tag, GUILayout.MaxWidth(222f));

                EditorGUILayout.LabelField("DefineSymblos:", GUILayout.MaxWidth(92f));
                string result = EditorGUILayout.TextField(defineSymblo.Define);

                if (result != defineSymblo.Define)
                    defineSymblo.Define = Normalize(result);


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
                defineSymblos.AddLast(new DefineAndTag());
            else if (!defineSymblos.Last.Value.IsEmpty)
                defineSymblos.AddLast(new DefineAndTag());
        }

        static void AutoRemoveEmpty()
        {
            LinkedListNode<DefineAndTag> pointer = defineSymblos.First;
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


        const char separatorChar = ';';
        const string separatorString = ";";
        static readonly List<char> newStr = new List<char>();

        /// <summary>
        /// 去除多余的结束符;
        /// </summary>
        static string Normalize(string defineSymblo)
        {
            if (string.IsNullOrEmpty(defineSymblo))
                return defineSymblo;

            char[] charArray = defineSymblo.ToCharArray();
            newStr.Clear();

            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];

                if (i != 0 && c == separatorChar && charArray[i - 1] == separatorChar)
                    continue;

                newStr.Add(c);
            }

            return new string(newStr.ToArray());
        }


        string GetDefinesFromPlayerSettings()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        }

        void SetDefinesToPlayerSettings()
        {
            string defines = defineSymblos.
                Select(item => item.Define).
                Aggregate(delegate(string workingSentence, string next)
                {
                    if (!string.IsNullOrEmpty(workingSentence) && !workingSentence.EndsWith(separatorString))
                    {
                        workingSentence += separatorString;
                    }

                    return workingSentence + next;
                });

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }


        static void ReadFromFile()
        {
            string filePath = FilePath;

            if (File.Exists(filePath))
            {
                DefineAndTag[] defines = (DefineAndTag[])DefineAndTag.ArraySerializer.DeserializeXiaGu(filePath);

                defineSymblos.Clear();
                AddRange(defineSymblos, defines);
            }
        }

        static void AddRange<T>(ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        static void WirteToFile()
        {
            Directory.CreateDirectory(DirPath);

            DefineAndTag[] save = defineSymblos.Where(item => !item.IsEmpty).ToArray();
            DefineAndTag.ArraySerializer.SerializeXiaGu(FilePath, save);
        }

    }


    [XmlType("Define")]
    public class DefineAndTag
    {

        static readonly XmlSerializer arraySerializer = new XmlSerializer(typeof(DefineAndTag[]));

        public static XmlSerializer ArraySerializer
        {
            get { return arraySerializer; }
        }

        public DefineAndTag()
        {
        }

        public DefineAndTag(string tag, string define)
        {
            this.Tag = tag;
            this.Define = define;
        }

        [XmlAttribute("tag")]
        public string Tag { get; set; }

        [XmlAttribute("defineSymblo")]
        public string Define { get; set; }

        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(Define) && string.IsNullOrEmpty(Tag); }
        }

    }

}
