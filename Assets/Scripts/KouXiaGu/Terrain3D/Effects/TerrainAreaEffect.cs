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

        public static IReadOnlyList<TerrainAreaEffect> TerrainAreaEffectItems
        {
            get { return terrainAreaEffectItems; }
        }


        /// <summary>
        /// 渲染优先次序;
        /// </summary>
        [SerializeField]
        int sortOrder = 0;
        /// <summary>
        /// 外部线框宽度;
        /// </summary>
        [SerializeField, Range(1, 40)]
        float outLineWidth = 3;
        /// <summary>
        /// 内部透明度;
        /// </summary>
        [SerializeField, Range(0, 1)]
        float internalTransparency = 0.1f;

        public int SortOrder
        {
            get { return sortOrder; }
        }

        public float OutLineWidth
        {
            get { return outLineWidth; }
            set { outLineWidth = value; }
        }

        public float InternalTransparency
        {
            get { return internalTransparency; }
            set { internalTransparency = value; }
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
