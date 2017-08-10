using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using KouXiaGu.OperationRecord;
using KouXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        List<UIMapEditHandlerView> handlerViews;
        public IPointSizer PointSizer { get; set; }
        public int CurrentViewIndex { get; private set; }

        public int ViewCount
        {
            get { return handlerViews.Count; }
        }

        internal IList<UIMapEditHandlerView> HandlerViews
        {
            get { return handlerViews; }
        }

        public UIMapEditHandlerView CurrentView
        {
            get { return handlerViews[CurrentViewIndex]; }
        }

        void Awake()
        {
            handlerViews = new List<UIMapEditHandlerView>();
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

        }

        /// <summary>
        /// 对所有节点执行操作;
        /// </summary>
        public IVoidable Execute()
        {
            if (WorldSceneManager.World == null)
                return null;

            var map = WorldSceneManager.World.WorldData.MapData;
            var selectedArea = GetSelectedArea(map, PointSizer.SelectedArea);
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
