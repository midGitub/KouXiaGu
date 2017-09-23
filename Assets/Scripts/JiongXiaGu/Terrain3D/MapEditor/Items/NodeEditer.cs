using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using JiongXiaGu.Operations;
using JiongXiaGu.World.Map;

namespace JiongXiaGu.Terrain3D.MapEditor
{


    public abstract class NodeEditer : IEditOperation
    {
        public NodeEditer(WorldMap data)
        {
            Data = data;
        }

        public WorldMap Data { get; private set; }
        public int Size { get; set; }

        protected IDictionary<CubicHexCoord, MapNode> map
        {
            get { return Data.Map; }
        }

        public virtual void OnPositionUpdate(CubicHexCoord position)
        {
            return;
        }

        VoidableOperation IEditOperation.Perform(CubicHexCoord position)
        {
            VoidableOperationGroup<VoidableOperation> group = new VoidableOperationGroup<VoidableOperation>();
            foreach (var pos in GetCoverageArea(position))
            {
                var operation = Perform(pos);
                group.Add(operation);
            }
            return group;
        }

        public abstract VoidableOperation Perform(CubicHexCoord position);

        IEnumerable<CubicHexCoord> GetCoverageArea(CubicHexCoord position)
        {
            return CubicHexCoord.Range(position, Size);
        }
    }
}
