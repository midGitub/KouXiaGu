using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 建筑管理;
    /// </summary>
    public class BuildingManager
    {
        public BuildingManager()
        {

        }

        public IReadOnlyDictionary<int, Building> Buildings { get; private set; }
        public IReadOnlyDictionary<int, BuildingInfo> BuildingInfos { get; private set; }



    }


}
