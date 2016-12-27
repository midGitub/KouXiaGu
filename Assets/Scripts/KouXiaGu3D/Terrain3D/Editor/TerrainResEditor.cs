using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    [CustomEditor(typeof(TerrainRes), true)]
    public class TerrainResEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("导出模板"))
            {
                RoadDescr[] items = new RoadDescr[] 
                {
                    new RoadDescr(),
                };

                DirectoryInfo directory = Directory.CreateDirectory(Path.GetDirectoryName(TerrainRes.RoadDescrFile));
                RoadDescr.ArraySerializer.SerializeFile(directory.FullName, items);
            }

        }



    }

}
