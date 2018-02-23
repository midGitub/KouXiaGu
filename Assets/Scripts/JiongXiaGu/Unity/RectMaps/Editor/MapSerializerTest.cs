using System;
using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using NUnit.Framework;
using System.IO;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图读写测试;
    /// </summary>
    [TestFixture]
    public class MapSerializerTest
    {
        private MapDescription description = new MapDescription()
        {
            ID = "000",
            Name = "大地图",
        };

        private MapData mapData = new MapData()
        {
            new MapData.NodeItem(new RectCoord(0, 0), new MapNode()),
            new MapData.NodeItem(new RectCoord(0, 1), new MapNode()),
            new MapData.NodeItem(new RectCoord(0, 2), new MapNode()),
        };

        private string GetTempDirectory()
        {
            string directory = Path.Combine(NUnit.TempDirectory, "RectMaps");
            Directory.CreateDirectory(directory);
            return directory;
        }

        private OMap CreateMap()
        {
            OMap map = new OMap(description, mapData);
            return map;
        }

        private void CheckIsSame(MapDescription desc0, MapDescription desc1)
        {
            Assert.AreEqual(desc0.ID, desc1.ID);
            Assert.AreEqual(desc0.Name, desc1.Name);
        }

        private void CheckIsSame(MapData map0, MapData map1)
        {
            Assert.IsTrue(map0.Data.Keys.IsSameContent(map1.Data.Keys));
        }

        [Test]
        public void SerializeInMemory()
        {
            OMap map0 = CreateMap();
            MapSerializer mapSerializer = new MapSerializer();
            Stream stream = new MemoryStream();

            mapSerializer.Serialize(stream, map0);
            stream.Seek(0, SeekOrigin.Begin);
            OMap map1 = mapSerializer.Deserialize(stream);

            CheckIsSame(map0.Description, map1.Description);
            CheckIsSame(map0.MapData, map1.MapData);
        }

        [Test]
        public void SerializeInFile()
        {
            MapSerializer mapSerializer = new MapSerializer();
            OMap map0 = CreateMap();
            OMap map1;
            string filePath = Path.Combine(GetTempDirectory(), "ReadWritTest.zip");

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                mapSerializer.Serialize(stream, map0);
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                map1 = mapSerializer.Deserialize(stream);
            }

            CheckIsSame(map0.Description, map1.Description);
            CheckIsSame(map0.MapData, map1.MapData);
        }

        [Test]
        public void SerializeDescInFile()
        {
            MapDescription desc;
            MapSerializer mapSerializer = new MapSerializer();
            OMap map0 = CreateMap();
            string filePath = Path.Combine(GetTempDirectory(), "ReadDescriptionTest.zip");

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                mapSerializer.Serialize(stream, map0);
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                desc = mapSerializer.DeserializeDesc(stream);
            }

            CheckIsSame(map0.Description, desc);
        }
    }
}
