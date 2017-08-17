using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Effects
{

    /// <summary>
    /// 地图区域显示组件;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainAreaEffect : MonoBehaviour
    {
        TerrainAreaEffect()
        {
        }

        static TerrainAreaEffect()
        {
            terrainAreaEffectItems = new Collections.SortedList<TerrainAreaEffect>(TerrainAreaEffectComparer.instance);
        }

        static readonly Collections.SortedList<TerrainAreaEffect> terrainAreaEffectItems;

        /// <summary>
        /// 渲染优先次序;
        /// </summary>
        [SerializeField]
        int sortOrder = 0;
        
        public static IReadOnlyList<TerrainAreaEffect> TerrainAreaEffectItems
        {
            get { return terrainAreaEffectItems; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
        }

        void Awake()
        {
            SetDisplay(false);
        }

        void OnEnable()
        {
            terrainAreaEffectItems.Add(this);
        }

        void OnDisable()
        {
            terrainAreaEffectItems.Remove(this);
        }

        public void SetSortOrder(int sortOrder)
        {
            this.sortOrder = sortOrder;
            terrainAreaEffectItems.Remove(this);
            terrainAreaEffectItems.Add(this);
        }

        public void SetDisplay(bool isDisplay)
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = isDisplay;
            }
        }

        class TerrainAreaEffectComparer : IComparer<TerrainAreaEffect>
        {
            public static readonly TerrainAreaEffectComparer instance = new TerrainAreaEffectComparer();

            public int Compare(TerrainAreaEffect x, TerrainAreaEffect y)
            {
                return x.sortOrder - y.sortOrder;
            }
        }
    }
}
