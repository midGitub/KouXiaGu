using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEditor;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    [CustomEditor(typeof(NavObject), true)]
    public class NavObjectEditor : Editor
    {
        Vector3 destination;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            NavObject nav = (NavObject)target;

            GUILayout.BeginVertical();

            GUILayout.Label("导航测试:");

            destination = EditorGUILayout.Vector3Field("目的地:", destination);

            if (GUILayout.Button("导航到"))
            {
                CubicHexCoord starting = Convert(nav.transform.position);
                CubicHexCoord destinationCoord = Convert(destination);
                NavigationPath path = GetPath(nav.gameObject, starting, destinationCoord);
                nav.Follow(path);
                Debug.Log(path.ToString());
            }

            GUILayout.EndVertical();

        }

        CubicHexCoord Convert(Vector3 pos)
        {
            return GridConvert.Grid.GetCubic(pos);
        }

        NavigationPath GetPath(GameObject gameObject, CubicHexCoord starting, CubicHexCoord destination)
        {
            IMovable character = gameObject.GetComponent<IMovable>();
            return Navigator.FindPath(starting, destination, new Obstruction(1, 1), new HexRadiusRange(50, starting), character);
        }

    }

}
