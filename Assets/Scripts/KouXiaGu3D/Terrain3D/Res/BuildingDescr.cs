using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    [XmlType("Building")]
    public struct BuildingDescr
    {

        static readonly XmlSerializer arraySerializer = new XmlSerializer(typeof(BuildingDescr[]));

        public static XmlSerializer ArraySerializer
        {
            get { return arraySerializer; }
        }

        /// <summary>
        /// 唯一标示(0,-1作为保留);
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }

        /// <summary>
        /// 定义名,允许为空;
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }




        public override bool Equals(object obj)
        {
            if (!(obj is BuildingDescr))
                return false;
            return ID == ((BuildingDescr)obj).ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            return "[Building_ID:" + ID.ToString() + ",Name:" + Name.ToString() + "]";
        }

    }

}
