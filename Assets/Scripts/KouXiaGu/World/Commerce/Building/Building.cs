using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;

namespace KouXiaGu.World
{

    /// <summary>
    /// 建筑信息;
    /// </summary>
    public struct BuildingInfo
    {

    }

    /// <summary>
    /// 建筑物基类;
    /// </summary>
    public abstract class Building : IEquatable<Building>
    {
        public Building(BuildingManager manager, int id)
        {
            Manager = manager;
            BuildingID = id;
        }

        public BuildingManager Manager { get; private set; }

        /// <summary>
        /// 编号;
        /// </summary>
        public int BuildingID { get; private set; }

        /// <summary>
        /// 建筑物信息;
        /// </summary>
        public BuildingInfo Info
        {
            get { return Manager.BuildingInfos[BuildingID]; }
        }

        /// <summary>
        /// 获取到城镇建筑类型实例;
        /// </summary>
        public abstract ITownBuilding GetTownBuilding(IRequestor requestor, Town belongToTown);

        /// <summary>
        /// 建造此建筑的前提;
        /// </summary>
        public bool Precondition(Town town)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Building other)
        {
            return other.BuildingID == this.BuildingID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Building))
                return false;
            return Equals((Building)obj);
        }

        public override int GetHashCode()
        {
            return BuildingID;
        }

        public static bool operator ==(Building v1, Building v2)
        {
            return v1.BuildingID == v2.BuildingID;
        }

        public static bool operator !=(Building v1, Building v2)
        {
            return v1.BuildingID != v2.BuildingID;
        }

    }

    /// <summary>
    /// 城镇建筑;
    /// </summary>
    public interface ITownBuilding : IDisposable
    {
        Building BuildingInfo { get; }
    }

}
