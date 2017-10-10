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
    class MapReadWriteTest
    {

        [Test]
        public void TestWriteFile()
        {
            var reader = new MapReader();
            var map = Generate();
            reader.WriteToDirectory(NUnit.TempDirectory, map);
        }

        [Test]
        public void TestReadWrite()
        {
            var reader = new MapReader();
            var map = Generate();
            using (var stream = new MemoryStream())
            {
                reader.Write(stream, map);

                stream.Seek(0, SeekOrigin.Begin);
                var newMap = reader.Read(stream);

                Assert.IsTrue(map.Description == newMap.Description);
                Assert.IsTrue(map.Data.IsSameContent(newMap.Data));


                stream.Seek(0, SeekOrigin.Begin);
                var mapDescription = reader.ReadInfo(stream);

                Assert.IsTrue(map.Description == mapDescription);
            }
        }

        Map Generate()
        {
            var points = RectCoord.Spiral_in(RectCoord.Self, 5);
            MapGenerator mapGenerator = new RandomMapGenerator(new MapDescription("测试1"), points);
            Map map = mapGenerator.Generate();
            return map;
        }
    }
}
