using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图读写测试;
    /// </summary>
    [TestFixture]
    public class MapReadWriteTest
    {

        [Test]
        public void TestWriteFile()
        {
            var reader = new MapXmlReader();
            var map = Generate();
            FileInfo fileInfo = new FileInfo(@"NUnitTemp/map.xml");
            using (var stream = fileInfo.Create())
            {
                reader.Write(stream, map);
            }
        }

        [Test]
        public void TestReadWrite()
        {
            var reader = new MapXmlReader();
            var map = Generate();
            using (var stream = new MemoryStream())
            {
                reader.Write(stream, map);

                stream.Seek(0, SeekOrigin.Begin);
                var newMap = reader.Read(stream);

                CheckIsSame(map, newMap);
            }
        }

        Map Generate()
        {
            Map map = new Map("测试1", 1);
            MapDataGenerator mapDataGenerator = new MapDataGenerator();
            mapDataGenerator.Generate(map.Data.Data, 5);
            return map;
        }

        void CheckIsSame(Map map, Map other)
        {
            Assert.AreEqual(map.Name, other.Name);
            Assert.AreEqual(map.Version, other.Version);
            Assert.IsTrue(map.Data.Data.IsSameContent(other.Data.Data));
        }
    }
}
