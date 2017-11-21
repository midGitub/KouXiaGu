using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using JiongXiaGu.Unity.Resources;
using System.IO;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图数据读取;
    /// </summary>
    public class MapSerializer
    {
        private const string descriptionFileName = "Description" + XmlSerializer<MapDescription>.fileExtension;
        private const string dictionaryFileName = "Map" + ProtoSerializer<MapData>.fileExtension;
        private readonly XmlSerializer<MapDescription> descriptionSerializer = new XmlSerializer<MapDescription>();
        private readonly ProtoSerializer<MapData> mapDataSerializer = new ProtoSerializer<MapData>();

        /// <summary>
        /// 获取到地图;
        /// </summary>
        public Map Deserialize(string directory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从目录获取到地图描述;
        /// </summary>
        public MapDescription DeserializeDesc(string directory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从目录获取到地图数据;
        /// </summary>
        public MapData DeserializeData(string directory)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 获取到地图;
        /// </summary>
        public Map Deserialize(LoadableContent loadableContent, ILoadableEntry entry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从目录获取到地图描述;
        /// </summary>
        public MapDescription DeserializeDesc(LoadableContent loadableContent, ILoadableEntry entry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从目录获取到地图数据;
        /// </summary>
        public MapData DeserializeData(LoadableContent loadableContent, ILoadableEntry entry)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 获取到地图;
        /// </summary>
        public Map Deserialize(Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从目录获取到地图描述;
        /// </summary>
        public MapDescription DeserializeDesc(Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从目录获取到地图数据;
        /// </summary>
        public MapData DeserializeData(Stream stream)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 输出地图到目录;
        /// </summary>
        public void Serialize(string directory, Map map)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输出地图数据到目录;
        /// </summary>
        public void Serialize(string directory, MapData mapData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输出地图描述到目录;
        /// </summary>
        public void Serialize(string directory, MapDescription description)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 输出地图到流;
        /// </summary>
        public void Serialize(Stream stream, Map map)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输出地图数据到流;
        /// </summary>
        public void Serialize(Stream stream, MapData mapData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输出地图描述到流;
        /// </summary>
        public void Serialize(Stream stream, MapDescription description)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 对地图目录进行压缩;
        /// </summary>
        public ZipFile Zip(string directory, FastZip fastZip)
        {
            throw new NotImplementedException();
        }
    }
}
