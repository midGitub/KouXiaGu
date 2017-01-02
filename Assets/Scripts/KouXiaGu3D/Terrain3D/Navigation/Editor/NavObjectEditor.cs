using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    [CustomEditor(typeof(Navigator), true)]
    public class NavObjectEditor : Editor
    {
        Vector3 destination;

        static PathFindingCost obstruction = new PathFindingCost(1, 1);

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Navigator nav = (Navigator)target;

            GUILayout.BeginVertical();

            GUILayout.Label("导航测试:");

            destination = EditorGUILayout.Vector3Field("目的地:", destination);

            if (GUILayout.Button("导航到"))
            {
                CubicHexCoord starting = Convert(nav.transform.position);
                CubicHexCoord destinationCoord = Convert(destination);

                IMovable character = nav.GetComponent<IMovable>();
                nav.NavigateTo(destinationCoord, obstruction, new HexRadiusRange(50, starting), character);
            }

            GUILayout.EndVertical();

        }

        CubicHexCoord Convert(Vector3 pos)
        {
            return GridConvert.Grid.GetCubic(pos);
        }

    }

}
