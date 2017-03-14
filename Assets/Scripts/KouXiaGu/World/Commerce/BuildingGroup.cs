using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 建筑类型;
    /// </summary>
    public class BuildingType
    {

        public BuildingType(int id)
        {
            this.ID = id;
        }

        /// <summary>
        /// 区分建筑物类型的ID;
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 建筑物是否允许在此城镇建设?若不允许则返回对应异常;
        /// </summary>
        public virtual void Precondition(Town town)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将建筑物安置到此城镇;
        /// </summary>
        public virtual IBuilding Install(Town town)
        {
            Precondition(town);
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BuildingType))
                return false;
            return ((BuildingType)obj).ID == this.ID;
        }

    }

    public interface IBuilding : IDisposable
    {
        bool IsNeedDayUpdate { get; }
        void DayUpdate();
    }

    /// <summary>
    /// 
    /// </summary>
    public class BuildingGroup : ICollection<IBuilding>
    {

        public BuildingGroup()
        {
            buildingList = new List<IBuilding>();
        }


        List<IBuilding> buildingList;


        public int Count
        {
            get { return ((ICollection<IBuilding>)this.buildingList).Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<IBuilding>)this.buildingList).IsReadOnly; }
        }


        public void DayUpdate()
        {

        }

        public void Add(IBuilding item)
        {
            ((ICollection<IBuilding>)this.buildingList).Add(item);
        }

        public void Clear()
        {
            ((ICollection<IBuilding>)this.buildingList).Clear();
        }

        public bool Contains(IBuilding item)
        {
            return ((ICollection<IBuilding>)this.buildingList).Contains(item);
        }

        public void CopyTo(IBuilding[] array, int arrayIndex)
        {
            ((ICollection<IBuilding>)this.buildingList).CopyTo(array, arrayIndex);
        }

        public bool Remove(IBuilding item)
        {
            return ((ICollection<IBuilding>)this.buildingList).Remove(item);
        }

        public IEnumerator<IBuilding> GetEnumerator()
        {
            return ((ICollection<IBuilding>)this.buildingList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<IBuilding>)this.buildingList).GetEnumerator();
        }

    }

}
