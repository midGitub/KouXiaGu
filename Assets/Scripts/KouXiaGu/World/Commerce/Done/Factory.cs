using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 工厂;
    /// </summary>
    public class Factory : Building
    {

        public Factory(int id, IEnumerable<ProductContainer> output) : base(id)
        {
            this.output = new List<ProductContainer>(output);
        }

        List<ProductContainer> output;

        /// <summary>
        /// 产出;
        /// </summary>
        public IEnumerable<ProductContainer> Output
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
