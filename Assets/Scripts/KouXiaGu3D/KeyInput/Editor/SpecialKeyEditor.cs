using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    [CustomEditor(typeof(SpecialKey), true)]
    class SpecialKeyEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            ShowKey(SpecialKey.Escape);
            ShowKey(SpecialKey.Enter);

        }

        void ShowKey(ResponseKeyStack stack)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("SpecialKey:");
            GUILayout.Label(stack.Key.ToString());

            GUILayout.Label("Observer:");

            if (stack.IsInitialized)
                GUILayout.Label(stack.Count.ToString());
            else
                GUILayout.Label("Null");

            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();

            GUILayout.Label("Activate:");

            if (stack.IsInitialized && !stack.IsEmpty)
                GUILayout.Label(stack.Activate.ToString());
            else
                GUILayout.Label("Null");

            GUILayout.EndHorizontal();
        }


    }


}
