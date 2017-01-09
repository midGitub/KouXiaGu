using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    [XmlType("TerrainMap")]
    public class MapDescription
    {

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(MapDescription));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }


    }

}
