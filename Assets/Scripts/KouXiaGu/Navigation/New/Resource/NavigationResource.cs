using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 导航信息资源;
    /// </summary>
    public class NavigationResource
    {
        public NavigationResource(WorldResource basicResource)
        {
            landform = basicResource.Landform.AsReadOnlyDictionary(item => item.Navigation);
            road = basicResource.Road.AsReadOnlyDictionary(item => item.Navigation);
            building = basicResource.Building.AsReadOnlyDictionary(item => item.Navigation);
        }

        readonly IReadOnlyDictionary<int, NavigationLandformInfo> landform;
        readonly IReadOnlyDictionary<int, NavigationRoadInfo> road;
        readonly IReadOnlyDictionary<int, NavigationBuildingInfo> building;
        readonly LandformTagManager landformTagManager;

        public IReadOnlyDictionary<int, NavigationLandformInfo> Landform
        {
            get { return landform; }
        }

        public IReadOnlyDictionary<int, NavigationRoadInfo> Road
        {
            get { return road; }
        }

        public IReadOnlyDictionary<int, NavigationBuildingInfo> Building
        {
            get { return building; }
        }
    }
}
