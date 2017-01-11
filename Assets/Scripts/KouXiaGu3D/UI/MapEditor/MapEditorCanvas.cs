using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent, RequireComponent(typeof(StandaloneUI))]
    public class MapEditorCanvas : MonoBehaviour
    {
        MapEditorCanvas() { }

        [SerializeField]
        Button btnApply;

        [SerializeField]
        Button btnReturn;

        [SerializeField]
        MapEditorPage mapEditor;

        [SerializeField]
        CreateMapPage createMap;

        StandaloneUI standaloneUI;

        void Awake()
        {
            standaloneUI = GetComponent<StandaloneUI>();
            btnApply.onClick.AddListener(StartTerrainEditor);
            btnReturn.onClick.AddListener(standaloneUI.Conceal);
        }

        void StartTerrainEditor()
        {
            TerrainInitializer.TerrainMap = mapEditor.Map;
            StartCoroutine(TerrainInitializer.Begin());
        }

    }

}
