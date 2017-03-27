using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    public class Building : IEquatable<Building>
    {

        public Building(int id)
        {
            BuildingID = id;
        }

        /// <summary>
        /// 编号;
        /// </summary>
        public int BuildingID { get; private set; }

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
    /// 工厂;
    /// </summary>
    public class Factory : Building
    {

        public Factory(int id, IEnumerable<Container<Product>> output) : base(id)
        {
            this.output = new List<Container<Product>>(output);
        }

        List<Container<Product>> output;

        /// <summary>
        /// 产出;
        /// </summary>
        public IEnumerable<Container<Product>> Output
        {
            get { return output; }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Factory v1, Factory v2)
        {
            return v1.BuildingID == v2.BuildingID;
        }

        public static bool operator !=(Factory v1, Factory v2)
        {
            return v1.BuildingID != v2.BuildingID;
        }

    }


}
