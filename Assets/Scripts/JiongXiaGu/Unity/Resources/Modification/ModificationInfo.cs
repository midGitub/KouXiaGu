using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{


    /// <summary>
    /// 模组信息;
    /// </summary>
    public struct ModificationInfo : IEquatable<ModificationInfo>
    {
        public string ModificationDirectory { get; private set; }
        public ModificationDescription Description { get; private set; }

        public ModificationInfo(string modificationDirectory, ModificationDescription description)
        {
            ModificationDirectory = modificationDirectory;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            return obj is ModificationInfo && Equals((ModificationInfo)obj);
        }

        public bool Equals(ModificationInfo other)
        {
            return ModificationDirectory == other.ModificationDirectory &&
                   EqualityComparer<string>.Default.Equals(Description.ID, other.Description.ID);
        }

        public override int GetHashCode()
        {
            var hashCode = -163661751;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ModificationDirectory);
            hashCode = hashCode * -1521134295 + EqualityComparer<ModificationDescription>.Default.GetHashCode(Description);
            return hashCode;
        }

        public static bool operator ==(ModificationInfo info1, ModificationInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(ModificationInfo info1, ModificationInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
