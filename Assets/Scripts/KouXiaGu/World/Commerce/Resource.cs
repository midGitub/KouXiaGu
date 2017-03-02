using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 资源信息(结构体);
    /// </summary>
    public class Resource : IEquatable<Resource>
    {

        /// <summary>
        /// 资源编号;
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 资源属于的类别;
        /// </summary>
        public ResoureCategorie Categorie { get; set; }


        public bool Equals(Resource other)
        {
            return other.ID == this.ID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Resource))
                return false;

            return Equals((Resource)obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }


        public static bool operator ==(Resource v1, Resource v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Resource v1, Resource v2)
        {
            return !v1.Equals(v2);
        }

    }

}
