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

        

        [Test]
        [Obsolete]
        public void TestWriteFile()
        {
            var reader = new OMapReader();
            var map = Generate();
            reader.WriteToDirectory(NUnit.TempDirectory, map);
        }

        [Test]
        public void TestReadWrite()
        {
            var reader = new OMapReader();
            var map = Generate();
            using (var stream = new MemoryStream())
            {
                reader.Write(stream, map);

                stream.Seek(0, SeekOrigin.Begin);
                var newMap = reader.Read(stream);

                Assert.IsTrue(map.Description.Name == newMap.Description.Name);
                Assert.IsTrue(map.MapData.Data.IsSameContent(newMap.MapData.Data));


                stream.Seek(0, SeekOrigin.Begin);
                var mapDescription = reader.ReadInfo(stream);

                Assert.IsTrue(map.Description.Name == mapDescription.Name);
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
