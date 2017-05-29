using System;
using System.Collections.Generic;

namespace KouXiaGu.Terrain3D
{

    public class TerrainResources
    {

        IDictionary<int, LandformInfo> landform;
        public IDictionary<int, LandformInfo> Landform
        {
            get { return landform; }
            internal set {
                landform = value;
                LandformResources = value.AsReadOnlyDictionary(item => item.Terrain);
            }
        }
        public IReadOnlyDictionary<int, LandformResource> LandformResources { get; private set; }


        IDictionary<int, BuildingInfo> building;
        public IDictionary<int, BuildingInfo> Building
        {
            get { return building; }
            internal set{
                building = value;
                BuildingResources = value.AsReadOnlyDictionary(item => item.Terrain);
            }
        }
        public IReadOnlyDictionary<int, BuildingResource> BuildingResources { get; private set; }


        IDictionary<int, RoadInfo> road;
        public IDictionary<int, RoadInfo> Road
        {
            get { return road; }
            internal set {
                road = value;
                RoadResources = value.AsReadOnlyDictionary(item => item.Terrain);
            }
        }
        public IReadOnlyDictionary<int, RoadResource> RoadResources { get; private set; }


        public LandformTag Tags { get; internal set; }

    }
}
