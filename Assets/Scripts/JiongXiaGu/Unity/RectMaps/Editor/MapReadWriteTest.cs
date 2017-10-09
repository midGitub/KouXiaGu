using JiongXiaGu.Collections;
using NUnit.Framework;
using System.IO;

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
            reader.Write(@"NUnitTemp", map);
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

                Assert.IsTrue(map.Description == newMap.Description);
                Assert.IsTrue(map.Data.IsSameContent(newMap.Data));


                stream.Seek(0, SeekOrigin.Begin);
                var mapDescription = MapXmlReader.ReadInfo(stream);

                Assert.IsTrue(map.Description == mapDescription);
            }
        }

        Map Generate()
        {
            Map map = new Map("测试1");
            MapDataGenerator mapDataGenerator = new MapDataGenerator();
            mapDataGenerator.Generate(map.Data, 5);
            return map;
        }
    }
}
