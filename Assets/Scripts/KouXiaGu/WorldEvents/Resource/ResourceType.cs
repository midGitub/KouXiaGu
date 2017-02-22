using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 资源类型;
    /// </summary>
    public struct ResourceType : IEquatable<ResourceType>, IEquatable<int>
    {

        public ResourceType(int id)
        {
            if (id == 0)
                throw new ArgumentOutOfRangeException();

            this.ID = id;
        }


        public int ID { get; private set; }


        public bool Equals(int other)
        {
            return this.ID == other;
        }

        public bool Equals(ResourceType other)
        {
            return this.ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ResourceType))
                return false;
            return Equals((ResourceType)obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }


        public static implicit operator int(ResourceType type)
        {
            return type.ID;
        }

        public static bool operator ==(ResourceType type, int id)
        {
            return type.ID == id;
        }

        public static bool operator !=(ResourceType type, int id)
        {
            return !(type == id);
        }

    }

}
