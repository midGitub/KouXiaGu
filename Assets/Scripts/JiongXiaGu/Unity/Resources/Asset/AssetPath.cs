using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示资源相对路径 或者 引用其它资源的路径;
    /// </summary>
    public struct AssetPath : IEquatable<AssetPath>, IXmlSerializable
    {
        /// <summary>
        /// 路径引用符;
        /// </summary>
        public const char PathReferenceSymbol = '@';

        /// <summary>
        /// 路径结构分离符;
        /// </summary>
        public const char PathStructuralSeparator = ':';

        /// <summary>
        /// 资源路径;
        /// </summary>
        public string Name { get; private set; }

        public AssetPath(string path)
        {
            Name = path.Trim();
        }

        public AssetPath(string contentID, string relativePath)
        {
            Name = GetReferencePath(contentID, relativePath);
        }

        /// <summary>
        /// 获取到引用路径;
        /// </summary>
        private static string GetReferencePath(string contentID, string relativePath)
        {
            return string.Concat(PathReferenceSymbol, contentID.Trim(), PathStructuralSeparator, relativePath.Trim());
        }

        /// <summary>
        /// 该路径是否为引用路径;
        /// </summary>
        public bool IsReferencePath()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name[0] == PathReferenceSymbol;
        }

        /// <summary>
        /// 获取到相对路径;若引用到其它资源则返回false,返回内容ID和相对路径;若为本地目录,则返回true,仅返回相对路径;
        /// </summary>
        public bool GetRelativePath(out string contentID, out string relativePath)
        {
            if (IsReferencePath())
            {
                int index = Name.IndexOf(PathStructuralSeparator);
                if (index >= 0)
                {
                    contentID = Name.Substring(1, index - 1).Trim();
                    relativePath = Name.Substring(index + 1).Trim();
                    return false;
                }
                else
                {
                    contentID = string.Empty;
                    relativePath = Name.Substring(1).Trim();
                    return true;
                }
            }
            else
            {
                contentID = string.Empty;
                relativePath = Name.Trim();
                return true;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj is AssetPath && Equals((AssetPath)obj);
        }

        public bool Equals(AssetPath other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.ReadContentAsString();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteValue(Name);
        }

        public static implicit operator AssetPath(string path)
        {
            return new AssetPath(path);
        }

        public static explicit operator string(AssetPath assetPath)
        {
            return assetPath.Name;
        }

        public static bool operator ==(AssetPath path1, AssetPath path2)
        {
            return path1.Equals(path2);
        }

        public static bool operator !=(AssetPath path1, AssetPath path2)
        {
            return !(path1 == path2);
        }
    }
}
