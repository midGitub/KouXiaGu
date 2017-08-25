using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map.MapEdit
{

    [Serializable]
    public class HexSpiralSizer : PointSizer
    {
        public HexSpiralSizer()
        {
            selectedOffsets = new List<CubicHexCoord>();
            readOnlySelectedOffsets = selectedOffsets.AsReadOnlyList();
        }

        public HexSpiralSizer(int size) : this()
        {
            Size = size;
        }

        List<CubicHexCoord> selectedOffsets;
        readonly IReadOnlyList<CubicHexCoord> readOnlySelectedOffsets;
        public int Size { get; private set; }

        public IReadOnlyList<CubicHexCoord> SelectedArea
        {
            get { return readOnlySelectedOffsets; }
        }

        public override IReadOnlyCollection<CubicHexCoord> SelectedOffsets
        {
            get { return readOnlySelectedOffsets; }
        }

        public void SetSize(int size)
        {
            if (Size != size)
            {
                Size = size;
                selectedOffsets.Clear();
                selectedOffsets.AddRange(CubicHexCoord.Spiral_in(CubicHexCoord.Self, Size));
            }
        }
    }
}
