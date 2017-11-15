using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包描述;
    /// </summary>
    public struct LanguagePackDescription : IEquatable<LanguagePackDescription>
    {
        /// <summary>
        /// 语言包名;
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 语言类型;
        /// </summary>
        [XmlAttribute("language")]
        public string Language { get; set; }

        public LanguagePackDescription(string name, string language)
        {
            Name = name;
            Language = language;
        }

        public override string ToString()
        {
            return string.Format("{1}[Name:{2}, Language:{3}]", base.ToString(), Name, Language);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Language.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is LanguagePackDescription)
            {
                return Equals((LanguagePackDescription)obj);
            }
            return false;
        }

        public bool Equals(LanguagePackDescription other)
        {
            return Name == other.Name
                && Language == other.Language;
        }

        public static bool operator ==(LanguagePackDescription v1, LanguagePackDescription v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(LanguagePackDescription v1, LanguagePackDescription v2)
        {
            return !(v1 == v2);
        }
    }
}
