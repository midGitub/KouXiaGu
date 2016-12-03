using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 地貌定义;
    /// </summary>
    public class Landform : ILandform
    {

        public Landform()
        {
        }
        public Landform(int id) : this()
        {
            this.ID = id;
        }

        /// <summary>
        /// 地形名;
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 地形唯一标示;
        /// </summary>
        [XmlAttribute]
        public int ID { get; set; }

        // 贴图名或路径定义;
        [XmlElement]
        public string diffusePath { get; set; }
        [XmlElement]
        public string heightPath { get; set; }
        [XmlElement]
        public string mixerPath { get; set; }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        [XmlIgnore]
        public Texture DiffuseTexture { get; set; }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        [XmlIgnore]
        public Texture HeightTexture { get; set; }

        /// <summary>
        /// 混合贴图;
        /// </summary>
        [XmlIgnore]
        public Texture MixerTexture { get; set; }

        /// <summary>
        /// 是否已经初始化完毕?
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return DiffuseTexture != null || HeightTexture != null || MixerTexture != null;
            }
        }

        public override string ToString()
        {
            string info = string.Concat(
                "id:", ID,
                " ,name:", Name,
                " ,IsInitialized:", IsInitialized,
                "\n", base.ToString());
            return info;
        }


        /// <summary>
        /// 地貌信息描述文件文件名;
        /// </summary>
        public const string ConfigFileName = "LandformDefinition.xml";

        /// <summary>
        /// 地貌信息描述文件路径;
        /// </summary>
        public static readonly string ConfigFilePath = ResourcePath.CombineConfiguration(ConfigFileName);

        /// <summary>
        /// 将现有地貌定义输出到文件;
        /// </summary>
        public static void Save(List<Landform> landforms)
        {
            Save(landforms, ConfigFilePath);
        }

        /// <summary>
        /// 将地貌定义输出到文件;
        /// </summary>
        public static void Save(List<Landform> landforms, string filePath)
        {
            SerializeHelper.SerializeXml(filePath, landforms);
        }

        /// <summary>
        /// 将地貌信息追加到定义的地貌文件;
        /// </summary>
        public static void Append(IEnumerable<Landform> landforms)
        {
            Append(landforms, ConfigFilePath);
        }

        /// <summary>
        /// 将此地貌结构附加到地貌定义文件中;
        /// </summary>
        public static void Append(IEnumerable<Landform> landforms, string filePath)
        {
            var originalLandforms = Load(filePath);
            originalLandforms.AddRange(landforms);
            Save(originalLandforms, filePath);
        }


        /// <summary>
        /// 从地貌定义文件读取到地貌信息;
        /// </summary>
        public static List<Landform> Load()
        {
            return Load(ConfigFilePath);
        }

        /// <summary>
        /// 从文件读取到地貌信息;
        /// </summary>
        public static List<Landform> Load(string filePath)
        {
            List<Landform> landforms = SerializeHelper.DeserializeXml<List<Landform>>(filePath);
            return landforms;
        }



    }

}
