using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using KouXiaGu.OperationRecord;
using KouXiaGu.Terrain3D;
using KouXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 地图节点编辑面板;
    /// </summary>
    [DisallowMultipleComponent]
    public class UIMapEditPanle : MonoBehaviour
    {
        UIMapEditPanle()
        {
        }

        [SerializeField]
        SelectablePanel panel;
        UIMapEditHandlerView currentView;
        [SerializeField]
        UIMapEditSizer pointSizer;
        [SerializeField]
        Toggle[] viewSwitchs;

        public UIMapEditHandlerView CurrentView
        {
            get { return currentView; }
            internal set { currentView = value; }
        }

        public UIMapEditSizer CurrentSizer
        {
            get { return pointSizer; }
        }

        void Awake()
        {
            panel.OnFocusEvent += OnFocus;
            panel.OnBlurEvent += OnBlur;

            if (panel.IsFocus)
            {
                OnFocus();
            }
            else
            {
                OnBlur();
            }
        }

        void OnFocus()
        {
            enabled = true;
        }

        void OnBlur()
        {
            enabled = false;
        }

        void Update()
        {
            Vector3 mousePoint;
            if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                pointSizer.OnUpdate(mousePoint);
            }
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                Execute(mousePoint.GetTerrainCubic());
            }
            ViewSwitchKeyResponse();
        }

        void ViewSwitchKeyResponse()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                viewSwitchs[0].SetValue(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                viewSwitchs[1].SetValue(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                viewSwitchs[2].SetValue(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                viewSwitchs[3].SetValue(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                viewSwitchs[4].SetValue(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                viewSwitchs[5].SetValue(true);
            }
        }

        /// <summary>
        /// 对所有节点执行操作;
        /// </summary>
        public IVoidable Execute(CubicHexCoord center)
        {
            if (WorldSceneManager.World == null)
                return null;

            var map = WorldSceneManager.World.WorldData.MapData;
            var selectedArea = GetSelectedArea(map, pointSizer.EnumerateSelecteArea(center));
            return CurrentView.Execute(map, selectedArea);
        }

        List<EditMapNode> GetSelectedArea(WorldMap map, IEnumerable<CubicHexCoord> points)
        {
            List<EditMapNode> selectedArea = new List<EditMapNode>();
            foreach (var point in points)
            {
                MapNode node;
                if (map.Map.TryGetValue(point, out node))
                {
                    var pair = new EditMapNode(point, node);
                    selectedArea.Add(pair);
                }
            }
            return selectedArea;
        }
    }
}
