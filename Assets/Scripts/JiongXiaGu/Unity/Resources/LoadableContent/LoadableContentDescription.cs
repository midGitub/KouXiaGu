using System.IO;
using UnityEngine;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 通用的描述;
    /// </summary>
    [XmlRoot("LoadableContentDescription")]
    public struct LoadableContentDescription : IEquatable<LoadableContentDescription>
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
        /// 标签分隔符;
        /// </summary>
        public const char TagSeparatorChar = ',';
        internal static readonly char[] TagSeparatorCharArray = new char[] { TagSeparatorChar };

        /// <summary>
        /// 标签;
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// AssetBundle 描述;
        /// </summary>
        public AssetBundleDescription[] AssetBundles { get; set; }

        internal LoadableContentDescription(string id, string name) : this()
        {
            ID = id;
            Name = name;
        }

        /// <summary>
        /// 获取到所有标签;
        /// </summary>
        public string[] GetTags()
        {
            return Tags.Split(TagSeparatorCharArray, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 设置到所有标签;
        /// </summary>
        public static string JoinTags(params string[] tag)
        {
            string tags = string.Join(TagSeparatorChar.ToString(), tag);
            return tags;
        }

        public override bool Equals(object obj)
        {
            return obj is LoadableContentDescription && Equals((LoadableContentDescription)obj);
        }

        public bool Equals(LoadableContentDescription other)
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

        public static bool operator ==(LoadableContentDescription description1, LoadableContentDescription description2)
        {
            return description1.Equals(description2);
        }

        public static bool operator !=(LoadableContentDescription description1, LoadableContentDescription description2)
        {
            return !(description1 == description2);
        }
    }
}
