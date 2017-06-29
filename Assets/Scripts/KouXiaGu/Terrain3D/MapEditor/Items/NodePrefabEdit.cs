using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.OperationRecord;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D.MapEditor
{

    public class NodesPrefab
    {
        public NodesPrefab()
        {
            Infos = new List<KeyValuePair<CubicHexCoord, NodeInfo>>();
        }

        public List<KeyValuePair<CubicHexCoord, NodeInfo>> Infos { get; private set; }

        public bool Contains(CubicHexCoord position)
        {
            return Infos.Contains(item => item.Key == position);
        }
    }

    public class NodePrefabEdit : IEditOperation
    {
        public NodePrefabEdit(GameMap data, NodesPrefab prefab)
        {
            Data = data;
            Prefab = prefab;
        }

        public GameMap Data { get; private set; }
        public NodesPrefab Prefab { get; private set; }

        public void OnPositionUpdate(CubicHexCoord position)
        {
            return;
        }

        public IVoidable Perform(CubicHexCoord position)
        {
            VoidableGroup<IVoidable> group = new VoidableGroup<IVoidable>();
            foreach (var info in Prefab.Infos)
            {
                var pos = info.Key + position;
                var node = Data.Map[pos];

                node.Building = node.Building.Update(Data, info.Value.Building);
                node.Landform = node.Landform.Update(Data, info.Value.Landform);
                node.Road = node.Road.Update(Data, info.Value.Road);

                Data.Map[pos] = node;
            }
            return group;
        }
    }
}
