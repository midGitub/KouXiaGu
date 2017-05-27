using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.Resources;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 导航信息资源;
    /// </summary>
    public class NavigationResource
    {
        public NavigationResource(BasicTerrainResource basicResource)
        {
            landform = basicResource.Landform.AsReadOnlyDictionary(item => item.Navigation);
            building = basicResource.Building.AsReadOnlyDictionary(item => item.Navigation);
        }

        IReadOnlyDictionary<int, NavLandformInfo> landform;
        IReadOnlyDictionary<int, NavBuildingInfo> building;
        LandformTag landformTagInfo;

        public IReadOnlyDictionary<int, NavLandformInfo> Landform
        {
            get { return landform; }
        }

        public IReadOnlyDictionary<int, NavBuildingInfo> Building
        {
            get { return building; }
        }
    }
}
