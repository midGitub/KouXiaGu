using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.OperationRecord;
using KouXiaGu.World.Map;
using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using System.Linq;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 地图节点编辑;
    /// </summary>
    public class MapEditPen
    {
        MapEditPen()
        {
            editPenHandlers = new List<IMapEditHandler>();
        }

        public MapEditPen(WorldMap map, IPointSizer pointSizer) : this()
        {
            Map = map;
            PointSizer = pointSizer;
        }

        List<IMapEditHandler> editPenHandlers;
        public WorldMap Map { get; set; }
        public IPointSizer PointSizer { get; set; }

        public bool Add(IMapEditHandler handler)
        {
            if (!Contains(handler))
            {
                editPenHandlers.Add(handler);
                return true;
            }
            return false;
        }

        public bool Remove(IMapEditHandler handler)
        {
            int index = editPenHandlers.FindIndex(item => item.Equals(handler));
            if (index >= 0)
            {
                editPenHandlers.RemoveAt(index);
                return true;
            }
            return false;
        }

        public bool Contains(IMapEditHandler handler)
        {
            return editPenHandlers.Contains(item => item.Equals(handler));
        }

        /// <summary>
        /// 对选中的坐标执行;
        /// </summary>
        public IVoidable Execute()
        {
            using (Map.MapEditorLock.WriteLock())
            {
                var points = PointSizer.GetSelectedArea().ToArray();
                if (points.Length > 0)
                {
                    var selectedArea = GetSelectedArea(points);
                    var voidableGroup = new VoidableGroup<IVoidable>();
                    foreach (var handlers in editPenHandlers)
                    {
                        var voidable = handlers.Execute(selectedArea);
                        if (voidable != null)
                        {
                            voidableGroup.Add(voidable);
                        }
                    }
                    return new MapSetValue(Map, selectedArea, voidableGroup);
                }
                return null;
            }
        }

        List<EditMapNode> GetSelectedArea(IEnumerable<CubicHexCoord> points)
        {
            List<EditMapNode> selectedArea = new List<EditMapNode>();
            foreach (var point in points)
            {
                MapNode node;
                if (Map.Map.TryGetValue(point, out node))
                {
                    var pair = new EditMapNode(point, node);
                    selectedArea.Add(pair);
                }
            }
            return selectedArea;
        }

        sealed class MapSetValue : SafeVoidable
        {
            public MapSetValue(WorldMap map, IEnumerable<EditMapNode> editNodes, IVoidable voidableGroup)
            {
                Map = map;
                EditNodes = editNodes;
                this.voidableGroup = voidableGroup;
                PerformRedo();
            }

            public WorldMap Map { get; private set; }
            public IEnumerable<EditMapNode> EditNodes { get; private set; }
            readonly IVoidable voidableGroup;

            public override void PerformRedo()
            {
                voidableGroup.PerformRedo();
                foreach (var editNode in EditNodes)
                {
                    Map.Map[editNode.Position] = editNode.Value;
                }
            }

            public override void PerformUndo()
            {
                voidableGroup.PerformUndo();
                foreach (var editNode in EditNodes)
                {
                    Map.Map[editNode.Position] = editNode.Original;
                }
            }
        }
    }
}