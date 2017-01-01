using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    [CustomEditor(typeof(NavigationRes), true)]
    public class NavigationResEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("导出缺失模板"))
            {
                OutputTemplet();
            }

        }

        public static void OutputTemplet()
        {
            TerrainResPath.Create();

            if (!File.Exists(NavigationRes.NavigationDescrFile))
                NavigationRes.Save(NavigationTemplets);

        }


        public static readonly NavigationDescr[] NavigationTemplets = new NavigationDescr[]
            {
                NavigationTemplet(),
                NavigationTemplet(),
                NavigationTemplet(),
            };


        public static NavigationDescr NavigationTemplet()
        {
            return new NavigationDescr()
            {
                Landform = 0,
                Walkable = false,
                SpeedOfTravel = 1,
                NavigationCost = 3,
            };
        }

    }

}
