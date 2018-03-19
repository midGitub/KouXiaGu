using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using NUnit.Framework;

namespace JiongXiaGu.Unity.Maps
{

    /// <summary>
    /// 地图存档测试
    /// </summary>
    [TestFixture]
    public class ArchiveMapTest
    {
        [Test]
        public void Test()
        {
            Map<RectCoord> map = CreateMap();
            MapChangeRecorder<RectCoord> mapChangeRecorder = new MapChangeRecorder<RectCoord>();
            map.Subscribe(mapChangeRecorder);

            map.Add(new RectCoord(1, 1), new MapNode()
            {
                Landform = new NodeLandformInfo()
                {
                    TypeID = "111",
                    Angle = 111,
                },
            });

            map.Remove(new RectCoord(0, 0));

            map[new RectCoord(1, 1)] = new MapNode()
            {
                Landform = new NodeLandformInfo()
                {
                    TypeID = "11",
                    Angle = 11,
                },
            };

            var archiveMap = mapChangeRecorder.GetArchiveMap(map);

            Map<RectCoord> map2 = CreateMap();
            MapChangeRecorder<RectCoord>.AddArchived(map2, archiveMap);

            Contrast.AreSame<RectCoord, MapNode>(map, map2);
        }

        private Map<RectCoord> CreateMap()
        {
            Map<RectCoord> map = new Map<RectCoord>()
            {
                {
                    new RectCoord(0, 0),
                    new MapNode()
                    {
                        Landform = new NodeLandformInfo()
                        {
                            TypeID = "00",
                            Angle = 0,
                        },
                    }
                },
                {
                    new RectCoord(0, 1),
                    new MapNode()
                    {
                        Landform = new NodeLandformInfo()
                        {
                            TypeID = "01",
                            Angle = 1,
                        },
                    }
                },
                {
                    new RectCoord(0, 2),
                    new MapNode()
                    {
                        Landform = new NodeLandformInfo()
                        {
                            TypeID = "02",
                            Angle = 2,
                        },
                    }
                },
            };

            return map;
        }
    }
}
