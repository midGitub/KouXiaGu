using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 建筑组;
    /// </summary>
    public class BuildingGroup : Townish
    {
        public BuildingGroup(Town belongToTown) : base(belongToTown)
        {
            buildingList = new List<ITownBuilding>();
        }

        List<ITownBuilding> buildingList;

        /// <summary>
        /// 创建这个建筑物,若存在相同的建筑物则不创建到,并返回false;
        /// </summary>
        public bool Create(IRequestor requestor, Building building)
        {
            if (Contains(building))
                return false;

            ITownBuilding instance = building.GetTownBuilding(requestor, BelongToTown);
            buildingList.Add(instance);
            return true;
        }

        /// <summary>
        /// 确认是否存在这个建筑物;
        /// </summary>
        public bool Contains(Building building)
        {
            int index = FindIndex(building);
            return index >= 0;
        }

        /// <summary>
        /// 获取到链表内第一个该建筑物下标;
        /// </summary>
        int FindIndex(Building building)
        {
            int index = buildingList.FindIndex(item => item.BuildingInfo == building);
            return index;
        }

        /// <summary>
        /// 尝试移除这个建筑物;
        /// </summary>
        public bool Destroy(IRequestor requestor, Building building)
        {
            int index = FindIndex(building);

            if (index >= 0)
            {
                buildingList[index].Dispose();
                buildingList.RemoveAt(index);
                return true;
            }

            return false;
        }

    }

}
