using System;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图描述;
    /// </summary>
    public struct MapDescription : IEquatable<MapDescription>
    {
        public MapDescription(string name) : this()
        {
            Name = name;
        }

        public MapDescription(string name, int version) : this(name)
        {
            Version = version;
        }

        public MapDescription(string name, int version, bool isArchived) : this(name, version)
        {
            IsArchived = isArchived;
        }

        /// <summary>
        /// 地图名;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地图版本;
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 是否为存档?
        /// </summary>
        public bool IsArchived { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is MapDescription)
            {
                return Equals((MapDescription)obj);
            }
            return false;
        }

        public bool Equals(MapDescription other)
        {
            return Name == other.Name
                && Version == other.Version
                && IsArchived == IsArchived;
        }

        public override int GetHashCode()
        {
            var hashCode = 2009097656;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Version.GetHashCode();
            hashCode = hashCode * -1521134295 + IsArchived.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(MapDescription description1, MapDescription description2)
        {
            return description1.Equals(description2);
        }

        public static bool operator !=(MapDescription description1, MapDescription description2)
        {
            return !(description1 == description2);
        }
    }
}
