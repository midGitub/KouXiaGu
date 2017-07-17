using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Terrain3D.MapEditor
{

    /// <summary>
    /// 在场景中显示选中的坐标;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SelectNodeList))]
    public sealed class SelectNodeSceneDisplay : MonoBehaviour
    {
        SelectNodeSceneDisplay()
        {
        }

        SelectNodeList selectNodeList;
        [SerializeField]
        BoundaryMesh mapBoundaryPrefab;

        void Awake()
        {
            selectNodeList = GetComponent<SelectNodeList>();
            mapBoundaryPrefab.Add(selectNodeList);
            selectNodeList.OnSelectNodeChanged += OnSelectNodeChanged;
        }

        void OnSelectNodeChanged(IEnumerable<CubicHexCoord> positions)
        {
            mapBoundaryPrefab.Clear();
            mapBoundaryPrefab.Add(positions);
        }
    }
}
