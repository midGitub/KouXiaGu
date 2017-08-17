using KouXiaGu.Grids;
using KouXiaGu.Terrain3D;
using KouXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 提供编辑的坐标;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIMapEditSizer : MonoBehaviour
    {
        UIMapEditSizer()
        {
        }

        [SerializeField]
        SelectablePanel panel;
        [SerializeField]
        BoundaryMesh terrainBoundaryMesh;
        List<CubicHexCoord> offsets;
        public IReadOnlyCollection<CubicHexCoord> Offsets { get; private set; }

        void Awake()
        {
            offsets = new List<CubicHexCoord>();
            Offsets = offsets.AsReadOnlyCollection();
        }

        /// <summary>
        /// 由 UIMapEditPanle 调用;
        /// </summary>
        internal void OnUpdate(Vector3 position)
        {
            terrainBoundaryMesh.transform.localPosition = ToXZ(position);
        }

        Vector3 ToXZ(Vector3 pos)
        {
            CubicHexCoord v1 = pos.GetTerrainCubic();
            pos = v1.GetTerrainPixel();
            pos.y = 0;
            return pos;
        }

        /// <summary>
        /// 获取到选中的坐标;
        /// </summary>
        public IEnumerable<CubicHexCoord> EnumerateSelecteArea(CubicHexCoord pos)
        {
            foreach (var offset in offsets)
            {
                yield return offset + pos;
            }
        }

        /// <summary>
        /// 设置到选中坐标的偏移量;
        /// </summary>
        public void SetSelecteOffsets(IEnumerable<CubicHexCoord> selecteOffsets)
        {
            offsets.Clear();
            offsets.AddRange(selecteOffsets);
            terrainBoundaryMesh.UpdatePoints(selecteOffsets);
        }
    }
}
