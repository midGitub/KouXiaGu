using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.OperationRecord;
using KouXiaGu.World.Map;

namespace KouXiaGu.World.MapEditor
{


    public abstract class NodeEditBase : IEditOperation
    {
        public NodeEditBase(GameMap data)
        {
            Data = data;
        }

        public GameMap Data { get; private set; }
        public int Size { get; set; }

        protected IDictionary<CubicHexCoord, MapNode> map
        {
            get { return Data.Map; }
        }

        public virtual void OnPositionUpdate(CubicHexCoord position)
        {
            return;
        }

        IVoidable IEditOperation.Perform(CubicHexCoord position)
        {
            VoidableGroup<IVoidable> group = new VoidableGroup<IVoidable>();
            foreach (var pos in GetCoverageArea(position))
            {
                var operation = Perform(pos);
                group.Add(operation);
            }
            return group;
        }

        public abstract IVoidable Perform(CubicHexCoord position);

        IEnumerable<CubicHexCoord> GetCoverageArea(CubicHexCoord position)
        {
            return CubicHexCoord.Range(position, Size);
        }
    }
}
