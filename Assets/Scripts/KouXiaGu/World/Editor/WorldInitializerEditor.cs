using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(WorldInitializer), true)]
    class WorldInitializerEditor : Editor
    {

        public WorldInitializer Target
        {
            get { return target as WorldInitializer; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MapGUI();
        }

        bool isFormRandomMap;

        void MapGUI()
        {
            EditorGUILayout.LabelField("地图:");
            isFormRandomMap = EditorGUILayout.BeginToggleGroup("使用随机地图?", isFormRandomMap);

            if (isFormRandomMap)
            {
                RandomMapReadr mapReadr;
                Target.Info.MapReader = mapReadr = 
                    Target.Info.MapReader is RandomMapReadr ? 
                    Target.Info.MapReader as RandomMapReadr :
                    new RandomMapReadr();
                mapReadr.MapSize = EditorGUILayout.IntField("MapSize", mapReadr.MapSize);
            }
            else
            {
                MapResourceReader mapReadr = Target.Info.MapReader is MapResourceReader ?
                    Target.Info.MapReader as MapResourceReader :
                    new MapResourceReader();
            }

            EditorGUILayout.EndToggleGroup();
        }

    }

}
