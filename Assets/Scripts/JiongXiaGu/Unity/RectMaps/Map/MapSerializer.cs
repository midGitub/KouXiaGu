using ICSharpCode.SharpZipLib.Zip;
using JiongXiaGu.Unity.Resources;
using System;
using System.IO;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图数据读取;
    /// </summary>
    public class MapSerializer
    {
        private const string descriptionFileName = "Description" + XmlSerializer<MapDescription>.fileExtension;
        private const string mapFileName = "Map.xml";
        private readonly XmlSerializer<MapDescription> descriptionSerializer = new XmlSerializer<MapDescription>();
        private readonly XmlSerializer<MapData> mapDataSerializer = new XmlSerializer<MapData>();



        /// <summary>
        /// 获取到地图;
        /// </summary>
        public Map Deserialize(Stream stream)
        {
            MapDescription? description = null;
            MapData mapData = null;

            using (var zipInputStream = new ZipInputStream(stream))
            {
                zipInputStream.IsStreamOwner = false;

                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    if (description == null && entry.Name == descriptionFileName)
                    {
                        description = descriptionSerializer.Deserialize(zipInputStream);
                    }
                    else if (mapData == null && entry.Name == mapFileName)
                    {
                        mapData = mapDataSerializer.Deserialize(zipInputStream);
                    }
                }

                if (description != null && mapData != null)
                    return new Map(description.Value, mapData);
                else
                    throw new FileNotFoundException();
            }
        }

        /// <summary>
        /// 获取到地图描述;
        /// </summary>
        public MapDescription DeserializeDesc(Stream stream)
        {
            using (ZipInputStream zipInputStream = new ZipInputStream(stream))
            {
                zipInputStream.IsStreamOwner = false;

                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    if (entry.Name == descriptionFileName)
                    {
                        var description = descriptionSerializer.Deserialize(zipInputStream);
                        return description;
                    }
                }
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 输出地图到流;
        /// </summary>
        public void Serialize(Stream stream, Map map)
        {
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(stream))
            {
                zipOutputStream.IsStreamOwner = false;

                ZipEntry descZipEntry = new ZipEntry(descriptionFileName);
                zipOutputStream.PutNextEntry(descZipEntry);
                descriptionSerializer.Serialize(zipOutputStream, map.Description);

                ZipEntry dictionaryZipEntry = new ZipEntry(mapFileName);
                zipOutputStream.PutNextEntry(dictionaryZipEntry);
                mapDataSerializer.Serialize(zipOutputStream, map.MapData);
            }
        }
    }
}
