using System.IO;
using UnityEngine;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组描述信息;
    /// </summary>
    [XmlRoot("Modification")]
    public struct ModificationDescription : IEquatable<ModificationDescription>
    {
        /// <summary>
        /// 唯一标识,只允许数字,字母,下划线组成;
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 模组名称;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作者;
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 版本;
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 标签;
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 所有 AssetBundle 描述;
        /// </summary>
        public Set<AssetBundleDescription> AssetBundles { get; set; }

        internal ModificationDescription(string id, string name) : this()
        {
            ID = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is ModificationDescription && Equals((ModificationDescription)obj);
        }

        public bool Equals(ModificationDescription other)
        {
            return ID == other.ID &&
                   Name == other.Name &&
                   Author == other.Author &&
                   Version == other.Version &&
                   Tags == other.Tags &&
                   Message == other.Message;
        }

        public override int GetHashCode()
        {
            var hashCode = 101501911;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Author);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Version);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tags);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            return hashCode;
        }

        public static bool operator ==(ModificationDescription description1, ModificationDescription description2)
        {
            return description1.Equals(description2);
        }

        public static bool operator !=(ModificationDescription description1, ModificationDescription description2)
        {
            return !(description1 == description2);
        }
    }
}
