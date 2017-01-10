using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public class MapFilerUI : MonoBehaviour
    {
        MapFilerUI() { }

        [SerializeField]
        Transform mapItemPrefabParent;

        [SerializeField]
        MapItemUI mapItemPrefab;

        /// <summary>
        /// 所有实例化的;
        /// </summary>
        List<MapItemUI> activates;

        MapItemUI Selected;

        /// <summary>
        /// 选中的地图,若未选中或者不存在则返回 default();
        /// </summary>
        public TerrainMap Map { get; private set; }

        void Awake()
        {
            activates = new List<MapItemUI>();
        }

        void Start()
        {
            foreach (var item in MapFiler.ReadOnlyMaps)
            {
                Create(item);
            }
        }

        void Create(TerrainMap map)
        {
            MapItemUI item = Instantiate<MapItemUI>(mapItemPrefab, mapItemPrefabParent);
            item.gameObject.SetActive(true);
            activates.Add(item);
        }

    }

}
