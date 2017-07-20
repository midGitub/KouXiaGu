using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Terrain3D.MapEditor
{

    /// <summary>
    /// 在场景中显示选中的坐标;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SelectNodeSceneDisplay : MonoBehaviour
    {
        SelectNodeSceneDisplay()
        {
        }

        [SerializeField]
        SelectNodeList selectNodeList;
        [SerializeField]
        BoundaryMesh mapBoundaryMesh;
        [SerializeField]
        SelectNodeCameraEffect cameraEffect;
        [SerializeField]
        bool isDisplay;

        public bool IsDisplay
        {
            get { return isDisplay; }
        }

        void Awake()
        {
            selectNodeList.OnSelectNodeChanged += OnSelectNodeChanged;
        }

        void OnEnable()
        {
            if (isDisplay)
            {
                Display();
            }
        }

        void OnDisable()
        {
            Hide();
        }

        public void SetDisplay(bool isDisplay)
        {
            if (isDisplay)
            {
                Display();
            }
            else
            {
                Hide();
            }
        }

        public void Display()
        {
            if (cameraEffect != null)
            {
                cameraEffect.enabled = true;
            }
            if (mapBoundaryMesh != null)
            {
                mapBoundaryMesh.gameObject.SetActive(true);
            }
            if (!isDisplay)
            {
                mapBoundaryMesh.Clear();
                mapBoundaryMesh.Add(selectNodeList);
            }
            isDisplay = true;
        }

        public void Hide()
        {
            if (cameraEffect != null)
            {
                cameraEffect.enabled = false;
            }
            if (mapBoundaryMesh != null)
            {
                mapBoundaryMesh.gameObject.SetActive(false);
                mapBoundaryMesh.Clear();
            }
            isDisplay = false;
        }

        void OnSelectNodeChanged(IEnumerable<CubicHexCoord> positions)
        {
            if (isDisplay)
            {
                mapBoundaryMesh.Clear();
                mapBoundaryMesh.Add(positions);
            }
        }
    }
}
