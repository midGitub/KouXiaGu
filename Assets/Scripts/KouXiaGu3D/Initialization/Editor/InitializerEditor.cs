
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    [CustomEditor(typeof(Initializer), true)]
    public class InitializerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            GUILayout.BeginVertical();

            EditorGUILayout.TextField("阶段", Initializer.Stages.ToString());

            if (GUILayout.Button("初始化"))
            {
                InitialStage.Start();
            }

            if (GUILayout.Button("开始游戏"))
            {
                GameStage.Start(new ArchiveFile());
            }

            if (GUILayout.Button("结束游戏"))
            {
                GameStage.End();
            }

            if (GUILayout.Button("进行存档"))
            {
                ArchiveStage.Start();
            }

            GUILayout.EndVertical();

        }
    }

}