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
        [SerializeField]
        SelectNodeCameraEffect cameraEffect;
        [SerializeField]
        bool isDisplay;

        public bool IsDisplay
        {
            get { return isDisplay; }
            set
            {
                isDisplay = value;
                cameraEffect.enabled = value;
            }
        }

        void Awake()
        {
            selectNodeList = GetComponent<SelectNodeList>();
            mapBoundaryPrefab.Add(selectNodeList);
            selectNodeList.OnSelectNodeChanged += OnSelectNodeChanged;
        }

        void OnValidate()
        {
            if (isDisplay)
            {
                Display_internal();
            }
            else
            {
                Hide_internal();
            }
        }

        void OnEnable()
        {
            if (isDisplay)
            {
                Display_internal();
            }
        }

        void OnDisable()
        {
            Hide_internal();
        }

        void Display_internal()
        {
            if (cameraEffect != null)
            {
                cameraEffect.enabled = true;
            }
        }

        void Hide_internal()
        {
            if (cameraEffect != null)
            {
                cameraEffect.enabled = false;
            }
        }

        void OnSelectNodeChanged(IEnumerable<CubicHexCoord> positions)
        {
            mapBoundaryPrefab.Clear();
            mapBoundaryPrefab.Add(positions);
        }
    }
}
