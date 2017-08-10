using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map.MapEdit
{


    public class HexSpiralSizer : IPointSizer
    {
        public HexSpiralSizer()
        {
            selectedPoints = new List<CubicHexCoord>();
            readOnlySelectedPoints = selectedPoints.AsReadOnlyList();
        }

        public HexSpiralSizer(CubicHexCoord centre, int size) : this()
        {
            Centre = centre;
            Size = size;
        }

        List<CubicHexCoord> selectedPoints;
        readonly IReadOnlyList<CubicHexCoord> readOnlySelectedPoints;
        public int Size { get; private set; }
        public CubicHexCoord Centre { get; private set; }

        public IReadOnlyList<CubicHexCoord> SelectedArea
        {
            get { return readOnlySelectedPoints; }
        }

        public void SetCentre(CubicHexCoord centre)
        {
            if (Centre != centre)
            {
                Centre = centre;
                selectedPoints.Clear();
                CubicHexCoord.Spiral(Centre, Size, ref selectedPoints);
            }
        }

        public void SetSize(int size)
        {
            if (Size != size)
            {
                Size = size;
                selectedPoints.Clear();
                CubicHexCoord.Spiral(Centre, Size, ref selectedPoints);
            }
        }
    }
}
