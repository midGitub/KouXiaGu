using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.RectTerrain.Resources
{

    /// <summary>
    /// 贴图资源;
    /// </summary>
    [XmlRoot("TextureInfo")]
    public class TextureInfo
    {
        public TextureInfo()
        {
        }

        public TextureInfo(string name)
        {
            Name = name;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlIgnore]
        public Texture Texture { get; set; }

        public bool IsCompleted
        {
            get { return Texture != null; }
        }

        public static implicit operator TextureInfo(string name)
        {
            return new TextureInfo()
            {
                Name = name
            };
        }
    }
}
