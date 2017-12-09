//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Schema;
//using System.Xml.Serialization;
//using UnityEngine;

//namespace JiongXiaGu.Unity.RectTerrain
//{

//    /// <summary>
//    /// 贴图资源信息;
//    /// </summary>
//    [XmlRoot("TextureInfo")]
//    public class TextureInfo
//    {
//        public TextureInfo()
//        {
//        }

//        public TextureInfo(string name)
//        {
//            Name = name;
//        }

//        [XmlElement("name")]
//        public string Name { get; set; }

//        [XmlIgnore]
//        public Texture Texture { get; set; }

//        public bool IsCompleted
//        {
//            get { return Texture != null; }
//        }

//        public static implicit operator TextureInfo(string name)
//        {
//            return new TextureInfo()
//            {
//                Name = name
//            };
//        }

//        public static implicit operator Texture(TextureInfo info)
//        {
//            return info.Texture;
//        }
//    }

//}
